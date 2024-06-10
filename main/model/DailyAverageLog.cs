namespace ProductiveHoursTracker.model;

public class DailyAverageLog
{
    private Dictionary<ProductivityEntry.Label, SortedDictionary<TimeSpan, double>> averageLog;
    private Dictionary<ProductivityEntry.Label, int[]> counts;

    // EFFECTS: constructs DailyAverageLog with an empty counts and an empty averageLog
    public DailyAverageLog()
    {
        counts = CreateEmptyCounts();
        averageLog = CreateEmptyLog();
    }

    // EFFECTS: constructs DailyAverageLog with a given list of productivity entries,
    //          and a log of levels by time of day, which is calculated from the productivity entries
    public DailyAverageLog(List<ProductivityEntry> entries)
    {
        counts = CreateEmptyCounts();
        averageLog = CreateEmptyLog();
        InitAverageLog(entries);
    }

    // EFFECTS: returns a dictionary with a key for all productivity entries and empty int[] as values
    private static Dictionary<ProductivityEntry.Label, int[]> CreateEmptyCounts()
    {
        var counts = new Dictionary<ProductivityEntry.Label, int[]>();
        foreach (ProductivityEntry.Label label in Enum.GetValues(typeof(ProductivityEntry.Label)))
        {
            counts[label] = new int[24];
        }

        return counts;
    }

    // EFFECTS: returns a dictionary with a key for all productivity entries and empty SortedDictionary as values
    private static Dictionary<ProductivityEntry.Label, SortedDictionary<TimeSpan, double>> CreateEmptyLog()
    {
        var emptyLog = new Dictionary<ProductivityEntry.Label, SortedDictionary<TimeSpan, double>>();
        foreach (ProductivityEntry.Label label in Enum.GetValues(typeof(ProductivityEntry.Label)))
        {
            emptyLog[label] = new SortedDictionary<TimeSpan, double>();
        }

        return emptyLog;
    }

    // EFFECTS: calls add method to add all entries to the average in averageLog
    private void InitAverageLog(List<ProductivityEntry> entries)
    {
        foreach (var entry in entries)
        {
            Add(entry);
        }
    }

    // MODIFIES: this
    // EFFECTS: adds the level of entry to the average for the time of the entry
    public double Add(ProductivityEntry entry)
    {
        TimeSpan time = entry.GetTime();
        int level = entry.GetLevel();
        ProductivityEntry.Label label = entry.GetLabel();

        double oldAverage;
        averageLog[label].TryGetValue(time, out oldAverage);
        double newAverage;

        int newCount = ++counts[label][time.Hours];
        if (averageLog[label].ContainsKey(time))
        {
            newAverage = oldAverage + ((level - oldAverage) / newCount);
        }
        else
        {
            newAverage = level;
        }

        averageLog[label][time] = newAverage;
        return newAverage;
    }

    // REQUIRES: averageLog[label][entry.GetTime()] != null
    // MODIFIES: this
    // EFFECTS: updates the log for the removal of this entry, by calculating the new average after removing this entry
    public double? Remove(ProductivityEntry entry)
    {
        TimeSpan time = entry.GetTime();
        int level = entry.GetLevel();
        ProductivityEntry.Label label = entry.GetLabel();

        double oldAverage = averageLog[label][time];
        int newCount = --counts[label][time.Hours];

        double newAverage;
        if (newCount == 0)
        {
            averageLog[label].Remove(time);
            return null;
        }

        newAverage = oldAverage + (oldAverage - level) / newCount;
        averageLog[label][time] = newAverage;
        return newAverage;
    }

    public Dictionary<ProductivityEntry.Label, SortedDictionary<TimeSpan, double>> GetLog()
    {
        return averageLog;
    }
}