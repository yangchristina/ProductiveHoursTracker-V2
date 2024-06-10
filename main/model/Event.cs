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
    public DateTime GetDate() {
        return dateLogged;
    }

    /**
     * Gets the description of this event.
     * @return  the description of the event
     */
    public String GetDescription() {
        return description;
    }

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

        return (this.dateLogged.Equals(otherEvent.dateLogged)
                && this.description.Equals(otherEvent.description));
    }
    
    public override int GetHashCode()
    {
        return (HASH_CONSTANT * dateLogged.GetHashCode() + description.GetHashCode());
    }

    public override string ToString() {
        return dateLogged.ToString(CultureInfo.InvariantCulture) + "\n" + description;
    }
}