
namespace Dal.strategies.delete;
/// <summary>
/// Deletion strategy that strictly deletes an item based on its ID.
/// </summary>
public class StrictDeletionStrategy<T>(Func<int, T?> getEntity) : IDeletionStrategy<T>
{
    public void Delete(List<T> items, int id)
    {
        var existingItem = getEntity(id) ?? throw new Exception($"Item with ID={id} does not exist");
        items.Remove(existingItem);
    }
}
