namespace Dal.strategies.delete;
/// <summary>
/// Deletion strategy that marks an item as inactive for soft deletion.
/// </summary>
public class SoftDeletionStrategy<T>(Func<T, T> getUpdatedItem, Action<List<T>, T>? updateMethod = null) : IDeletionStrategy<T>
{
    private readonly Func<T, T> _getUpdatedItem = getUpdatedItem ?? throw new ArgumentNullException(nameof(getUpdatedItem));

    public void Delete(List<T> items, int id, Func<int, T?>? getItem)
    {
        _ = getItem ?? throw new ArgumentNullException(nameof(getItem), "getItem cannot be null");

        var existingItem = getItem(id) ?? throw new Exception($"Item with ID={id} does not exist");

        T updatedItem = _getUpdatedItem(existingItem) ?? throw new Exception($"Deletion of item with ID={id} failed because failure to update the necessary fields");

        updateMethod?.Invoke(items, updatedItem);
    }

}
