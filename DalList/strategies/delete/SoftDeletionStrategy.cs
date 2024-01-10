namespace Dal.strategies.delete;
/// <summary>
/// Deletion strategy that marks an item as inactive for soft deletion.
/// </summary>
public class SoftDeletionStrategy<T>(Func<int, T?> getEntity, Action<T>? collectionUpdater = null) : IDeletionStrategy<T>
{
    public void Delete(List<T> items, int id)
    {
        var existingEntity = getEntity(id) ?? throw new Exception($"Item with ID={id} does not exist");

        T updatedItem = UpdateEntity(existingEntity, "IsActive", false) ?? throw new Exception($"Deletion of item with ID={id} failed because failure to update the necessary fields");

        collectionUpdater?.Invoke(updatedItem);
    }

    private T UpdateEntity(T existingEntity, string propertyName, object valueToUpdate)
    {
        var propertyInfo = typeof(T).GetProperty(propertyName)
                           ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).FullName}'", nameof(propertyName));

        return Activator.CreateInstance<T>().GetType().GetProperties()
            .Aggregate(existingEntity, (acc, prop) =>
            {
                prop.SetValue(acc, prop == propertyInfo ? valueToUpdate : prop.GetValue(existingEntity));
                return acc;
            });
    }

}
