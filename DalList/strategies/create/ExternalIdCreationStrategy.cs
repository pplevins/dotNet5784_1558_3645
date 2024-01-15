

using DO;

namespace Dal.Strategies.Create;
/// <summary>
/// ExternalIdCreationStrategy ensures the uniqueness of external IDs before adding an item.
/// It uses an external existence check to prevent the creation of duplicate items based on external IDs.
/// If a conflict is detected, it raises a DalAlreadyExistsException.
/// Otherwise, the item is added to the list, and its external ID is returned.
/// </summary>
public class ExternalIdCreationStrategy<T>(Func<int, T?> existsCheck) : ICreationStrategy<T>
{

    public int Create(List<T> items, T item)
    {
        int itemId = StrategiesHelper<T>.GetEntityId(item);

        if (existsCheck(itemId) is not null)
            throw new Exceptions.DalAlreadyExistsException($"{typeof(T).Name} with external ID={itemId} already exists");

        items.Add(item);
        return itemId;
    }
}
