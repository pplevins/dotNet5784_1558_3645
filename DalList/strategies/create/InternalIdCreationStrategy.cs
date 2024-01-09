
using Dal.strategies.create;

namespace Dal.Strategies.Create
{
    /// <summary>
    /// Creation strategy that takes an entity and based on internal Id generation strategy create the entity.
    /// </summary>
    /// <typeparam name="T">The type of the items to create.</typeparam>
    public class InternalIdCreationStrategy<T>(Func<int, T, T> getUpdatedItem, Func<int> idGenerator)
        : ICreationStrategy<T>
    {
        private Func<int, T, T> _getUpdatedItem = getUpdatedItem ?? throw new ArgumentNullException(nameof(getUpdatedItem));
        private Func<int> _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));


        public InternalIdCreationStrategy<T> SetGetUpdatedItem(Func<int, T, T> getUpdatedItem)
        {
            _getUpdatedItem = getUpdatedItem ?? throw new ArgumentNullException(nameof(getUpdatedItem));
            return this;
        }

        public InternalIdCreationStrategy<T> SetIdGenerator(Func<int> idGenerator)
        {
            _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
            return this;
        }
        public int Create(List<T> items, T item)
        {
            int id = _idGenerator(); // Generate the ID using the provided function
            T updatedItem = _getUpdatedItem(id, item);

            // Add the updated item directly to the list
            items.Add(updatedItem);

            return id;
        }
    }
}
