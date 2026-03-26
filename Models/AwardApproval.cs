namespace SchoolManagement.Models
{
    public enum Decision
    {
        Approve,Reject
    }
    public class AwardApproval
    {
        public int ApprovalId { get; set; }
        public int TeacherId { get; set; }
        public int AwardId { get; set; }
        public Decision? decision { get; set; }
        public DateTime DecisionDate { get; set; }
        public string Comment { get; set; } = string.Empty;


        public Teacher? Teacher { get; set; }
        public Award? Award { get; set; }

    }
}
