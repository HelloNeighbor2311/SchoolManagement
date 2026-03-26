using Microsoft.EntityFrameworkCore.Storage.Json;

namespace SchoolManagement.Models
{
    public enum Status
    {
        Pending, Approved, Rejected
    }
    public class Award
    {
        public int AwardId { get; set; }
        public int GpaId { get; set; }
        public int StudentId { get; set; }
        public string Description { get; set; } = string.Empty;
        public Status? status { get; set; } = Status.Pending;
        public DateTime ExpiredDate { get; set; }
        public int RequireApproval { get; set; }

        public Gpa? Gpa { get; set; }
        public Student? Student { get; set; }
        public ICollection<AwardApproval>? AwardApprovals { get; set; } = new List<AwardApproval>();

        public void SetStatus(DateTime dateNow)
        {
            if(ExpiredDate<dateNow && AwardApprovals.Count < RequireApproval)
            {
                status = Status.Rejected;
            }else if(ExpiredDate > dateNow && AwardApprovals.Count < RequireApproval)
            {
                status = Status.Pending;
            }
            else
            {
                status = Status.Approved;
            }
        }
    }
}
