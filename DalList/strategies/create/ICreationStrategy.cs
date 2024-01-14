
namespace Dal.Strategies.Create;
using System.Collections.Generic;

/// <summary>
/// The ICreationStrategy interface defines a strategy pattern for creating items of type T.
/// It features a Create method, accepting a list of items and a specific item to be created.
/// This interface enables the implementation of different creation algorithms, providing flexibility and extensibility in handling various item creation scenarios.
/// The strategy pattern allows dynamic selection of the creation strategy at runtime, promoting modular and adaptable code architecture.
/// </summary>
public interface ICreationStrategy<T>
{
    int Create(List<T> items, T item);
}

