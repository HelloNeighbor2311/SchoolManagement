using SchoolManagement.Models;

namespace SchoolManagement.DTOs.Award
{
    public class CreateAwardRequest
    {
        public int GpaId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ExpiredDate { get; set; }
        public int RequireApproval { get; set; }
    }
}
