using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

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


    // Validate positive number
    private static bool ValidatePositiveNumber(object value)
    {
        if (value is not null && value is IComparable comparable && comparable.CompareTo(0) <= 0)
        {
            throw new ArgumentException("Invalid value. Must be a positive number.");
        }

        return true;
    }

    // Validate non-empty string
    private static bool ValidateNonEmptyString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Invalid value. Must be a non-empty string.");
        }
        return true;
    }

    // Validate email address
    private static bool ValidateEmailAddress(string value)
    {
        if (ValidateNonEmptyString(value) && !IsValidEmailAddress(value))
        {
            throw new ArgumentException("Invalid email address.");
        }

        return true;
    }
    // Validate email address using a simple regex
    private static bool IsValidEmailAddress(string emailAddress)
    {
        // This is a simple regex for demonstration purposes, you may need a more comprehensive one in a real-world scenario
        const string emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(emailAddress, emailRegex);
    }

    /// <summary>
    /// Copies similar fields from the source object to the destination object, excluding specified properties.
    /// </summary>
    public static void CopySimilarFields<TSource, TDestination>(TSource source, TDestination destination, params Expression<Func<TDestination, object>>[] excludedProperties)
    {
        ValidateArguments(source, destination);

        foreach (var sourceProperty in typeof(TSource).GetProperties())
        {
            if (IsExcluded(sourceProperty.Name, excludedProperties)) continue;

            var matchingDestinationProperty = FindMatchingDestinationProperty(sourceProperty, typeof(TDestination));

            CopyPropertyValue(sourceProperty, matchingDestinationProperty, source, destination);
        }
    }
    /// <summary>
    /// Validates the arguments for null and throws an <see cref="ArgumentNullException"/> if either source or destination is null.
    /// </summary>
    private static void ValidateArguments<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (source is null || destination is null)
        {
            throw new ArgumentNullException(nameof(source));
        }
    }
    /// <summary>
    /// Checks if a property is excluded based on the specified property name and excluded properties.
    /// </summary>
    private static bool IsExcluded<TSource>(string propertyName, Expression<Func<TSource, object>>[] excludedProperties)
    {
        return excludedProperties.Any(excludedProperty => GetPropertyName(excludedProperty) == propertyName);
    }
    /// <summary>
    /// Finds a matching destination property for a given source property and destination type.
    /// </summary>
    private static PropertyInfo? FindMatchingDestinationProperty(PropertyInfo sourceProperty, Type destinationType)
    {
        return destinationType.GetProperties()
            .FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType.IsAssignableFrom(sourceProperty.PropertyType) && p.CanWrite);
    }
    /// <summary>
    /// Copies the value of a property from the source object to the destination object.
    /// </summary>
    private static void CopyPropertyValue(PropertyInfo sourceProperty, PropertyInfo? destinationProperty, object source, object destination)
    {
        if (destinationProperty != null)
        {
            var valueToCopy = sourceProperty.GetValue(source);
            destinationProperty.SetValue(destination, valueToCopy);
        }
    }
    /// <summary>
    /// Gets the name of the property from the specified expression.
    /// </summary>
    private static string? GetPropertyName<TSource>(Expression<Func<TSource, object>> expression)
    {
        return (expression.Body as MemberExpression)?.Member.Name ?? (expression.Body as UnaryExpression)?.Operand.ToString();
    }
}
