using System.Xml.Linq;

namespace Dal.Strategies.Create;



/// <summary>
/// InternalIdCreationStrategy is a creation strategy that generates internal IDs based on a provided function (idGenerator).
/// It updates the entity with the generated ID and adds it to the list.
/// </summary>
public class InternalIdCreationStrategy<T>(Func<int> idGenerator)
    : ICreationStrategy<T>
{
    /// <summary>
    /// Creates a new item, assigns a generated ID to it, and adds it to the specified list.
    /// </summary>
    /// <param name="item">The item to be added to the list.</param>
    /// <param name="source">The list to which the item should be added.</param>
    /// <param name="saveFunction">
    /// An optional function to save the modified XML element. If provided, it will be invoked
    /// with the modified XML element list, and a string identifier for the location of the file.
    /// </param>
    /// <param name="fileName">An optional string of the file identifier</param>
    /// <returns>the new entity's ID</returns>
    public int Create(T item, List<T> source, Action<List<T>, string>? saveFunction = null, string? fileName = null)
    {
        int id = idGenerator(); // Generate the ID using the provided function
        //Console.WriteLine($"running id is in: {typeof(T).Name} are now: {id}");
        T updatedItem = StrategyHelper<T>.UpdateEntity(item, "Id", id);

        // Add the updated item directly to the list
        source.Add(updatedItem);
        saveFunction?.Invoke(source, fileName);
        return id;
    }

    /// <summary>
    /// Creates a new item, assigns a generated ID to it, and adds it to the specified XML element.
    /// </summary>
    /// <param name="item">The item to be added to the XML element.</param>
    /// <param name="source">The XML element to which the item should be added.</param>
    /// <param name="saveFunction">
    /// An optional function to save the modified XML element. If provided, it will be invoked
    /// with the modified XML element and a string identifier for the file location.
    /// </param>
    /// <param name="fileName">An optional string of the file identifier</param>
    /// <returns>the new entity's ID</returns>
    public int Create(T item, XElement source, Action<XElement, string>? saveFunction = null, string? fileName = null)
    {
        int id = idGenerator(); // Generate the ID using the provided function
        //Console.WriteLine($"running id is in: {typeof(T).Name} are now: {id}");
        T updatedItem = StrategyHelper<T>.UpdateEntity(item, "Id", id);
        XElement dependency = StrategyHelper<T>.ParseXElement(updatedItem);

        // Add the updated item directly source
        source.Add(dependency);
        saveFunction?.Invoke(source, fileName);
        return id;
    }

}
