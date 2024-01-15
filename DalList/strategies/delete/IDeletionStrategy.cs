namespace Dal.Strategies.Delete;
/// <summary>
/// The IDeletionStrategy interface defines a strategy pattern for deleting items of type T.
/// It declares a Delete method, accepting a list of items and the ID of the item to be deleted.
/// This interface allows the implementation of diverse deletion strategies, providing flexibility and extensibility in handling different scenarios.
/// The strategy pattern enables dynamic selection of deletion strategies at runtime, fostering modular and adaptable code architecture.
/// </summary>
public interface IDeletionStrategy<T>
{
    public void Delete(List<T> items, int id);
}
