namespace SchoolManagement.DTOs.Course
{
    public class CourseResponse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string? Description { get; set; } = string.Empty;
    }
}
