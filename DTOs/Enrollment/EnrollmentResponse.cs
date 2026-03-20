using SchoolManagement.Models;

namespace SchoolManagement.DTOs.Enrollment
{
    public class EnrollmentResponse
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseSemesterId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string SemesterDescription { get; set; } = string.Empty;

    }
}
