namespace SchoolManagement.DTOs.Grade
{
    public class GradeResponse
    {
        public int GradeId { get; set; }
        public int EnrollmentId { get; set; }
        public int? FirstGrade { get; set; }
        public int? SecondGrade { get; set; }
        public int? FinalGrade { get; set; }
    }
}
