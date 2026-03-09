using SchoolManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.DTOs
{
    public class CreateUserResponse
    {
        public int RoleId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public int? EnrollYear { get; set; }
        public string? Speciality { get; set; }
    }
}
