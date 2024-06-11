using System.Collections;

namespace ProductiveHoursTracker.model;

/**
 * Represents a log of alarm system events.
 * We use the Singleton Design Pattern to ensure that there is only
 * one EventLog in the system and that the system has global access
 * to the single instance of the EventLog.
 */
public sealed class EventLog : IEnumerable<Event>
{
    /** the only EventLog in the system (Singleton Design Pattern) */
    private static readonly Lazy<EventLog> theLog = new Lazy<EventLog>(() => new EventLog());

    private readonly List<Event> events;

    private EventLog()
    {
        events = new List<Event>();
    }

    /**
     * Gets instance of EventLog - creates it
     * if it doesn't already exist.
     * (Singleton Design Pattern)
     * @return  instance of EventLog
     */
    public static EventLog Instance
    {
        get { return theLog.Value; }
    }

    /**
     * Adds an event to the event log.
     * @param e the event to be added
     */
    public void LogEvent(Event e)
    {
        events.Add(e);
    }
    
    /**
     * Clears the event log and logs the event.
     */
    public void Clear() {
        events.Clear();
        LogEvent(new Event("Event log cleared."));
    }
    
    public IEnumerator<Event> GetEnumerator()
    {
        return events.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}