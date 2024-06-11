using System.Reflection.Emit;
using System.Text.Json.Nodes;
using MySqlConnector;
using ProductiveHoursTracker.model.exceptions;
using ProductiveHoursTracker.persistence;

namespace ProductiveHoursTracker.model;

using System;
using System.Collections.Generic;
using System.IO;

public class UserList
{
    private readonly Dictionary<string, Guid> _users;
    
    public UserList()
    {
        _users = new Dictionary<string, Guid>();
    }

    // EFFECTS: constructs a user list with a given users
    public UserList(Dictionary<string, Guid> users)
    {
        _users = users;
    }

    // EFFECTS: if user is not in users, then call add method to add it to users, else throw UserAlreadyExistsException
    public void Register(User user)
    {
        if (_users.ContainsKey(user.Name))
        {
            throw new UserAlreadyExistsException();
        }

        Add(user);
    }

    // MODIFIES: this
    // EFFECTS: adds an entry to users where the user's name is the key, and the user's id is the value
    public void Add(User user)
    {
        _users.Add(user.Name, user.Id);
    }
    
    public void Add(string name, Guid id)
    {
        _users.Add(name, id);
    }

    // EFFECTS: loads user from file by name and returns it
    public User LoadUser(string name)
    {
        using (var connection = new MySqlConnection("jdbc:mysql://localhost:3306/")
               // new MySqlConnection($"Server={server};User ID={userID};Password={password};Database={database}"))
              ) // Replace with your connection string
        {
            connection.Open();
            Guid userId;
            string nameQuery =
                "SELECT * FROM Users WHERE name = @name"; // Select only needed columns if performance is critical
            using (var command = new MySqlCommand(nameQuery, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string? id = reader["id"].ToString();
                    if (id == null) throw new InvalidUserException();
                    userId = Guid.Parse(id);
                }
                else
                {
                    throw new InvalidUserException();
                }
            }
            List<ProductivityEntry> entries = new List<ProductivityEntry>();

            string query =
                "SELECT e.date, e.time, e.label, e.level FROM Entries e INNER JOIN Users u ON e.user_id = u.user_id WHERE u.user_id = @userId";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ProductivityEntry.Label.TryParse(reader.GetString("label"), out ProductivityEntry.Label label);
                    entries.Add(new ProductivityEntry(
                        label, 
                        reader.GetDateTime("date"), 
                        reader.GetTimeSpan("time"), 
                        reader.GetInt32("level")
                    ));
                }
            }

            return new User(name, userId, entries);
        }
    }

    // EFFECTS: returns user id from users with given name, null otherwise
    public Guid GetUserId(string name)
    {
        if (!_users.TryGetValue(name, out Guid id))
        {
            throw new InvalidUserException();
        }

        return id;
    }

    // EFFECTS: returns number of users
    public int Size()
    {
        return _users.Count;
    }

    // EFFECTS: returns true if user list is empty, else false
    public bool IsEmpty()
    {
        return _users.Count == 0;
    }

    // EFFECTS: returns names of all users in user list
    public HashSet<string> GetNames()
    {
        return new HashSet<string>(_users.Keys);
    }

    public JsonObject ToJson()
    {
        JsonObject json = new JsonObject();
        json.Add("users", UserListToJson());
        return json;
    }

    // EFFECTS: returns users as a JSON array
    private JsonArray UserListToJson()
    {
        JsonArray jsonArray = new JsonArray();

        foreach (KeyValuePair<string, Guid> user in _users)
        {
            jsonArray.Add(UserInfoToJson(user));
        }

        return jsonArray;
    }

    // EFFECTS: returns user as a json object
    private JsonObject UserInfoToJson(KeyValuePair<string, Guid> user)
    {
        JsonObject json = new JsonObject();

        json.Add("name", user.Key);
        json.Add("id", user.Value.ToString());

        return json;
    }
}