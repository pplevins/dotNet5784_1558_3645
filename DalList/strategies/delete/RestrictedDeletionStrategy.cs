using DO;

namespace Dal.strategies.delete;
/// <summary>
/// The RestrictedDeletionStrategy enforces a deletion restriction by throwing an exception, indicating that deletion is not allowed for the specified item.
/// This strategy is useful in scenarios where certain items should be protected from deletion, providing a mechanism to control and prevent undesired removal actions.
/// </summary>
public class RestrictedDeletionStrategy<T> : IDeletionStrategy<T>
{
    public void Delete(List<T> items, int id)
    {
        throw new Exceptions.DalDeletionImpossibleException($"Deletion of {typeof(T).Name} with ID={id} is not allowed");
    }
}
