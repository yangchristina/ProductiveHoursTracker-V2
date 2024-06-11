using System.Text.Json.Nodes;
using ProductiveHoursTracker.persistence;

namespace ProductiveHoursTracker.model;

public class ProductivityEntry : Subject, Writable
{
    public enum Label
    {
        Energy,
        Focus,
        Motivation
    }
    
    private readonly DateTime date;
    private TimeSpan _time;
    public int Level { get; private set; }
    private Label _label;
    
    // EFFECTS: creates a new productivity entry with given values for label, date, time and level
    public ProductivityEntry(Label label, DateTime date, TimeSpan time, int level)
    {
        _label = label;
        this.date = date; // cannot be modified
        _time = time;
        Level = level;
    }
    
    // EFFECTS: returns a string with a description of the entry
    public override string ToString()
    {
        return $"{GetLabel()} level of {Level} at {_time} on {date.ToShortDateString()}.";
    }
    
    // EFFECTS: calls setter methods to edit productivity entry
    public void Edit(Label label, TimeSpan time, int level)
    {
        var old = new ProductivityEntry(this._label, this.date, this._time, this.Level);

        _label = label;
        _time = time;
        Level = level;

        NotifyObservers(this, old);
    }
    
    public void EditLevel(int level)
    {
        var old = new ProductivityEntry(this._label, this.date, this._time, this.Level);
        Level = level;

        NotifyObservers(this, old);
    }
    
    public void EditTime(TimeSpan time)
    {
        var old = new ProductivityEntry(this._label, this.date, this._time, this.Level);
        _time = time;

        NotifyObservers(this, old);
    }

    public Label GetLabel()
    {
        return _label;
    }
    
    public DateTime Date => date;
    
    public TimeSpan Time => _time;

    public JsonObject ToJson()
    {
        var json = new JsonObject
        {
            { "label", GetLabel().ToString() },
            { "date", date.ToString("yyyy-MM-dd") },
            { "time", _time.ToString() },
            { "level", Level }
        };
        return json;
    }
}