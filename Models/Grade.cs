namespace SchoolManagement.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int EnrollmentId { get; set; }
        public int FirstGrade { get; set; }
        public int SecondGrade { get; set; }
        public int FinalGrade { get; set; }


        public Enrollment? Enrollment { get; set; }
    }
}
