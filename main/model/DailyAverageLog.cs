namespace ProductiveHoursTracker.model;

public class DailyAverageLog
{
    private Dictionary<ProductivityEntry.Label, int[]> _counts;
    public Dictionary<ProductivityEntry.Label, SortedDictionary<TimeSpan, double>> Log { get; }

    // EFFECTS: constructs DailyAverageLog with an empty counts and an empty averageLog
    public DailyAverageLog()
    {
        _counts = CreateEmptyCounts();
        Log = CreateEmptyLog();
    }

    // EFFECTS: constructs DailyAverageLog with a given list of productivity entries,
    //          and a log of levels by time of day, which is calculated from the productivity entries
    public DailyAverageLog(List<ProductivityEntry> entries)
    {
        _counts = CreateEmptyCounts();
        Log = CreateEmptyLog();
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
        TimeSpan time = entry.Time;
        int level = entry.Level;
        ProductivityEntry.Label label = entry.GetLabel();

        double oldAverage;
        Log[label].TryGetValue(time, out oldAverage);
        double newAverage;

        int newCount = ++_counts[label][time.Hours];
        if (Log[label].ContainsKey(time))
        {
            newAverage = oldAverage + ((level - oldAverage) / newCount);
        }
        else
        {
            newAverage = level;
        }

        Log[label][time] = newAverage;
        return newAverage;
    }

    // REQUIRES: averageLog[label][entry.GetTime()] != null
    // MODIFIES: this
    // EFFECTS: updates the log for the removal of this entry, by calculating the new average after removing this entry
    public double? Remove(ProductivityEntry entry)
    {
        TimeSpan time = entry.Time;
        int level = entry.Level;
        ProductivityEntry.Label label = entry.GetLabel();

        double oldAverage = Log[label][time];
        int newCount = --_counts[label][time.Hours];

        double newAverage;
        if (newCount == 0)
        {
            Log[label].Remove(time);
            return null;
        }

        newAverage = oldAverage + (oldAverage - level) / newCount;
        Log[label][time] = newAverage;
        return newAverage;
    }
    
// EFFECTS: gets peaks and troughs of a certain entry type
    public Dictionary<string, List<TimeSpan>> GetPeaksAndTroughs(ProductivityEntry.Label entryType)
    {
        Dictionary<string, List<TimeSpan>> peaksAndTroughs = new Dictionary<string, List<TimeSpan>>();

        List<TimeSpan> peakHours = new List<TimeSpan>();
        List<TimeSpan> troughHours = new List<TimeSpan>();

        KeyValuePair<TimeSpan, double>? left = null;
        KeyValuePair<TimeSpan, double>? curr = null;

        foreach (KeyValuePair<TimeSpan, double> right in Log[entryType])
        {
            if (left != null && curr != null)
            {
                if (curr.Value.Value < left.Value.Value && curr.Value.Value < right.Value)
                {
                    troughHours.Add(curr.Value.Key);
                }
                else if (curr.Value.Value > left.Value.Value && curr.Value.Value > right.Value)
                {
                    peakHours.Add(curr.Value.Key);
                }
                // ignoring equals case /--\ and \__/ and edge cases, ex. \____
            }
            left = curr;
            curr = right;
        }

        peaksAndTroughs.Add("peak", peakHours);
        peaksAndTroughs.Add("trough", troughHours);
        return peaksAndTroughs;
    }
}