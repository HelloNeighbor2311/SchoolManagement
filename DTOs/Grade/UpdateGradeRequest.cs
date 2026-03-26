using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.DTOs.Grade
{
    public class UpdateGradeRequest
    {
        [Range(0,10,ErrorMessage = "Grade must be between 0 and 10")]
        public double? FirstGrade { get; set; } 
        [Range(0,10,ErrorMessage = "Grade must be between 0 and 10")]
        public double? SecondGrade { get; set; }
    }
}
