namespace ProductiveHoursTracker.model;

public abstract class Subject
{
    private List<Observer> observers;
    
    public Subject()
    {
        observers = new List<Observer>();
    }
    
    // MODIFIES: this
    // EFFECTS: if o is not already in observers, add o to observers
    public void AddObserver(Observer o)
    {
        if (!observers.Contains(o))
        {
            observers.Add(o);
        }
    }
    
    // MODIFIES: this
    // EFFECTS: if o is in observers, remove o from observers
    public void RemoveObserver(Observer o)
    {
        observers.Remove(o);
    }

    // EFFECTS: calls update method on each observer in observers
    public void NotifyObservers(ProductivityEntry curr, ProductivityEntry prev)
    {
        foreach (Observer o in observers)
        {
            o.Update(curr, prev);
        }
    }
}