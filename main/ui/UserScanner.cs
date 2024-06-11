using ProductiveHoursTracker.ui.exceptions;

public class UserScanner
{
    private ConsoleKeyInfo _keyInfo;

    // EFFECTS: returns the key value of an entry
    public int ItemKey()
    {
        while (true)
        {
            Console.WriteLine("Enter key of item");

            string input = Console.ReadLine();
            int key;
            if (!Int32.TryParse(input, out key))
            {
                //no, not able to parse, repeat, throw exception, use fallback value?
                continue;
            }

            return key;
        }
    }

    // EFFECTS: returns yes or no, depending on userInput
    public bool YesOrNo()
    {
        Console.WriteLine(" (enter yes or no)");
        string ans = Console.ReadLine().ToLower();
        if (ans == "yes")
        {
            return true;
        }
        if (ans == "no")
        {
            return false;
        }
        throw new InvalidInputException("Invalid input. Please enter yes or no.");
    }

    // EFFECTS: returns the key value of an entry
    public string EntryType()
    {
        while (true)
        {
            Console.WriteLine("Select energy, focus, or motivation");
            string label = Console.ReadLine().ToLower();
            if (label == "energy" || label == "focus" || label == "motivation")
            {
                return label;
            }
            Console.WriteLine("Invalid entry. Please try again");
            Console.WriteLine();
        }
    }

    // EFFECTS: Asks user to input a time of day and returns it
    public TimeSpan Time()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("Enter -1 to select right now, otherwise enter the hour [0, 23])");
                int hour = int.Parse(Console.ReadLine());
                if (hour == -1)
                {
                    return DateTime.Now.TimeOfDay;
                }
                if (hour >= 0 && hour < 24)
                {
                    return TimeSpan.FromHours(hour);
                }
                Console.WriteLine("Invalid hour. Please try again.");
                Console.WriteLine();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid hour. Please try again.");
                Console.WriteLine();
            }
        }
    }

    // EFFECTS: Asks user to input value for level and returns it
    public int Level(string entryType)
    {
        while (true)
        {
            Console.WriteLine("Enter your " + entryType + " level, out of 10");
            try
            {
                int level = int.Parse(Console.ReadLine());
                if (level <= 10 && level >= 0)
                {
                    return level;
                }
                Console.WriteLine("Invalid " + entryType + " level, please try again.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid " + entryType + " level, please try again.");
            }
        }
    }
}
