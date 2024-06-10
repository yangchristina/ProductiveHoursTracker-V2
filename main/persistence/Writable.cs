using System.Text.Json.Nodes;

namespace ProductiveHoursTracker.persistence;

public interface Writable
{
    // EFFECTS: returns this as a JSON object
    JsonObject ToJson();
}