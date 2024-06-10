namespace ProductiveHoursTracker.model;

public interface Observer
{
    void Update(ProductivityEntry curr, ProductivityEntry prev);
}