
using System.Xml.Linq;

namespace Dal.Strategies.Create;
/// <summary>
/// The ICreationStrategy interface defines a strategy pattern for creating items of type T.
/// It features a Create method, accepting a list of items and a specific item to be created.
/// This interface enables the implementation of different creation algorithms, providing flexibility and extensibility in handling various item creation scenarios.
/// The strategy pattern allows dynamic selection of the creation strategy at runtime, promoting modular and adaptable code architecture.
/// </summary>
public interface ICreationStrategy<T>
{
    int Create(T item, List<T> source, Action<List<T>, string>? saveFunction = null, string? fileName = null);
    int Create(T item, XElement source, Action<XElement, string>? saveFunction = null, string? fileName = null);
}

