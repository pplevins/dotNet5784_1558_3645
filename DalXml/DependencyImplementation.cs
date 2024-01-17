
using Dal.Strategies.Create;
using Dal.Strategies.Delete;
using DalApi;
using DO;
using System.Xml.Linq;

namespace Dal;
internal class DependencyImplementation : IDependency
{
    const string s_dependencies_file_name = "dependencies";
    private readonly ICreationStrategy<Dependency> _creationStrategy;
    private readonly IDeletionStrategy<Dependency> _deletionStrategy;


    public DependencyImplementation()
    {
        //Internal id creation(running id) so we don't get the id in the entity itself, but we need to provide a method for the creation of it
        _creationStrategy = new InternalIdCreationStrategy<Dependency>(idGenerator: () => Config.NextDependencyId);
        //strict Deletion for dependency entity (no need for soft deletion)
        _deletionStrategy = new StrictDeletionStrategy<Dependency>(Read);
    }


    public IEnumerable<Dependency> GetDataOf(Func<Dependency, bool>? filter = null)
    {
        XElement dependencyRoot = XMLTools.LoadListFromXMLElement(s_dependencies_file_name);
        return from d in dependencyRoot.Elements()
               let dependency = ParseDependency(d)
               where filter?.Invoke(dependency) ?? true
               select dependency;

    }

    private Dependency ParseDependency(XElement element)
    {
        return new Dependency()
        {
            Id = int.TryParse(element.Element("Id")?.Value, out int id) ? id : 0,
            DependentTask = int.TryParse(element.Element("DependentTask")?.Value, out int dependentTask) ? dependentTask : 0,
            PreviousTask = int.TryParse(element.Element("PreviousTask")?.Value, out int previousTask) ? previousTask : 0
        };
    }

    public int Create(Dependency item)
    {
        return _creationStrategy.Create(item, XMLTools.LoadListFromXMLElement(s_dependencies_file_name), XMLTools.SaveListToXMLElement);
    }

    public void Delete(int id)
    {
        _deletionStrategy.Delete(id, XMLTools.LoadListFromXMLElement(s_dependencies_file_name), XMLTools.SaveListToXMLElement);
    }

    public Dependency? Read(int id)
    {
        return GetDataOf().FirstOrDefault(d => d?.Id == id);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return GetDataOf().FirstOrDefault(filter);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        return GetDataOf(filter);
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}
