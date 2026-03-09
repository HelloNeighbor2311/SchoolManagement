namespace SchoolManagement.Models
{
    public enum Rank
    {
        Excellent,
        Good,
        Average,
        Bad
    }
    public class Gpa
    {
        public int GPAId { get; set; }
        public int StudentId { get; set; }
        public int SemesterId { get; set; }
        public int gpa { get; set; }
        public Rank rank { get; set; }

        public Semester? Semester { get; set; }
        public Student? Student { get; set; }
        public Award? Award { get; set; }
    }
}
