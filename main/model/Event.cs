using System.Globalization;

namespace ProductiveHoursTracker.model;

public class Event
{
    private static readonly int HASH_CONSTANT = 13;
    private DateTime dateLogged;
    private string description;

    /**
     * Creates an event with the given description
     * and the current date/time stamp.
     * @param description  a description of the event
     */
    public Event(string description)
    {
        dateLogged = DateTime.Now;
        this.description = description;
    }
    
    /**
     * Gets the date of this event (includes time).
     * @return  the date of the event
     */
    public DateTime Date => dateLogged;
    
    public String Description => description;

    public override bool Equals(object other)
    {
        if (other == null)
        {
            return false;
        }

        if (other.GetType() != this.GetType())
        {
            return false;
        }

        Event otherEvent = (Event) other;

        return (dateLogged.Equals(otherEvent.dateLogged)
                && description.Equals(otherEvent.description));
    }
    
    public override int GetHashCode()
    {
        return (HASH_CONSTANT * dateLogged.GetHashCode() + description.GetHashCode());
    }

    public override string ToString() {
        return dateLogged.ToString("g") + "\n" + description;
    }
}