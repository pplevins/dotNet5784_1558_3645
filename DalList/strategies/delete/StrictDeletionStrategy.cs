
namespace Dal.strategies.delete;
/// <summary>
/// Deletion strategy that strictly deletes an item based on its ID.
/// </summary>
public class StrictDeletionStrategy<T> : IDeletionStrategy<T>
{
    public void Delete(List<T> items, int id, Func<int, T?>? getItem)
    {
        _ = getItem ?? throw new ArgumentNullException(nameof(getItem), "getItem cannot be null");
        var existingItem = getItem(id) ?? throw new Exception($"Item with ID={id} does not exist");
        items.Remove(existingItem);
    }
}
