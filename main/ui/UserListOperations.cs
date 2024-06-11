namespace ProductiveHoursTracker.ui;

using ProductiveHoursTracker.model;
using ProductiveHoursTracker.model.exceptions;
using System;
using System.Collections.Generic;
using System.IO;

// calls user list methods through console inputs
public class UserListOperations
{
    private static readonly List<string> OPS = new List<string>() { "register", "login", "show users", "quit" };

    private UserList users;
    private UserListScanner input;

    // EFFECTS: constructs an empty user list and initializes scanner and UserListScanner
    public UserListOperations()
    {
        // JsonReadUserList reader = new JsonReadUserList();
        try
        {
            // TODO
            users = new UserList([]); // Read data here
        }
        catch (IOException e)
        {
            // Maybe write some message
            // Consider logging the exception for debugging
        }

        input = new UserListScanner(); // Use Console instead of Scanner
        processOperations();
    }

    // MODIFIES: this
    // EFFECTS: processes operations input by users
    private void processOperations()
    {
        while (true)
        {
            string operation = input.ValidateInput(OPS);
            User user = null;
            switch (operation)
            {
                case "quit":
                    return;
                case "register":
                    user = registerUser(); // ask again
                    break;
                case "login":
                    user = loginUser();
                    break;
                case "show users":
                    listUsers();
                    continue;
            }
            startSessionIfUserValid(user);
        }
    }

    // EFFECTS: if user is not null, starts the session for the user. After session, if user was saved, save user list
    private void startSessionIfUserValid(User user)
    {
        if (user != null)
        {
            UserOperations operationRecord = new UserOperations(user); // Use Console instead of Scanner
            if (operationRecord.WasSaved())
            {
                save();
            }
        }
    }

    // REQUIRES: user is not yet in user list, name is not the empty string
    // MODIFIES: this
    // EFFECTS: adds user to end of user list
    public User registerUser() // Put in UserList
    {
        string name = input.Name();
        User user = new User(name);
        try
        {
            users.Register(user);
        }
        catch (UserAlreadyExistsException e)
        {
            user = null;
            Console.WriteLine("User already exists");
            Console.WriteLine();
        }
        return user;
    }

    // EFFECTS: returns user with the given name, or null if there are no users with name in user list
    public User loginUser()
    {
        string name = input.Name();

        try
        {
            return users.LoadUser(name);
        }
        catch (InvalidUserException e) // Create this exception
        {
            Console.WriteLine("User not found");
            return null;
        }
    }

    // EFFECTS: lists all users in list
    public void listUsers() // Put in UserList
    {
        Console.WriteLine("\nThe users are: ");

        foreach (string name in users.GetNames())
        {
            Console.WriteLine("\t - " + name);
        }

        Console.WriteLine();
    }

    // EFFECTS: saves user list to file
    public void save()
    {
        try
        {
            // TODO
            // JsonWriter writer = new JsonWriter("users");
            //
            // writer.Open();
            // writer.Write(users);
            // writer.Close();
        }
        catch (IOException e)
        {
            Console.WriteLine("An error occurred during saving");
        }
    }
}
