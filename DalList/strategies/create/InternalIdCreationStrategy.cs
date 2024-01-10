
using Dal.strategies.create;

namespace Dal.Strategies.Create
{
    /// <summary>
    /// Creation strategy that takes an entity and based on internal Id generation strategy create the entity.
    /// </summary>
    /// <typeparam name="T">The type of the items to create.</typeparam>
    public class InternalIdCreationStrategy<T>(Func<int> idGenerator)
        : ICreationStrategy<T>
    {
        public int Create(List<T> items, T item)
        {
            int id = idGenerator(); // Generate the ID using the provided function
            T updatedItem = UpdateEntity(item, "Id", id);

            // Add the updated item directly to the list
            items.Add(updatedItem);

            return id;
        }
        private T UpdateEntity(T existingEntity, string propertyName, object valueToUpdate)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName)
                               ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).FullName}'", nameof(propertyName));

            return Activator.CreateInstance<T>().GetType().GetProperties()
                .Aggregate(existingEntity, (acc, prop) =>
                {
                    prop.SetValue(acc, prop == propertyInfo ? valueToUpdate : prop.GetValue(existingEntity));
                    return acc;
                });
        }
    }
}
