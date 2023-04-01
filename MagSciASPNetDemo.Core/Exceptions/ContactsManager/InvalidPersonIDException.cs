namespace ContactsManagement.Core.Exceptions.ContactsManager
{
    public class InvalidPersonIDException : ArgumentException
    {
        public InvalidPersonIDException()
        {
        }

        public InvalidPersonIDException(string? message) : base(message)
        {
        }

        public InvalidPersonIDException(string? message, Exception? innerException) : base(message, innerException)
        {
            //If there is a database exception, you can add the exception as an innerException arg
        }
    }
}