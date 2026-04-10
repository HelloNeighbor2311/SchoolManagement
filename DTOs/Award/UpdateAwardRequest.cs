using SchoolManagement.Models;

namespace SchoolManagement.DTOs.Award
{
    public class UpdateAwardRequest
    {
        public string Description { get; set; } = string.Empty;
        public int RequireApproval { get; set; }
        public string RowVersion { get; set; } = string.Empty;
    }
}
