namespace SchoolManagement.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException(string message): base(message){}
        public NotFoundException(string name, object key): base($"{name} with the given {key} was not found!"){}
    }
}
