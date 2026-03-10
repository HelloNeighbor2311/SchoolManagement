namespace SchoolManagement.Exceptions
{
    public class BadRequestException: Exception
    {
        public BadRequestException(string message) : base(message) { }
        public BadRequestException(string name, Exception innerException) : base(name, innerException) { }
    }
}
