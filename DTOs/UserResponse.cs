using SchoolManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.DTOs
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public int? EnrollYear { get; set; }
        public string? Speciality { get; set; }
    }
}
