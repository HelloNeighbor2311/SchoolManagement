namespace SchoolManagement.Exceptions
{
    public class ConflictException: Exception
    {
        public ConflictException(string message): base(message){}
        public ConflictException(string name, Exception innerException): base(name, innerException){}
    }
}
