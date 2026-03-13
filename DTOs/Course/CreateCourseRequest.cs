using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.DTOs.Course
{
    public class CreateCourseRequest
    {
        [Required]
        public string CourseName { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
