namespace ProductiveHoursTracker.ui;

public class UserListScanner
{
    public string Name()
    {
        string? name;
        while (true)
        {
            Console.WriteLine("Enter name");
            name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name)) // Check for empty or null string
            {
                return name;
            }
            Console.WriteLine("Invalid input. Please try again.");
        }
    }
}