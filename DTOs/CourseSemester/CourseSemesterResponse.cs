namespace SchoolManagement.DTOs.CourseSemester
{
    public class CourseSemesterResponse
    {
        public int CourseSemesterId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterDescription { get; set; } = string.Empty;
    }
}
