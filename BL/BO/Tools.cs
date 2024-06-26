﻿using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BO;
/// <summary>
/// Provides tools for object manipulation and representation.
/// </summary>
public static class Tools
{

    /// <summary>
    /// Validate positive number
    /// </summary>
    /// <typeparam name="T">int/double</typeparam>
    /// <param name="value">the value to validate</param>
    /// <returns>true/false</returns>
    /// <exception cref="ArgumentException">in case the value is not valid</exception>
    public static bool ValidatePositiveNumber<T>(T? value) where T : struct, IComparable<T>
    {
        if (value is not null && value?.CompareTo(default(T)) <= 0)
        {
            throw new ArgumentException($"Invalid {nameof(value)}. Must be a positive number.");
        }
        return true;
    }

    /// <summary>
    /// Validate non-empty string
    /// </summary>
    /// <param name="value">string to validate</param>
    /// <returns>true/false</returns>
    /// <exception cref="ArgumentException">in case the value is not valid</exception>
    public static bool ValidateNonEmptyString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Invalid {nameof(value)}. Must be a non-empty string.");
        }
        return true;
    }

    /// <summary>
    /// Validate email address
    /// </summary>
    /// <param name="value">the email to validate</param>
    /// <returns>true/false</returns>
    /// <exception cref="ArgumentException">in case the value is not valid</exception>
    public static bool ValidateEmailAddress(string value)
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
            var propertyValue = GetPropertyValue(obj, property);
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

    /// <summary>
    /// Create new copy and Updates a specified property of an existing entity with a new value.
    /// </summary>
    /// <param name="existingEntity">The entity to be updated.</param>
    /// <param name="propertyName">The name of the property to update.</param>
    /// <param name="valueToUpdate">The new value for the specified property.</param>
    /// <returns>The updated entity with the modified property.</returns>
    public static T UpdateEntity<T>(T existingEntity, string propertyName, object valueToUpdate)
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
    /// Copies similar fields from the source object to the destination object, excluding specified properties.
    /// </summary>
    public static void CopySimilarFields<TSource, TDestination>(TSource source, TDestination destination, Type[]? typesToCopy = null, params string[] excludedProperties)
    {
        ValidateArguments(source, destination);

        foreach (var sourceProperty in typeof(TSource).GetProperties())
        {

            var matchingDestinationProperty = FindMatchingDestinationProperty(sourceProperty, typeof(TDestination));
            if (matchingDestinationProperty != null && IsExcluded(sourceProperty.Name, excludedProperties))
            {
                CheckPropertyValueMatch(sourceProperty, matchingDestinationProperty, source!, destination!);
                continue;
            }
            if (matchingDestinationProperty != null && (typesToCopy == null || typesToCopy.Contains(sourceProperty.PropertyType)))
            {
                CopyPropertyValue(sourceProperty, matchingDestinationProperty, source!, destination!);
            }
        }
    }

    private static void CheckPropertyValueMatch(PropertyInfo sourceProperty, PropertyInfo? destinationProperty, object source, object destination)
    {
        if (destinationProperty != null)
        {
            var sourceValue = GetPropertyValue(source, sourceProperty);
            var destinationValue = GetPropertyValue(destination, destinationProperty);
            if (!Equals(sourceValue, destinationValue)) throw new InvalidOperationException($"You can't change the {sourceProperty.Name} property");
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
    private static bool IsExcluded(string propertyName, string[] excludedProperties)
    {
        var e = excludedProperties.Any(excludedProperty => excludedProperty == propertyName);
        return e;
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
            var valueToCopy = GetPropertyValue(source, sourceProperty);
            destinationProperty.SetValue(destination, valueToCopy);
        }
    }
    /// <summary>
    /// Retrieves a PropertyInfo object for a specified property of an entity.
    /// </summary>
    /// <param name="entity">The entity from which to retrieve the property.</param>
    /// <param name="propertyName">The name of the property to retrieve.</param>
    /// <returns>PropertyInfo object for the specified property.</returns>

    public static PropertyInfo GetProperty<T>(T entity, string propertyName)
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
    public static object GetPropertyValue<T>(T entity, PropertyInfo propertyInfo)
    {
        return propertyInfo.GetValue(entity, null);

    }

    /// <summary>
    /// Helper function to check if the read entity is inactive, and prints note to the user about it
    /// </summary>
    /// <typeparam name="T">generic</typeparam>
    /// <param name="entity">engineer/task/dependency (not relevant)</param>
    /// <param name="ent">the entity itself</param>
    public static void CheckActive<T>(string entity, T? ent)
    {
        var isActiveProperty = typeof(T).GetProperty("IsActive");

        if (isActiveProperty is not null && isActiveProperty.PropertyType == typeof(bool))
        {
            // If "isActive" property exists and is of type bool, check its value
            bool isActive = (bool)isActiveProperty.GetValue(ent);

            if (!isActive)
                Console.WriteLine($"NOTE: This is an inactive {entity}!");
        }
    }
}