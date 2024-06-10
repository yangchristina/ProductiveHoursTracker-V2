// using System.Text.Json.Nodes;
// using ProductiveHoursTracker.model.exceptions;
// using ProductiveHoursTracker.persistence;
//
// namespace ProductiveHoursTracker.model;
//
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Text.Json; // assuming using Newtonsoft.Json is not preferred
//
// public class UserList : Writable
// {
//     public const string JSON_USER_LIST = "./data/users.json";
//     private readonly Dictionary<string, Guid> users;
//
//     // EFFECTS: constructs a user list with a given users
//     public UserList(Dictionary<string, Guid> users)
//     {
//         this.users = users;
//     }
//
//     // EFFECTS: if user is not in users, then call add method to add it to users, else throw UserAlreadyExistsException
//     public void Register(User user)
//     {
//         if (users.ContainsKey(user.Name))
//         {
//             throw new UserAlreadyExistsException();
//         }
//
//         Add(user);
//     }
//
//     // MODIFIES: this
//     // EFFECTS: adds an entry to users where the user's name is the key, and the user's id is the value
//     public void Add(User user)
//     {
//         users.Add(user.Name, user.Id);
//     }
//
//     // EFFECTS: loads user from file by name and returns it
//     public User LoadUser(string name)
//     {
//         Guid userId = GetUserId(name); // throws InvalidUserException if name is invalid
//
//         JsonReadUser reader = new JsonReadUser(userId.ToString());
//         try
//         {
//             return reader.Read();
//         }
//         catch (IOException e)
//         {
//             // This exception shouldn't be caught because all valid users have a file.
//             // Consider logging the error instead.
//             return null;
//         }
//     }
//
//     // EFFECTS: returns user id from users with given name, null otherwise
//     public Guid GetUserId(string name)
//     {
//         if (!users.TryGetValue(name, out Guid id))
//         {
//             throw new InvalidUserException();
//         }
//
//         return id;
//     }
//
//     // EFFECTS: returns number of users
//     public int Size()
//     {
//         return users.Count;
//     }
//
//     // EFFECTS: returns true if user list is empty, else false
//     public bool IsEmpty()
//     {
//         return users.Count == 0;
//     }
//
//     // EFFECTS: returns names of all users in user list
//     public HashSet<string> GetNames()
//     {
//         return new HashSet<string>(users.Keys);
//     }
//
//     public JsonObject ToJson()
//     {
//         JsonObject json = new JsonObject();
//         json.Add("users", UserListToJson());
//         return json;
//     }
//
//     // EFFECTS: returns users as a JSON array
//     private JsonArray UserListToJson()
//     {
//         JsonArray jsonArray = new JsonArray();
//
//         foreach (KeyValuePair<string, Guid> user in users)
//         {
//             jsonArray.Add(UserInfoToJson(user));
//         }
//
//         return jsonArray;
//     }
//
//     // EFFECTS: returns user as a json object
//     private JsonObject UserInfoToJson(KeyValuePair<string, Guid> user)
//     {
//         JsonObject json = new JsonObject();
//
//         json.Add("name", user.Key);
//         json.Add("id", user.Value.ToString());
//
//         return json;
//     }
// }