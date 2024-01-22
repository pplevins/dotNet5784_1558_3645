using DO;
using System.Xml.Linq;

namespace Dal.Strategies.Delete;
/// <summary>
/// The SoftDeletionStrategy implements a deletion strategy by marking items as inactive, enabling a soft deletion approach.
/// Instead of removing items from the list, it marks them as inactive, allowing for the preservation of historical data while indicating non-active status.
/// This strategy promotes a more nuanced and reversible approach to deletion in scenarios where keeping a record of inactive items is beneficial.
/// </summary>
public class SoftDeletionStrategy<T>(Func<int, T?> getEntity, Action<T> collectionUpdater) : IDeletionStrategy<T>
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
    /// <exception cref="Exceptions.DalDoesNotExistException">In case the entity with the given ID doesn't exist.</exception>
    /// <exception cref="Exceptions.DalDeletionImpossibleException">When the deletion failed due to failure in updating the flag.</exception>
    public void Delete(int id, List<T>? source = null, Action<List<T>, string>? saveFunction = null, string? fileName = null)
    {
        var existingEntity = getEntity(id) ?? throw new Exceptions.DalDoesNotExistException($"{typeof(T).Name} with ID={id} does not exist");

        T updatedItem = StrategyHelper<T>.UpdateEntity(existingEntity, "IsActive", false) ?? throw new Exceptions.DalDeletionImpossibleException($"Deletion of {typeof(T).Name} with ID={id} failed because failure to update the necessary fields");

        collectionUpdater?.Invoke(updatedItem);
    }
    /// <summary>
    /// Deletes an element identified by the specified ID from XML
    /// </summary>
    /// <param name="id">The ID of the element to be deleted.</param>
    /// <param name="source">The XML element source from which the element should be deleted.</param>
    /// <param name="saveFunction">
    /// An optional function to save the modified XML element. If provided, it will be invoked
    /// with the modified XML element. and a string identifier for the location of the file.
    /// </param>
    /// <param name="fileName">An optional string of the file identifier</param>
    /// <exception cref="Exceptions.DalDoesNotExistException">In case the entity with the given ID doesn't exist.</exception>
    /// <exception cref="Exceptions.DalDeletionImpossibleException">When the deletion failed due to failure in updating the flag.</exception>
    public void Delete(int id, XElement? source = null, Action<XElement, string>? saveFunction = null, string? fileName = null)
    {
        var existingEntity = getEntity(id) ?? throw new Exceptions.DalDoesNotExistException($"{typeof(T).Name} with ID={id} does not exist");

        T updatedItem = StrategyHelper<T>.UpdateEntity(existingEntity, "IsActive", false) ?? throw new Exceptions.DalDeletionImpossibleException($"Deletion of {typeof(T).Name} with ID={id} failed because failure to update the necessary fields");

        collectionUpdater?.Invoke(updatedItem);
    }
}
