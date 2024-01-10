namespace DalApi;

public interface ICrud<T> where T : class
{
    int Create(T item); // Creates a new entity object in DAL
    T? Read(int id); // Reads an entity object by its ID
    T? Read(Func<T, bool> filter); // stage 2

    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null); // stage 2
    void Update(T item); // Updates an entity object
    void Delete(int id); // Deletes an object by its ID

    void Reset(); //reset all the list
}