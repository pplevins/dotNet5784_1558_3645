
namespace Dal.strategies.create;
using System.Collections.Generic;

/// <summary>
/// Interface for creation strategy.
/// </summary>
/// <typeparam name="T">The type of the items to create.</typeparam>
public interface ICreationStrategy<T>
{
    int Create(List<T> items, T item);
}

