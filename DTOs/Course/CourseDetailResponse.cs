using SchoolManagement.DTOs.CourseSemester;

namespace SchoolManagement.DTOs.Course
{
    public class CourseDetailResponse: CourseResponse
    {
        public List<CourseSemesterResponse> Detail { get; set; } = new List<CourseSemesterResponse>();
    }
}
