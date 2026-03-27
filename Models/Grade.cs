using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int EnrollmentId { get; set; }
        public double? FirstGrade { get; set; } = null;
        public double? SecondGrade { get; set; } = null;
        public double? FinalGrade { get; set; } = null;

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;


        public Enrollment? Enrollment { get; set; }
    }
}
