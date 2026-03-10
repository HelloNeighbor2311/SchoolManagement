namespace SchoolManagement.Exceptions
{
    public class ValidationException: Exception
    {
        public List<string> Errors { get; set; }
        public ValidationException(): base("Validation errors occured")
        {
            Errors = new List<string>();
        }
        public ValidationException(List<string> errors): base("Validation errors occured")
        {
            Errors = errors;
        }
        public ValidationException(string errors): base("Validation errors occured")
        {
            Errors = new List<string>() { errors };
        }

    }
}
