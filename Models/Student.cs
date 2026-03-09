namespace SchoolManagement.Models
{
    public class Student: User
    {
        public int EnrollYear { get; set; }

        public ICollection<Enrollment> Enrollment { get; set; } = new List<Enrollment>();
        public ICollection<Gpa> Gpa { get; set; } = new List<Gpa>();
        public ICollection<Award> Award { get; set; } = new List<Award>();
        
    }
}
