namespace SchoolManagement.DTOs
{
    public class UpdateUserResponse
    {
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? EnrollYear { get; set; }
        public string? Speciality { get; set; }
    }
}
