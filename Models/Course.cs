namespace SchoolManagement.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<CourseSemester> CourseSemester { get; set; } = new List<CourseSemester>();
    }
}
