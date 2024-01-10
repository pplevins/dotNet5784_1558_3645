using DO;

namespace Dal.strategies.delete;
/// <summary>
/// The SoftDeletionStrategy implements a deletion strategy by marking items as inactive, enabling a soft deletion approach.
/// Instead of removing items from the list, it marks them as inactive, allowing for the preservation of historical data while indicating non-active status.
/// This strategy promotes a more nuanced and reversible approach to deletion in scenarios where keeping a record of inactive items is beneficial.
/// </summary>
public class SoftDeletionStrategy<T>(Func<int, T?> getEntity, Action<T>? collectionUpdater = null) : IDeletionStrategy<T>
{
    public void Delete(List<T> items, int id)
    {
        var existingEntity = getEntity(id) ?? throw new Exception($"{typeof(T).Name} with ID={id} does not exist");

        T updatedItem = StrategiesHelper<T>.UpdateEntity(existingEntity, "IsActive", false) ?? throw new Exceptions.DalDeletionImpossibleException($"Deletion of {typeof(T).Name} with ID={id} failed because failure to update the necessary fields");

        collectionUpdater?.Invoke(updatedItem);
    }
}
