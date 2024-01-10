
using static DO.Exceptions;

namespace Dal.strategies.delete;
/// <summary>
/// The StrictDeletionStrategy implements a deletion strategy that strictly removes an item based on its ID.
/// It ensures that the item with the specified ID exists in the list, throwing an DalDoesNotExistException if not found.
/// This strategy adheres to a more rigorous and immediate deletion approach, suitable for cases requiring precise removal of items.
/// </summary>
public class StrictDeletionStrategy<T>(Func<int, T?> getEntity, Action<T>? collectionUpdater = null) : IDeletionStrategy<T>
{
    public void Delete(List<T> items, int id)
    {
        _ = getEntity(id) ?? throw new DalDoesNotExistException($"{typeof(T).Name} with ID={id} does not exist");
        items.RemoveAll(t => StrategiesHelper<T>.GetEntityId(t) == id);
    }
}
