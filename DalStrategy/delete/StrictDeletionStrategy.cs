
using System.Xml.Linq;
using static DO.Exceptions;

namespace Dal.Strategies.Delete;
/// <summary>
/// The StrictDeletionStrategy implements a deletion strategy that strictly removes an item based on its ID.
/// It ensures that the item with the specified ID exists in the list, throwing an DalDoesNotExistException if not found.
/// This strategy adheres to a more rigorous and immediate deletion approach, suitable for cases requiring precise removal of items.
/// </summary>
public class StrictDeletionStrategy<T>(Func<int, T?> getEntity) : IDeletionStrategy<T>
{
    /// <summary>
    /// Deletes an element identified by the specified ID from the given list.
    /// </summary>
    /// <param name="id">The ID of the element to be deleted.</param>
    /// <param name="source">The list from which the element should be deleted.</param>
    /// <param name="saveFunction">
    /// An optional function to save the modified list. If provided, it will be invoked
    /// with the modified list and a string identifier.
    /// </param>
    /// <param name="fileName">An optional string of the file identifier</param>
    /// <exception cref="DalDoesNotExistException">In case the entity with the given ID doesn't exist.</exception>
    public void Delete(int id, List<T>? source = null, Action<List<T>, string>? saveFunction = null, string? fileName = null)
    {
        _ = getEntity(id) ?? throw new DalDoesNotExistException($"{typeof(T).Name} with ID={id} does not exist");
        if (source is null) return;
        source?.RemoveAll(t => StrategyHelper<T>.GetEntityId(t) == id);

        saveFunction?.Invoke(source, fileName);
    }

    /// <summary>
    /// Deletes an element identified by the specified ID from XML.
    /// </summary>
    /// <param name="id">The ID of the element to be deleted.</param>
    /// <param name="source">The XML element source from which the element should be deleted.</param>
    /// <param name="saveFunction">
    /// An optional function to save the modified XML element. If provided, it will be invoked
    /// with the modified XML element. and a string identifier for the location of the file.
    /// </param>
    /// <param name="fileName">An optional string of the file identifier</param>
    /// <exception cref="DalDoesNotExistException">In case the entity with the given ID doesn't exist.</exception>
    public void Delete(int id, XElement? source, Action<XElement, string>? saveFunction = null, string? fileName = null)
    {
        _ = getEntity(id) ?? throw new DalDoesNotExistException($"{typeof(T).Name} with ID={id} does not exist");
        var dependencyElement = GetXElement(source, id);
        dependencyElement?.Remove();
        saveFunction?.Invoke(source, fileName);
    }

    /// <summary>
    /// Retrieves the XML element with the specified ID from the given XML element.
    /// </summary>
    /// <param name="source">The XML element to search for the specified ID.</param>
    /// <param name="id">The ID to search for in the XML element.</param>
    private XElement? GetXElement(XElement source, int id)
    {
        return source.Elements()
            .FirstOrDefault(p => int.TryParse(p.Element("Id")?.Value, out int elementId) && elementId == id);
    }


}
