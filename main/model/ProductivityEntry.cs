using System.Text.Json.Nodes;
using ProductiveHoursTracker.persistence;

namespace ProductiveHoursTracker.model;

public class ProductivityEntry : Subject, Writable
{
    public enum Label
    {
        ENERGY,
        FOCUS,
        MOTIVATION
    }
    
    private readonly DateTime date;
    private TimeSpan time;
    protected int level;
    private Label label;
    
    // EFFECTS: creates a new productivity entry with given values for label, date, time and level
    public ProductivityEntry(Label label, DateTime date, TimeSpan time, int level)
    {
        this.label = label;
        this.date = date; // cannot be modified
        this.time = time;
        this.level = level;
    }
    
    // EFFECTS: returns a string with a description of the entry
    public override string ToString()
    {
        return $"{GetLabel()} level of {level} at {time} on {date.ToShortDateString()}.";
    }
    
    // EFFECTS: calls setter methods to edit productivity entry
    public void Edit(Label label, TimeSpan time, int level)
    {
        var old = new ProductivityEntry(this.label, this.date, this.time, this.level);

        this.label = label;
        this.time = time;
        this.level = level;

        base.NotifyObservers(this, old);
    }

    public Label GetLabel()
    {
        return label;
    }

    public DateTime GetDate()
    {
        return date;
    }

    public TimeSpan GetTime()
    {
        return time;
    }

    public int GetLevel()
    {
        return level;
    }

    public JsonObject ToJson()
    {
        var json = new JsonObject
        {
            { "label", GetLabel().ToString() },
            { "date", date.ToString("yyyy-MM-dd") },
            { "time", time.ToString() },
            { "level", level }
        };
        return json;
    }
}