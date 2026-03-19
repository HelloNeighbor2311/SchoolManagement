using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.DTOs.Semester
{
    public class CreateSemesterRequest
    {
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
