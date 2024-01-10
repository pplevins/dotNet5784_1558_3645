

namespace Dal.strategies.delete;
/// <summary>
/// Interface for deletion strategy.
/// </summary>
/// <typeparam name="T">The type of the items in the list.</typeparam>
public interface IDeletionStrategy<T>
{
    public void Delete(List<T> items, int id);
}
