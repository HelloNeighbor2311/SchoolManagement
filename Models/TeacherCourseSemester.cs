namespace SchoolManagement.Models
{
    public class TeacherCourseSemester
    {
        public int TeacherCourseSemesterId { get; set; }
        public int CourseSemesterId { get; set; }
        public int TeacherId { get; set; }

        public Teacher? Teacher { get; set; }
        public CourseSemester? CourseSemester { get; set; }
    }
}
