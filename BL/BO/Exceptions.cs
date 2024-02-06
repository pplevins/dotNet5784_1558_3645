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
    public class BlAlreadyExistsException : Exception
    {
        public BlAlreadyExistsException(string? message) : base(message) { }
        public BlAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    [Serializable]
    public class BlDeletionImpossibleException : Exception
    {
        public BlDeletionImpossibleException(string? message) : base(message) { }
        public BlDeletionImpossibleException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    [Serializable]
    public class BlXMLFileLoadCreateException(string? message) : Exception(message);

    [Serializable]
    public class BlUpdateCreateImpossibleException(string? message) : Exception(message);

}