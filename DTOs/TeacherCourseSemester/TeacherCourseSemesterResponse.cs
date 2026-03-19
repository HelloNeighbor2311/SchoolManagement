namespace SchoolManagement.DTOs.TeacherCourseSemester
{
    public class TeacherCourseSemesterResponse
    {
        public int TeacherCourseSemesterId { get; set; }
        public int CourseSemesterId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string SemesterDescription { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
    }
}
