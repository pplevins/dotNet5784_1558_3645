
namespace Dal;

/// <summary>
/// Provides generic methods for item deletion with different behaviors.
/// </summary>
public static class DeletionHelper
{
    /// <summary>
    /// Deletes an item from the list strictly based on its ID, throwing an exception if the item is not found.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="items">The list of items to delete from.</param>
    /// <param name="getItem">A function to extract an item from the ID its given.</param>
    /// <param name="id">The ID of the item to delete.</param>
    /// <exception cref="Exception">Thrown when the item with the specified ID is not found.</exception>
    public static void StrictDelete<T>(List<T> items, Func<int, T?> getItem, int id)
    {
        var existingItem = getItem(id) ?? throw new Exception($"Item with ID={id} does not exist");
        items.Remove(existingItem);
    }

    /// <summary>
    /// Throws an exception indicating that deletion is not allowed for the specified item.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="id">The ID of the item for which deletion is not allowed.</param>
    /// <exception cref="Exception">Thrown to indicate that deletion is not allowed.</exception>
    public static void RestrictedDelete<T>(int id)
    {
        throw new Exception($"Deletion of item with ID={id} is not allowed");
    }

    /// <summary>
    /// Marks an item as inactive for soft deletion based on its ID, throwing an exception if the item is not found.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="items">The list of items to mark as inactive.</param>
    /// <param name="getItem">A function to extract an item from the ID its given.</param>
    /// <param name="id">The ID of the item to mark as inactive.</param>
    /// <param name="getUpdatedItem">A function that returns the item with the updated fields(like marking the item as inactive).</param>
    /// <param name="updateMethod">An optional method that responsible for updating the list of items with the updated Item</param>
    /// <exception cref="Exception">Thrown when the item with the specified ID is not found, or when getUpdatedItem is null.</exception>
    public static void SoftDelete<T>(List<T> items, Func<int, T?> getItem, int id, Func<T, T> getUpdatedItem, Action<List<T>, T>? updateMethod = null)
    {
        var existingItem = getItem(id) ?? throw new Exception($"Item with ID={id} does not exist");

        T updatedItem = getUpdatedItem(existingItem) ?? throw new Exception($"Deletion of item with ID={id} Failed because failure to update the necessary fields"); ;

        updateMethod?.Invoke(items, updatedItem);
    }

}

