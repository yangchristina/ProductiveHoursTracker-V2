namespace ProductiveHoursTracker.ui;

using System;
using System.Collections.Generic;
using model;
using ui.exceptions;

public class UserOperations
{
    private static readonly List<string> OPS = new List<string>
    {
        "logout", "add", "peak", "trough", "show", "edit", "delete", "save", "help"
    };

    private User user;
    private UserScanner input;
    private bool wasSaved;

    // EFFECTS: constructs a UserOperations with a given user and a UserScanner
    public UserOperations(User user)
    {
        this.user = user;
        input = new UserScanner();
        wasSaved = false;
        Console.WriteLine("Welcome " + user.Name + "!");
        ProcessOperations();
        Console.WriteLine("End operations");
    }

    // EFFECTS: prints out a message for when the input is invalid
    private void InvalidInputMessage()
    {
        Console.WriteLine("Invalid input. Please try again");
    }

    // EFFECTS: calls a method depending on the input value
    private void ProcessOperations()
    {
        while (true)
        {
            Console.WriteLine("Type command or enter help to see commands:");
            string operation = input.ValidateInput(OPS);

            switch (operation)
            {
                case "logout":
                    PromptSave();
                    return;
                case "add":
                    AddEntries();
                    break;
                case "peak":
                    ShowPeakHours();
                    break;
                case "trough":
                    ShowTroughHours();
                    break;
                case "show":
                    ProcessShowEntries();
                    break;
                case "edit":
                    EditEntry();
                    break;
                case "delete":
                    RemoveEntry();
                    break;
                case "save":
                    Save();
                    break;
                case "help":
                    Console.WriteLine("logout, add, peak, show, edit, delete, save, help");
                    break;
            }

            Console.WriteLine();
        }
    }

    // MODIFIES: this
    // EFFECTS: lets the user enter values to create a new productivity entry, and add it to the user's log
    private void AddEntries()
    {
        DateTime localDateTime = DateTime.Now;
        TimeSpan localTime = DateTime.Now.TimeOfDay;

        int energyLevel = input.Level("energy");
        var energyEntry = new ProductivityEntry(ProductivityEntry.Label.Energy,localDateTime, localTime, energyLevel);

        int focusLevel = input.Level("focus");
        var focusEntry = new ProductivityEntry(ProductivityEntry.Label.Focus, localDateTime, localTime, focusLevel);

        int motivationLevel = input.Level("motivation");
        var motivationEntry = new ProductivityEntry(ProductivityEntry.Label.Motivation, localDateTime, localTime, motivationLevel);

        user.ProductivityLog.Add(energyEntry);
        user.ProductivityLog.Add(energyEntry);
        user.ProductivityLog.Add(focusEntry);
        user.ProductivityLog.Add(motivationEntry);

        Console.WriteLine("You have added " + energyEntry);
        Console.WriteLine("You have added " + focusEntry);
        Console.WriteLine("You have added " + motivationEntry);
    }

