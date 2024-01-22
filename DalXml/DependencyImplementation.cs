
using Dal.Strategies.Create;
using Dal.Strategies.Delete;
using DalApi;
using DO;
using System.Xml.Linq;
using static DO.Exceptions;

namespace Dal;
internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";
    private readonly ICreationStrategy<Dependency> _creationStrategy;
    private readonly IDeletionStrategy<Dependency> _deletionStrategy;


    public DependencyImplementation()
    {
        //Internal id creation(running id) so we don't get the id in the entity itself, but we need to provide a method for the creation of it
        _creationStrategy = new InternalIdCreationStrategy<Dependency>(idGenerator: () => Config.NextDependencyId);
        //strict Deletion for dependency entity (no need for soft deletion)
        _deletionStrategy = new StrictDeletionStrategy<Dependency>(Read);
    }

    /// <summary>
    /// Helper method to parse the dependency form the XElement
    /// </summary>
    /// <param name="element">The XElement of the dependency</param>
    /// <returns>Dependency</returns>
    private Dependency ParseDependency(XElement element)
    {
        return new Dependency()
        {
            Id = int.TryParse(element.Element("Id")?.Value, out int id) ? id : 0,
            DependentTask = int.TryParse(element.Element("DependentTask")?.Value, out int dependentTask) ? dependentTask : 0,
            PreviousTask = int.TryParse(element.Element("PreviousTask")?.Value, out int previousTask) ? previousTask : 0
        };
    }

    /// <inheritdoc />
    public int Create(Dependency item)
    {
        return _creationStrategy.Create(item, XMLTools.LoadListFromXMLElement(s_dependencies_xml), XMLTools.SaveListToXMLElement, s_dependencies_xml);
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        _deletionStrategy.Delete(id, XMLTools.LoadListFromXMLElement(s_dependencies_xml), XMLTools.SaveListToXMLElement, s_dependencies_xml);
    }

    /// <inheritdoc />
    public Dependency? Read(int id)
    {
        XElement? depenElem = XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().FirstOrDefault(item => (int?)item.Element("Id") == id);
        return depenElem is null ? null : ParseDependency(depenElem);
    }

    /// <inheritdoc />
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(item => ParseDependency(item)).FirstOrDefault(filter);
    }

    /// <inheritdoc />
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(s => ParseDependency(s)).Where(item => filter?.Invoke(item) ?? true);
    }

    /// <inheritdoc />
    public void Reset()
    {
        XElement? rootElem = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        rootElem.RemoveAll();
        XMLTools.SaveListToXMLElement(rootElem, s_dependencies_xml);
    }

    /// <inheritdoc />
    public void Update(Dependency dep)
    {
        XElement? rootElem = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? depenElem = rootElem.Elements().FirstOrDefault(item => (int?)item.Element("Id") == dep.Id);
        if (depenElem is null)
        {
            throw new DalDoesNotExistException($"Dependency with ID={dep.Id} does not exist");
        }
        else
        {
            depenElem.SetElementValue("DependentTask", dep.DependentTask);
            depenElem.SetElementValue("PreviousTask", dep.PreviousTask);

            XMLTools.SaveListToXMLElement(rootElem, s_dependencies_xml);
        }

    }
}
