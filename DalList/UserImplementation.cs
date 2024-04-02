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
        return _creationStrategy.Create(item, DataSource.Users);
    }

    /// <inheritdoc />
    public void Delete(int id)
    {
        //regular Deletion with proper Exception in case of error
        _deletionStrategy.Delete(id, DataSource.Users);
    }

    /// <inheritdoc />
    public User? Read(int id)
    {
        // Find and return the User with the specified ID or null if not found.
        //Even when the User is inactive, returns the entity with isActive = false
        return DataSource.Users.FirstOrDefault(d => d.Id == id);
    }

    /// <inheritdoc />
    public User? Read(Func<User, bool> filter)
    {
        return DataSource.Users.FirstOrDefault(filter);
    }

    /// <inheritdoc />
    public IEnumerable<User?> ReadAll(Func<User, bool>? filter = null)
    {
        //returns all the active Users
        return DataSource.Users.Where(item => item.IsActive && (filter?.Invoke(item) ?? true));
    }

    /// <inheritdoc />
    public void Update(User item)
    {
        // Find the existing User in the list by ID
        var existingUserIndex = DataSource.Users.FindIndex(d => d.Id == item.Id);

        if (existingUserIndex != -1)
        {
            // Replace the existing User in the list
            DataSource.Users[existingUserIndex] = item;
        }
        else
        {
            throw new DalDoesNotExistException($"User with ID={item.Id} does not exist");
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        // Clear the list of Users
        DataSource.Users.Clear();
    }
}
