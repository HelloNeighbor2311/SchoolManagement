namespace SchoolManagement.DTOs.AwardApproval
{
    public class CreateAwardApprovalRequest
    {
        public int AwardId { get; set; }
        public int TeacherId { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
