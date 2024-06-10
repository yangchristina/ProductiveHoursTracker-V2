using System.Text.Json.Nodes;
using ProductiveHoursTracker.persistence;

namespace ProductiveHoursTracker.model;

public class User : Writable
{
    private string _name;
    private Guid _id;
    private ProductivityLog _productivityLog;
    
    public User(string name)
    {
        _name = name;
        _id = Guid.NewGuid();
        _productivityLog = new ProductivityLog(this);
    }
    
    public User(string name, Guid id, List<ProductivityEntry> entries)
    {
        _name = name;
        _id = id;
        _productivityLog = new ProductivityLog(this, entries);
    }

    public string Name => _name;
    public Guid Id => _id;
    public ProductivityLog ProductivityLog => _productivityLog;
    
    public JsonObject ToJson()
    {
        JsonObject json = _productivityLog.ToJson();
        json.Add("name", _name);
        json.Add("id", _id);
        return json;
    }
}