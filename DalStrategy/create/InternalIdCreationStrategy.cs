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
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <param name="item">The item to be added to the list.</param>
    /// <param name="source">The list to which the item should be added.</param>
    public int Create(T item, List<T> source)
    {
        int id = idGenerator(); // Generate the ID using the provided function
        //Console.WriteLine($"running id is in: {typeof(T).Name} are now: {id}");
        T updatedItem = StrategyHelper<T>.UpdateEntity(item, "Id", id);

        // Add the updated item directly to the list
        source.Add(updatedItem);

        return id;
    }

    /// <summary>
    /// Creates a new item, assigns a generated ID to it, and adds it to the specified XML element.
    /// </summary>
    /// <typeparam name="T">The type of items to be created and added to the XML element.</typeparam>
    /// <param name="item">The item to be added to the XML element.</param>
    /// <param name="source">The XML element to which the item should be added.</param>
    /// <param name="saveFunction">
    /// An optional function to save the modified XML element. If provided, it will be invoked
    /// with the modified XML element and a string identifier for the file location.
    /// </param>
    public int Create(T item, XElement source, Action<XElement, string>? saveFunction = null)
    {
        int id = idGenerator(); // Generate the ID using the provided function
        //Console.WriteLine($"running id is in: {typeof(T).Name} are now: {id}");
        T updatedItem = StrategyHelper<T>.UpdateEntity(item, "Id", id);
        XElement dependency = StrategyHelper<T>.ParseXElement(updatedItem);

        // Add the updated item directly source
        source.Add(dependency);
        saveFunction?.Invoke(source, $"{typeof(T).Name}+s");
        return id;
    }

}
