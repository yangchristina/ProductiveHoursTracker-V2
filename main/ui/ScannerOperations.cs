using System.Text;

namespace ProductiveHoursTracker.ui;

using System.Collections.Generic;

public class ScannerOperations
{
    // EFFECTS: returns input, and makes sure input is a value in inputOptions
    public string ValidateInput(List<string> inputOptions)
    {
        StringBuilder message = BuildOperationInstructions(inputOptions);
        while (true)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            if (inputOptions.Contains(input))
            {
                return input;
            }

            Console.WriteLine("Invalid input. Please try again.");
            Console.WriteLine();
        }
    }

    // EFFECTS: connects the strings in inputOptions to create a message of what inputs to enter and returns message
    private StringBuilder BuildOperationInstructions(List<string> inputOptions)
    {
        StringBuilder message = new StringBuilder("Select ");
        int i = 0;
        for (; i < inputOptions.Count - 1; i++)
        {
            message.Append(inputOptions[i]).Append(", ");
        }

        message.Append("or ").Append(inputOptions[i]).Append(".");
        return message;
    }
}