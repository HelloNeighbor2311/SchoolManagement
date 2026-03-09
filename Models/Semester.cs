namespace SchoolManagement.Models
{
    public class Semester
    {
        public int SemesterId { get; set; }
        public string Description { get; set; } = string.Empty;

        public Gpa? Gpa { get; set; }
        public ICollection<CourseSemester> CourseSemester { get; set; } = new List<CourseSemester>();
    }
}
