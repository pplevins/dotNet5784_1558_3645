using Dal.Strategies.Create;
using Dal.Strategies.Delete;
using DalApi;
using DO;
using static DO.Exceptions;

namespace Dal;

public class UserImplementation : IUser
{
    private readonly ICreationStrategy<User> _creationStrategy;
    private readonly IDeletionStrategy<User> _deletionStrategy;

    readonly string s_users_xml = "users";

    public UserImplementation()
    {
        //External id creation so we get the id frm outside in the entity itself
        _creationStrategy = new ExternalIdCreationStrategy<User>(Read);
        //soft Deletion(only marks the entity as non-active) with proper Exception in case of error
        _deletionStrategy = new SoftDeletionStrategy<User>(Read, Update);
    }
    /// <inheritdoc />
    public int Create(User item)
    {
        return _creationStrategy.Create(item, XMLTools.LoadListFromXMLSerializer<User>(s_users_xml), XMLTools.SaveListToXMLSerializer, s_users_xml);
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        _deletionStrategy.Delete(id, users, XMLTools.SaveListToXMLSerializer);
    }

    /// <inheritdoc />
    public User? Read(int id)
    {
        // Find and return the User with the specified ID or null if not found
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        return users.FirstOrDefault(d => d.Id == id);
    }

    /// <inheritdoc />
    public User? Read(Func<User, bool> filter)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        return users.FirstOrDefault(filter);
    }

    /// <inheritdoc />
    public IEnumerable<User?> ReadAll(Func<User, bool>? filter = null) //stage 3
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        return users.Where(item => item.IsActive && (filter?.Invoke(item) ?? true));
    }

    /// <inheritdoc />
    public void Update(User item)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        // Find the existing User in the list by ID
        var existingUserIndex = users.FindIndex(d => d.Id == item.Id);

        if (existingUserIndex != -1)
        {
            // Replace the existing User in the list
            users[existingUserIndex] = item;
            XMLTools.SaveListToXMLSerializer<User>(users, s_users_xml);
        }
        else
        {
            throw new DalDoesNotExistException($"User with ID={item.Id} does not exist");
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        // Clear the list of users
        users.Clear();
        XMLTools.SaveListToXMLSerializer<User>(users, s_users_xml);
    }
}