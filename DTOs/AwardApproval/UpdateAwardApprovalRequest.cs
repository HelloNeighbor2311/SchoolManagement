namespace SchoolManagement.DTOs.AwardApproval
{
    public class UpdateAwardApprovalRequest
    {
        public string? decision { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
