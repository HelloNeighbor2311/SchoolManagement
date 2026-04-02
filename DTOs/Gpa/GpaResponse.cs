using SchoolManagement.Models;

namespace SchoolManagement.DTOs.Gpa
{
    public class GpaResponse
    {
        public int GPAId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string SemesterDescription { get; set; } = string.Empty;
        public int TotalCredits { get; set; }
        public double? gpa { get; set; } = null;
        public string Rank { get; set; } = string.Empty;
    }
}
