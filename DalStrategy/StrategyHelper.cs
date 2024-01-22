

using System.Reflection;
using System.Xml.Linq;

namespace Dal.Strategies;
/// <summary>
/// Utility class providing helper methods for common tasks related to generics
/// </summary>
/// <typeparam name="T">The type of the entities.</typeparam>
public static class StrategyHelper<T>
{
    /// <summary>
    /// Retrieves the ID property value of a given entity.
    /// </summary>
    /// <param name="entity">The entity from which to retrieve the ID.</param>
    /// <returns>The ID value as an integer.</returns>
    public static int GetEntityId(T entity)
    {
        var idProperty = GetProperty(entity, "Id");

        var idValue = GetPropertyValue(entity, idProperty);

        return (int)idValue;
    }
    /// <summary>
    /// Create new copy and Updates a specified property of an existing entity with a new value.
    /// </summary>
    /// <param name="existingEntity">The entity to be updated.</param>
    /// <param name="propertyName">The name of the property to update.</param>
    /// <param name="valueToUpdate">The new value for the specified property.</param>
    /// <returns>The updated entity with the modified property.</returns>

    public static T UpdateEntity(T existingEntity, string propertyName, object valueToUpdate)
    {
        var propertyInfo = GetProperty(existingEntity, propertyName);

        return Activator.CreateInstance<T>().GetType().GetProperties()
            .Aggregate(existingEntity, (acc, prop) =>
            {
                prop.SetValue(acc, prop == propertyInfo ? valueToUpdate : prop.GetValue(existingEntity));
                return acc;
            });
    }
    /// <summary>
    /// Retrieves a PropertyInfo object for a specified property of an entity.
    /// </summary>
    /// <param name="entity">The entity from which to retrieve the property.</param>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <returns>PropertyInfo object for the specified property.</returns>

    public static PropertyInfo GetProperty(T entity, string propertyName)
    {
        return entity?.GetType().GetProperty(propertyName)
               ?? throw new InvalidOperationException($"Type {typeof(T).Name} does not have an {propertyName} property.");

    }
    /// <summary>
    /// Retrieves the value of a specified property of an entity.
    /// </summary>
    /// <param name="entity">The entity from which to retrieve the property value.</param>
    /// <param name="propertyInfo">PropertyInfo object representing the property.</param>
    /// <returns>The value of the specified property.</returns>
    public static object GetPropertyValue(T entity, PropertyInfo propertyInfo)
    {
        return propertyInfo.GetValue(entity, null)
                          ?? throw new InvalidOperationException($"The {propertyInfo.Name} property of {typeof(T).Name} is null.");

    }

    /// <summary>
    /// An helper function to parse entity into XML element. 
    /// </summary>
    /// <param name="item">the entity item to be parse.</param>
    /// <returns>the parsed XElement</returns>
    public static XElement ParseXElement(T item)
    {
        Type itemType = typeof(T);

        var elements = itemType.GetProperties()
            .Select(property =>
            {
                object value = StrategyHelper<T>.GetPropertyValue(item, property);
                return new XElement(property.Name, value.ToString());
            });

        return new XElement(itemType.Name, elements);
    }
}
