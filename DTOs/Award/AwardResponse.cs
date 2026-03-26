using SchoolManagement.Models;

namespace SchoolManagement.DTOs.Award
{
    public class AwardResponse
    {
        public int AwardId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public double Gpa { get; set; }
        public string Description { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public int RequireApproval { get; set; }
    }
}
