using SchoolManagement.DTOs.CourseSemester;

namespace SchoolManagement.DTOs.Semester
{
    public class SemesterDetailResponse: SemesterResponse
    {
        public List<CourseSemesterResponse> Detail { get; set; } = new();
    }
}
