namespace SchoolManagement.DTOs.User
{
    public class UpdateUserRequest
    {
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? EnrollYear { get; set; }
        public string? Speciality { get; set; }

        public string RowVersion { get; set; } = string.Empty;
    }
}
