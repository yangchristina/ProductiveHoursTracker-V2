using MySqlConnector;
using ProductiveHoursTracker.model;
using ProductiveHoursTracker.model.exceptions;

namespace ProductiveHoursTracker.persistence;

public class MySqlConnector
{
    private readonly string ConnectionString = $"server={Environment.GetEnvironmentVariable("MYSQL_HOST")};" +
                                               $"port=3306;" +
                                               $"UserId={Environment.GetEnvironmentVariable("MYSQL_USERNAME")};" +
                                               $"Password={Environment.GetEnvironmentVariable("MYSQL_PASSWORD")};" +
                                               $"Database={Environment.GetEnvironmentVariable("MYSQL_DATABASE")};";

    // private const string ConnectionString = "jdbc:mysql://localhost:3306/";
    private static MySqlConnection _connection;

    public MySqlConnector()
    {
        // _connection = new MySqlConnection(ConnectionString);
    }

    public UserList GetUserList()
    {
        using (_connection = new MySqlConnection(ConnectionString))
        {
            _connection.Open();


            string query = "SELECT * FROM Users";
            using (var command = new MySqlCommand(query, _connection))
            {
                UserList userList = new UserList(this);
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
    
    public void SaveEntry(User user, ProductivityEntry entry)
    {
        using (_connection = new MySqlConnection(ConnectionString))
        {
            _connection.Open();
            
            // Insert User record (if it doesn't exist)
            string userInsertQuery =
                "INSERT INTO Entries (user_id, date, time, label, level) VALUES (@userId, @date, @time, @label, @level) ON DUPLICATE KEY UPDATE level = @level";
            using (var insertCommand = new MySqlCommand(userInsertQuery, _connection))
            {
                insertCommand.Parameters.AddWithValue("@userId", user.Id.ToString());
                insertCommand.Parameters.AddWithValue("@date", entry.Date);
                insertCommand.Parameters.AddWithValue("@time", entry.Time.ToString());
                insertCommand.Parameters.AddWithValue("@label", entry.GetLabel().ToString());
                insertCommand.Parameters.AddWithValue("@level", entry.Level);
                insertCommand.ExecuteNonQuery();
            }
        }
    }

    public void SaveUser(User user)
    {
        using (_connection = new MySqlConnection(ConnectionString))
        {
            _connection.Open();

            List<ProductivityEntry> entries = user.ProductivityLog.Entries;

            // Insert User record (if it doesn't exist)
            string userInsertQuery =
                "INSERT INTO Users (user_id, name) VALUES (@userId, @name) ON DUPLICATE KEY UPDATE name = @name";
            using (var insertCommand = new MySqlCommand(userInsertQuery, _connection))
            {
                insertCommand.Parameters.AddWithValue("@userId", user.Id.ToString());
                insertCommand.Parameters.AddWithValue("@name", user.Name);
                insertCommand.ExecuteNonQuery();
            }
        //
        //     // Clear existing entries for the user (prevents duplicates)
        //     string deleteEntriesQuery = "DELETE FROM Entries WHERE user_id = @userId";
        //     using (var deleteCommand = new MySqlCommand(deleteEntriesQuery, _connection))
        //     {
        //         deleteCommand.Parameters.AddWithValue("@userId", user.Id);
        //         deleteCommand.ExecuteNonQuery();
        //     }
        //
        //     // Insert each productivity entry
        //     foreach (var entry in entries)
        //     {
        //         string entryInsertQuery =
        //             "INSERT INTO Entries (user_id, date, time, label, level) VALUES (@userId, @date, @time, @label, @level)";
        //         using (var insertCommand = new MySqlCommand(entryInsertQuery, _connection))
        //         {
        //             insertCommand.Parameters.AddWithValue("@userId", user.Id);
        //             insertCommand.Parameters.AddWithValue("@date", entry.Date);
        //             insertCommand.Parameters.AddWithValue("@time", entry.Time);
        //             insertCommand.Parameters.AddWithValue("@label", entry.GetLabel());
        //             insertCommand.Parameters.AddWithValue("@level", entry.Level);
        //             insertCommand.ExecuteNonQuery();
        //         }
        //     }
        }
    }

    public User LoadUser(string name)
    {
        using (_connection = new MySqlConnection(ConnectionString))
        {
            _connection.Open();
            Guid userId;
            string nameQuery =
                "SELECT * FROM Users WHERE name = @name"; // Select only needed columns if performance is critical
            var ucommand = new MySqlCommand(nameQuery, _connection);
            // {
            
            ucommand.Parameters.AddWithValue("@name", name);
            var ureader = ucommand.ExecuteReader();
            if (ureader.Read())
            {
                string? id = ureader["user_id"].ToString();
                if (id == null) throw new InvalidUserException();
                userId = Guid.Parse(id);
            }
            else
            {
                throw new InvalidUserException();
            }
            _connection.Close();

            _connection.Open();
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

                return new User(name, userId, entries);
            }
        }
    }
}