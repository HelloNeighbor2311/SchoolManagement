namespace SchoolManagement.DTOs.Grade
{
    public class GradeResponse
    {
        public int GradeId { get; set; }
        public int EnrollmentId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string SemesterDescription { get; set; } = string.Empty;
        public double? FirstGrade { get; set; }
        public double? SecondGrade { get; set; }
        public double? FinalGrade { get; set; }

        public string RowVersion { get; set; } = string.Empty;
    }
}
