namespace SchoolManagement.Models
{
    public class CourseSemester
    {
        public int CourseSemesterId { get; set; }
        public int CourseId { get; set; }
        public int SemesterId { get; set; }

        public ICollection<Enrollment> Enrollment { get; set; } = new List<Enrollment>();
        public ICollection<TeacherCourseSemester> TeacherCourseSemester { get; set; } = new List<TeacherCourseSemester>();
        public Course? Course { get; set; }
        public Semester? Semester { get; set; }
    }
}
