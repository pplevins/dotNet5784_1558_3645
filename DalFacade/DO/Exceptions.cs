

namespace DO;
public class Exceptions
{
    [Serializable]
    public class DalDoesNotExistException(string? message) : Exception(message);

    [Serializable]
    public class DalAlreadyExistsException(string? message) : Exception(message);

    [Serializable]
    public class DalDeletionImpossibleException(string? message) : Exception(message);
}
