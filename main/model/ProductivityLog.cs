using System.Text.Json.Nodes;
using ProductiveHoursTracker.persistence;

namespace ProductiveHoursTracker.model;

public class ProductivityLog : Observer
{
    private readonly DailyAverageLog _dailyAverageLog;
    public User User { get; }
    public List<ProductivityEntry> Entries { get; }

    // EFFECTS: constructs a ProductivityLog with an empty list of entries and an empty dailyAverageLog
    public ProductivityLog(User user)
    {
        Entries = new List<ProductivityEntry>();
        _dailyAverageLog = new DailyAverageLog();
        User = user;
    }

    // EFFECTS: constructs a ProductivityLog with a given list of entries and a dailyAverageLog with these entries
    public ProductivityLog(User user, List<ProductivityEntry> entries)
    {
        Entries = entries;
        _dailyAverageLog = new DailyAverageLog(entries);
        User = user;
    }
    
    // MODIFIES: this
    // EFFECTS: add given entry to the array it belongs in, and adds it to DailyAverageLog
    public double Add(ProductivityEntry entry) {
        Entries.Add(entry);
        EventLog.Instance.LogEvent(new Event(User.Name + " added entry: " + entry.ToString()));
        entry.AddObserver(this);
        return _dailyAverageLog.Add(entry);
    }

    // MODIFIES: this
    // EFFECTS: removes entry from list, and removes it to DailyAverageLog
    public double? Remove(ProductivityEntry entry) {
        Entries.Remove(entry);
        EventLog.Instance.LogEvent(new Event(User.Name + " removed entry: " + entry.ToString()));
        entry.RemoveObserver(this);
        return _dailyAverageLog.Remove(entry);
    }
    
    public DailyAverageLog DailyAverageLog => _dailyAverageLog;

    public void Update(ProductivityEntry curr, ProductivityEntry prev)
    {
        _dailyAverageLog.Remove(prev);
        _dailyAverageLog.Add(curr);
        EventLog.Instance.LogEvent(new Event(User.Name + " edited entry: \n" + prev + " –––>\n" + curr));
    }
}