namespace SchoolManagement.Models
{
    public class Teacher : User
    {
        public string Speciality { get; set; } = string.Empty;

        public List<TeacherCourseSemester> TeacherCourses { get; set; } = new List<TeacherCourseSemester>();
        public List<AwardApproval> AwardApprovals { get; set; } = new List<AwardApproval>();
    }
}
