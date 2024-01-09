

namespace Dal.strategies.create;
/// <summary>
/// Creation strategy that checks if an item with the specified external ID already exists before adding it.
/// </summary>
public class ExternalIdCreationStrategy<T>(Func<int, T?> getItem) : ICreationStrategy<T>
{
    private Func<int, T?> _getItem = getItem ?? throw new ArgumentNullException(nameof(getItem));

    public ExternalIdCreationStrategy<T> SetGetItem(Func<int, T?> getItem)
    {
        _getItem = getItem ?? throw new ArgumentNullException(nameof(getItem));
        return this;
    }

    public int Create(List<T> items, T item)
    {
        int itemId = GetItemId(item);

        if (_getItem(itemId) is not null)
            throw new Exception($"{typeof(T).Name} with external ID={itemId} already exists");

        items.Add(item);
        return itemId;
    }

    private int GetItemId(T item)
    {
        // Assuming items have an "Id" property
        var idProperty = item?.GetType().GetProperty("Id")
                         ?? throw new InvalidOperationException($"Type {typeof(T).Name} does not have an 'Id' property.");

        var idValue = idProperty.GetValue(item, null)
                      ?? throw new InvalidOperationException($"The 'Id' property of {typeof(T).Name} is null.");

        return (int)idValue;
    }
}
