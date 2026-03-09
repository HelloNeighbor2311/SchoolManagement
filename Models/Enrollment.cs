namespace SchoolManagement.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseSemesterId { get; set; }
        public Student? Student { get; set; }
        public ICollection<Grade> Grade { get; set; } = new List<Grade>();
        public CourseSemester? CourseSemester { get; set; }


    }
}
