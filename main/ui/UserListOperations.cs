namespace ProductiveHoursTracker.ui;

using System;
using System.Collections.Generic;
using System.IO;
using model; // Assuming model folder contains User.cs, UserList.cs, exceptions

public class UserListOperations
{
    private static readonly List<string> OPS = new List<string>() { "register", "login", "show users", "quit" };

    private UserList users;
    private UserListScanner input;
    private Scanner scanner;

    public UserListOperations()
    {
        scanner = new Scanner(Console.In);

        JsonReadUserList reader = new JsonReadUserList();
        try
        {
            users = new UserList(reader.Read()); // Read data here
        }
        catch (IOException e)
        {
            // Maybe write something here?  // Consider logging the error
            // When does this happen? Shouldn't data/users.json always exist? (Handle potential missing file)
        }

        input = new UserListScanner(scanner);
        processOperations();
    }

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
                    user = registerUser();
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

    private void startSessionIfUserValid(User user)
    {
        if (user != null)
        {
            UserOperations operationRecord = new UserOperations(user, scanner);
            if (operationRecord.WasSaved())
            {
                save();
            }
        }
    }

    public User registerUser()
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

    public User loginUser()
    {
        string name = input.Name();

        try
        {
            return users.LoadUser(name);
        }
        catch (InvalidUserException e)
        {
            Console.WriteLine("User not found");
            return null;
        }
    }

    public void listUsers()
    {
        Console.WriteLine("\nThe users are: ");

        foreach (string name in users.GetNames())
        {
            Console.WriteLine("\t - " + name);
        }

        Console.WriteLine();
    }

    public void save()
    {
        try
        {
            JsonWriter writer = new JsonWriter("users.json");

            writer.Open();
            writer.Write(users);
            writer.Close();
        }
        catch (IOException e)
        {
            Console.WriteLine("An error occurred while saving the data."); // More informative message
        }
    }
}