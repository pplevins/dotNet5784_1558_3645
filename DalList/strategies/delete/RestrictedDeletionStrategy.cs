namespace Dal.strategies.delete;
/// <summary>
/// Deletion strategy that restricts deletion for the specified item.
/// </summary>
public class RestrictedDeletionStrategy<T> : IDeletionStrategy<T>
{
    public void Delete(List<T> items, int id, Func<int, T?>? getItem)
    {
        throw new Exception($"Deletion of item with ID={id} is not allowed");
    }
}
