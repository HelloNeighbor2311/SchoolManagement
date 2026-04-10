using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.DTOs.Authentication
{
    public class RegisterTeacherRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Speciality { get; set; } = string.Empty;
    }
}
