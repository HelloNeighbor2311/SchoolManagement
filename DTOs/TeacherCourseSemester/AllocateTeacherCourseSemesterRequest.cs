using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.DTOs.TeacherCourseSemester
{
    public class AllocateTeacherCourseSemesterRequest
    {
        [Required]
        public int CourseSemesterId { get; set; }
        [Required]
        public int TeacherId { get; set; }
      
    }
}
