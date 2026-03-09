namespace SchoolManagement.DTOs
{
    public class StudentResponse
    {
        public int UserId { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public int EnrollYear { get; set; }
    }
}
