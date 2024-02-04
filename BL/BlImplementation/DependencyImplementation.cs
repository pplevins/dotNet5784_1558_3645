using BlApi;
using BO;

namespace BlImplementation;

/// <summary>
/// Implementation of the <see cref="IDependency"/> interface to manage dependencies in the data source.
/// </summary>
internal class DependencyImplementation : IDependency
{
    public int Create(Milestone item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Milestone? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Milestone? Read(Func<Milestone, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Milestone?> ReadAll(Func<Milestone, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Update(Milestone item)
    {
        throw new NotImplementedException();
    }
}

