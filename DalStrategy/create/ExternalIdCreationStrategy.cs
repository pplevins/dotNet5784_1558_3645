

using DO;
using System.Xml.Linq;

namespace Dal.Strategies.Create;
/// <summary>
/// ExternalIdCreationStrategy ensures the uniqueness of external IDs before adding an item.
/// It uses an external existence check to prevent the creation of duplicate items based on external IDs.
/// If a conflict is detected, it raises a DalAlreadyExistsException.
/// Otherwise, the item is added to the list, and its external ID is returned.
/// </summary>
public class ExternalIdCreationStrategy<T>(Func<int, T?> existsCheck) : ICreationStrategy<T>
{

    /// <summary>
    /// Creates a new item with external given ID, and adds it to the specified list.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <param name="item">The item to be added to the list.</param>
    /// <param name="source">The list to which the item should be added.</param>
    public int Create(T item, List<T> source, Action<List<T>, string>? saveFunction = null, string? fileName = null)
    {
        int itemId = StrategyHelper<T>.GetEntityId(item);

        if (existsCheck(itemId) is not null)
            throw new Exceptions.DalAlreadyExistsException($"{typeof(T).Name} with external ID={itemId} already exists");

        source.Add(item);
        saveFunction?.Invoke(source, fileName);
        return itemId;
    }
    /// <summary>
    /// Creates a new item with external given ID, and adds it to the specified XML element.
    /// </summary>
    /// <typeparam name="T">The type of items to be created and added to the XML element.</typeparam>
    /// <param name="item">The item to be added to the XML element.</param>
    /// <param name="source">The XML element to which the item should be added.</param>
    /// <param name="saveFunction">
    /// An optional function to save the modified XML element. If provided, it will be invoked
    /// with the modified XML element and a string identifier for the file location.
    /// </param>
    public int Create(T item, XElement source, Action<XElement, string>? saveFunction = null, string? fileName = null)
    {
        int itemId = StrategyHelper<T>.GetEntityId(item);

        if (existsCheck(itemId) is not null)
            throw new Exceptions.DalAlreadyExistsException($"{typeof(T).Name} with external ID={itemId} already exists");
        XElement dependency = StrategyHelper<T>.ParseXElement(item);

        // Add the updated item directly source
        source.Add(dependency);
        saveFunction?.Invoke(source, fileName);
        return itemId;
    }
}
