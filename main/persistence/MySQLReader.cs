using MySqlConnector;
using ProductiveHoursTracker.model;
using ProductiveHoursTracker.model.exceptions;

namespace ProductiveHoursTracker.persistence;

public class MySqlConnector
{
    private const string ConnectionString = "jdbc:mysql://localhost:3306/";
    private MySqlConnection _connection;

    public MySqlConnector()
    {
        _connection = new MySqlConnection(ConnectionString);
        // new MySqlConnection($"Server={server};User ID={userID};Password={password};Database={database}"))
    }

    public UserList GetUserList()
    {
        using (_connection)
        {
            _connection.Open();
            UserList userList = new UserList();


            string query = "SELECT * FROM Users";
            using (var command = new MySqlCommand(query, _connection))
            {
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string? id = reader["user_id"].ToString();
                    string? name = reader["name"].ToString();
                    if (id == null || name == null) throw new InvalidUserException();
                    var userId = Guid.Parse(id);
                    userList.Add(name, userId);
                }

                return userList;
            }
        }
    }

    public void SaveUser(User user)
    {
        using (_connection)
        {
            _connection.Open();

            List<ProductivityEntry> entries = user.ProductivityLog.Entries;

            // Insert User record (if it doesn't exist)
            string userInsertQuery =
                "INSERT INTO Users (user_id, name) VALUES (@userId, @name) ON DUPLICATE KEY UPDATE name = @name";
            using (var insertCommand = new MySqlCommand(userInsertQuery, _connection))
            {
                insertCommand.Parameters.AddWithValue("@userId", user.Id);
                insertCommand.Parameters.AddWithValue("@name", user.Name);
                insertCommand.ExecuteNonQuery();
            }

            // Clear existing entries for the user (prevents duplicates)
            string deleteEntriesQuery = "DELETE FROM Entries WHERE user_id = @userId";
            using (var deleteCommand = new MySqlCommand(deleteEntriesQuery, _connection))
            {
                deleteCommand.Parameters.AddWithValue("@userId", user.Id);
                deleteCommand.ExecuteNonQuery();
            }

            // Insert each productivity entry
            foreach (var entry in entries)
            {
                string entryInsertQuery =
                    "INSERT INTO Entries (user_id, date, time, label, level) VALUES (@userId, @date, @time, @label, @level)";
                using (var insertCommand = new MySqlCommand(entryInsertQuery, _connection))
                {
                    insertCommand.Parameters.AddWithValue("@userId", user.Id);
                    insertCommand.Parameters.AddWithValue("@date", entry.Date);
                    insertCommand.Parameters.AddWithValue("@time", entry.Time);
                    insertCommand.Parameters.AddWithValue("@label", entry.GetLabel());
                    insertCommand.Parameters.AddWithValue("@level", entry.Level);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }
    }

    public User LoadUser(string name)
    {
        using (_connection)
        {
            _connection.Open();
            Guid userId;
            string nameQuery =
                "SELECT * FROM Users WHERE name = @name"; // Select only needed columns if performance is critical
            using (var command = new MySqlCommand(nameQuery, _connection))
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
            using (var command = new MySqlCommand(query, _connection))
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
}