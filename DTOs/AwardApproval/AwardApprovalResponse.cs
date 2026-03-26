using SchoolManagement.Models;

namespace SchoolManagement.DTOs.AwardApproval
{
    public class AwardApprovalResponse
    {
        public int ApprovalId { get; set; }
        public int AwardId { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string decision { get; set; } = string.Empty;
        public DateTime DecisionDate { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
