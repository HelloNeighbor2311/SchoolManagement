using SchoolManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.DTOs.User
{
    public class CreateUserResponse
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public int? EnrollYear { get; set; }
        public string? Speciality { get; set; }
    }
}
