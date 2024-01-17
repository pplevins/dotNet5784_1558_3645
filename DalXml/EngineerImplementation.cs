

using Dal.Strategies.Create;
using Dal.Strategies.Delete;
using DalApi;
using DO;
using static DO.Exceptions;

namespace Dal;
internal class EngineerImplementation : IEngineer
{
    private readonly ICreationStrategy<Engineer> _creationStrategy;
    private readonly IDeletionStrategy<Engineer> _deletionStrategy;

    private const string s_engineers_file_name = "engineers";

    public EngineerImplementation()
    {
        //External id creation so we get the id frm outside in the entity itself
        _creationStrategy = new ExternalIdCreationStrategy<Engineer>(Read);
        //soft Deletion(only marks the entity as non-active) with proper Exception in case of error
        _deletionStrategy = new SoftDeletionStrategy<Engineer>(Read, Update);
    }
    /// <inheritdoc />
    public int Create(Engineer item)
    {
        return _creationStrategy.Create(item, XMLTools.LoadListFromXMLElement(s_engineers_file_name));
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_file_name);
        _deletionStrategy.Delete(id, engineers, XMLTools.SaveListToXMLSerializer);
    }

    /// <inheritdoc />
    public Engineer? Read(int id)
    {
        // Find and return the Engineer with the specified ID or null if not found
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_file_name);
        return engineers.FirstOrDefault(d => d.Id == id);
    }

    /// <inheritdoc />
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_file_name);
        return engineers.FirstOrDefault(filter);
    }

    /// <inheritdoc />
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null) //stage 3
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_file_name);
        return engineers.Where(item => item.IsActive && (filter?.Invoke(item) ?? true));
    }

    /// <inheritdoc />
    public void Update(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_file_name);
        // Find the existing Engineer in the list by ID
        var existingEngineerIndex = engineers.FindIndex(d => d.Id == item.Id);

        if (existingEngineerIndex != -1)
        {
            // Replace the existing Engineer in the list
            engineers[existingEngineerIndex] = item;
            XMLTools.SaveListToXMLSerializer<Engineer>(engineers, s_engineers_file_name);
        }
        else
        {
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_file_name);
        // Clear the list of engineers
        engineers.Clear();
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, s_engineers_file_name);
    }
}
