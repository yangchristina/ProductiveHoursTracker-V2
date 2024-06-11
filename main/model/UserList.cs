using ProductiveHoursTracker.model.exceptions;

namespace ProductiveHoursTracker.model;

using System;
using System.Collections.Generic;

public class UserList
{
    private readonly Dictionary<string, Guid> _users;
    private readonly persistence.MySqlConnector _sqlConnector;
    
    public UserList(persistence.MySqlConnector sqlConnector)
    {
        _sqlConnector = sqlConnector;
            // new persistence.MySqlConnector();
        _users = new Dictionary<string, Guid>();
    }
    
    
    // EFFECTS: if user is not in users, then call add method to add it to users, else throw UserAlreadyExistsException
    public void Register(User user)
    {
        if (_users.ContainsKey(user.Name))
        {
            throw new UserAlreadyExistsException();
        }
        
        _sqlConnector.SaveUser(user);

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
        return _sqlConnector.LoadUser(name);
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
}