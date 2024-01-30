using System.Collections;
using System.Reflection;

namespace DO;
/// <summary>
/// Provides tools for object manipulation and representation.
/// </summary>
public static class Tools
{
    /// <summary>
    /// Converts the properties of an object to a formatted string using Reflection.
    /// For properties of collection type (e.g., List<T>), it attempts to display the elements inside the collection.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">The object whose properties are to be converted to a string.</param>
    /// <returns>A formatted string representing the object's properties.</returns>
    public static string ToStringProperty<T>(this T obj)
    {
        Type objectType = typeof(T);
        PropertyInfo[] properties = objectType.GetProperties();

        string result = $"{objectType.Name} Properties:\n";

        foreach (PropertyInfo property in properties)
        {
            object propertyValue = property.GetValue(obj);

            if (IsCollectionType(property.PropertyType))
            {
                result += $"{property.Name}: {FormatCollection(propertyValue)}\n";
            }
            else
            {
                result += $"{property.Name}: {propertyValue}\n";
            }
        }

        return result;
    }

    /// <summary>
    /// Checks if the specified type is a collection type (excluding strings).
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is a collection type; otherwise, false.</returns>
    private static bool IsCollectionType(Type type)
    {
        return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
    }

    /// <summary>
    /// Converts a collection to a formatted string.
    /// For collections with known element types, it displays the elements inside the collection.
    /// </summary>
    /// <param name="collection">The collection object.</param>
    /// <returns>A formatted string representing the collection.</returns>
    private static string FormatCollection(object collection)
    {
        IEnumerable items = (IEnumerable)collection;

        var formattedItems = items?.Cast<object>().Select(item => item.ToString()) ?? Enumerable.Empty<string>();

        return $"[ {string.Join(", ", formattedItems)} ]";
    }
}
