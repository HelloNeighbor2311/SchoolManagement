namespace SchoolManagement.DTOs
{
    public class TeacherResponse
    {
        public int UserId { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string Speciality { get; set; }
    }
}