    // EFFECTS: selects an entry based on category and key input by user
    private ProductivityEntry SelectEntry()
    {
        List<string> options = new List<string> { "energy", "focus", "motivation", "cancel" };

        while (true)
        {
            string category = input.ValidateInput(options);
            Console.WriteLine(category);
            int key = input.ItemKey();
            
            try
            {
                switch (category)
                {
                    case "energy":
                    case "focus":
                    case "motivation":
                        return user.ProductivityLog.Entries[key - 1];
                        // return user.GetFocusEntries()[key - 1];
                        // return user.GetMotivationEntries()[key - 1];
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("exception");
                InvalidInputMessage();
            }
        }
    }

    // MODIFIES: this
    // EFFECTS: edits selected entry according to user inputs
    private void EditEntry()
    {
        ProductivityEntry entry = SelectEntry();

        while (true)
        {
            List<string> options = new List<string> { "level", "time", "quit" };

            string entryValue = input.ValidateInput(options);
            switch (entryValue)
            {
                case "quit":
                    return;
                case "level":
                    string label = entry.GetLabel().ToString();
                    int level = input.Level(label);
                    entry.EditLevel(level);
                    break;
                case "time":
                    DateTime time = input.Time();
                    entry.EditTime(time);
                    break;
            }
        }
    }

    // MODIFIES: this
    // EFFECTS: removes selected entry from the productivity log
    public void RemoveEntry()
    {
        Console.WriteLine("Operation: remove");
        ProductivityEntry entry = SelectEntry();
        // bool isRemoved = 
         double? removal = user.ProductivityLog.Remove(entry);
        if (removal == null)
        {
            Console.WriteLine("Removed " + entry);
        }
    }

    // EFFECTS: saves user to file
    public void Save()
    {
        try
        {
            JsonWriter writer = new JsonWriter(user.GetId().ToString());

            writer.Open();
            writer.Write(user);
            writer.Close();

            wasSaved = true;
        }
        catch (IOException)
        {
            // idk what yet, shouldn't ever be thrown because no illegal file names, all filenames are uuid
        }
    }

    // asks user if they would like to save their session
    // EFFECTS: if user inputs true, save session, if false don't save, else ask again
    private void PromptSave()
    {
        Console.WriteLine("Would you like to save?");
        try
        {
            bool ans = input.YesOrNo();
            if (ans)
            {
                Save();
            }
        }
        catch (InvalidInputException)
        {
            PromptSave();
        }
    }

    // EFFECTS: shows the user's peak hours for either focus, energy, or motivation, depending on the user's input
    private void ShowPeakHours()
    {
        string label = input.EntryType();
        List<DateTime> peakHours = user.ProductivityLog.DailyAverageLog.GetPeaksAndTroughs(label)["peak"];
        if (peakHours.Count == 0)
        {
            Console.WriteLine("Not enough " + label + " entries");
        }
        else
        {
            Console.WriteLine("Your peak " + label + " hours are at " + string.Join(", ", peakHours));
        }
    }

    // EFFECTS: shows the user's trough hours for either focus, energy, or motivation, depending on the user's input
    private void ShowTroughHours()
    {
        string label = input.EntryType();
        List<DateTime> troughHours = user.GetPeaksAndTroughs(label)["trough"];
        if (troughHours.Count == 0)
        {
            Console.WriteLine("Not enough " + label + " entries");
        }
        else
        {
            Console.WriteLine("Your trough " + label + " hours are at " + string.Join(", ", troughHours));
        }
    }

    // EFFECTS: shows the user log indicated by the input options, by calling a method depending on the input
    private void ProcessShowEntries()
    {
        List<string> options = new List<string> { "all", "energy", "focus", "motivation" };

        string entryType = input.ValidateInput(options);
        switch (entryType)
        {
            case "all":
                ShowAllEntries();
                break;
            case "energy":
                ShowAllEnergyEntries();
                break;
            case "focus":
                ShowAllFocusEntries();
                break;
            case "motivation":
                ShowAllMotivationEntries();
                break;
        }
    }

    // EFFECTS: prints out the given list for the user to see
    private void ShowEntries(List<ProductivityEntry> productivityEntries)
    {
        if (productivityEntries.Count == 0)
        {
            return;
        }

        Console.WriteLine(productivityEntries[0].Label() + " entries:");
        int key = 1;
        foreach (ProductivityEntry entry in productivityEntries)
        {
            Console.WriteLine(entry.Description(key));
            key++;
        }
    }

    // EFFECTS: shows details for all entries
    private void ShowAllEntries()
    {
        Console.WriteLine("Your entries are: ");
        ShowAllEnergyEntries();
        ShowAllFocusEntries();
        ShowAllMotivationEntries();
        Console.WriteLine();
    }

    // EFFECTS: shows details for all energy logs
    private void ShowAllEnergyEntries()
    {
        ShowEntries(user.GetEnergyEntries());
    }

    // EFFECTS: shows details for all focus logs
    private void ShowAllFocusEntries()
    {
        ShowEntries(user.GetFocusEntries());
    }

    // EFFECTS: shows details for all motivation logs
    private void ShowAllMotivationEntries()
    {
        ShowEntries(user.GetMotivationEntries());
    }

    public bool WasSaved()
    {
        return wasSaved;
    }
}

public class UserScanner
{
    public string ValidateInput(List<string> options)
    {
        while (true)
        {
            string input = Console.ReadLine().Trim().ToLower();
            if (options.Contains(input))
            {
                return input;
            }

            Console.WriteLine("Invalid input. Please try again.");
        }
    }

    public int Level(string label)
    {
        while (true)
        {
            Console.WriteLine("Enter " + label + " level (1-10):");
            if (int.TryParse(Console.ReadLine(), out int level) && level >= 1 && level <= 10)
            {
                return level;
            }

            Console.WriteLine("Invalid input. Please enter a number between 1 and 10.");
        }
    }

    public int ItemKey()
    {
        while (true)
        {
            Console.WriteLine("Enter item key:");
            if (int.TryParse(Console.ReadLine(), out int key) && key > 0)
            {
                return key;
            }

            Console.WriteLine("Invalid input. Please enter a valid item key.");
        }
    }

    public TimeSpan Time()
    {
        while (true)
        {
            Console.WriteLine("Enter time (HH:MM):");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime time))
            {
                return time.TimeOfDay;
            }

            Console.WriteLine("Invalid input. Please enter time in HH:MM format.");
        }
    }

    public bool YesOrNo()
    {
        while (true)
        {
            Console.WriteLine("Enter yes or no:");
            string input = Console.ReadLine().Trim().ToLower();
            if (input == "yes")
            {
                return true;
            }
            else if (input == "no")
            {
                return false;
            }

            Console.WriteLine("Invalid input. Please enter yes or no.");
        }
    }

    public ProductivityEntry.Label EntryType()
    {
        List<string> options = new List<string> { "energy", "focus", "motivation" };
        Console.WriteLine("Enter entry type (energy, focus, motivation):");
        string input = ValidateInput(options);
        ProductivityEntry.Label.TryParse(input, true, out ProductivityEntry.Label entry);
        return entry;
    }
}