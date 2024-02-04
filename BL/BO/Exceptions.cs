namespace BO;

public class Exceptions
{
    [Serializable]
    public class BlDoesNotExistException : Exception
    {
        public BlDoesNotExistException(string? message) : base(message) { }
        public BlDoesNotExistException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    [Serializable]
    public class BlNullPropertyException(string? message) : Exception(message);

    [Serializable]
    public class BlAlreadyExistsException(string? message) : Exception(message);

    [Serializable]
    public class BlDeletionImpossibleException(string? message) : Exception(message);

    [Serializable]
    public class BlXMLFileLoadCreateException(string? message) : Exception(message);

}