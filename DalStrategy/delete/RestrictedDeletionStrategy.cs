using DO;
using System.Xml.Linq;

namespace Dal.Strategies.Delete;
/// <summary>
/// The RestrictedDeletionStrategy enforces a deletion restriction by throwing an exception, indicating that deletion is not allowed for the specified item.
/// This strategy is useful in scenarios where certain items should be protected from deletion, providing a mechanism to control and prevent undesired removal actions.
/// </summary>
public class RestrictedDeletionStrategy<T> : IDeletionStrategy<T>
{
    public void Delete(int id, List<T>? source = null, Action<List<T>, string>? saveFunction = null, string? fileName = null)
    {
        throw new Exceptions.DalDeletionImpossibleException($"Deletion of {typeof(T).Name} with ID={id} is not allowed");
    }

    public void Delete(int id, XElement? source, Action<XElement, string>? saveFunction = null, string? fileName = null)
    {
        throw new Exceptions.DalDeletionImpossibleException($"Deletion of {typeof(T).Name} with ID={id} is not allowed");
    }
}
