
using Dal.strategies;
using Dal.strategies.create;

namespace Dal.Strategies.Create
{
    /// <summary>
    /// InternalIdCreationStrategy is a creation strategy that generates internal IDs based on a provided function (idGenerator).
    /// It updates the entity with the generated ID and adds it to the list.
    /// </summary>
    public class InternalIdCreationStrategy<T>(Func<int> idGenerator)
        : ICreationStrategy<T>
    {
        public int Create(List<T> items, T item)
        {
            int id = idGenerator(); // Generate the ID using the provided function
            Console.WriteLine($"running id is in: {typeof(T).Name} are now: {id}");
            T updatedItem = StrategiesHelper<T>.UpdateEntity(item, "Id", id);

            // Add the updated item directly to the list
            items.Add(updatedItem);

            return id;
        }
    }
}
