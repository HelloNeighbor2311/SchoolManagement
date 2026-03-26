using AutoMapper;
using SchoolManagement.DTOs.Authentication;
using SchoolManagement.DTOs.Award;
using SchoolManagement.DTOs.AwardApproval;
using SchoolManagement.DTOs.Course;
using SchoolManagement.DTOs.CourseSemester;
using SchoolManagement.DTOs.Enrollment;
using SchoolManagement.DTOs.Gpa;
using SchoolManagement.DTOs.Grade;
using SchoolManagement.DTOs.Semester;
using SchoolManagement.DTOs.TeacherCourseSemester;
using SchoolManagement.DTOs.User;
using SchoolManagement.Models;

namespace SchoolManagement.Mappings
{
    public class AuthMappingProfile: Profile
    {
        public AuthMappingProfile()
        {

            //Auth
            CreateMap<CreateUserResponse, Admin>().
                ForMember(destination => destination.PasswordHashed, opt => opt.Ignore()).
                ForMember(destination => destination.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<CreateUserResponse, Teacher>().
                ForMember(destination => destination.PasswordHashed, opt => opt.Ignore()).
                ForMember(destination => destination.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<CreateUserResponse, Student>().
                ForMember(destination => destination.PasswordHashed, opt => opt.Ignore()).
                ForMember(destination => destination.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UpdateUserRequest, Admin>().
                ForMember(destination => destination.PasswordHashed, opt => opt.Ignore());
            CreateMap<UpdateUserRequest, Teacher>().
                ForMember(destination => destination.PasswordHashed, opt => opt.Ignore());
            CreateMap<UpdateUserRequest, Student>().
                ForMember(destination => destination.PasswordHashed, opt => opt.Ignore());
            CreateMap<Admin, UserResponse>().ForMember(destination => destination.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<Student, UserResponse>().ForMember(destination => destination.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<Teacher, UserResponse>().ForMember(destination => destination.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<RegisterStudentRequest, Student>().
                 ForMember(destination => destination.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow)).
                 ForMember(destination => destination.PasswordHashed, opt => opt.Ignore()).
                 ForMember(destination => destination.RoleId, opt => opt.MapFrom(_ => 2));
            CreateMap<RegisterTeacherRequest, Teacher>().
                ForMember(destination => destination.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow)).
                ForMember(destination => destination.PasswordHashed, opt => opt.Ignore()).
                ForMember(destination => destination.RoleId, opt => opt.MapFrom(_ => 3));
            CreateMap<CreateAwardApprovalRequest, AwardApproval>().ForMember(destination => destination.DecisionDate, opt => opt.Ignore());
            CreateMap<CreateCourseRequest,Course>();
            CreateMap<CreateSemesterRequest,Semester>();
            CreateMap<CreateCourseSemesterRequest,CourseSemester>();
            CreateMap<CreateAwardRequest, Award>().
                ForMember(destination => destination.StudentId, opt => opt.Ignore());
            CreateMap<AllocateTeacherCourseSemesterRequest,TeacherCourseSemester>();

            //Entities
            CreateMap<Course, CourseResponse>();
            CreateMap<Semester,SemesterResponse>();
            CreateMap<Course, CourseDetailResponse>().ForMember(destination => destination.Detail, src => src.MapFrom(u => u.CourseSemester));
            CreateMap<Semester, SemesterDetailResponse>().ForMember(destination => destination.Detail, src => src.MapFrom(u => u.CourseSemester));
            CreateMap<CourseSemester, CourseSemesterResponse>().
                ForMember(destination => destination.CourseName, opt => opt.MapFrom(u => u.Course != null ? u.Course.CourseName : string.Empty)).
                ForMember(destination => destination.SemesterDescription, opt => opt.MapFrom(u => u.Semester != null ? u.Semester.Description: string.Empty));
            CreateMap<TeacherCourseSemester, TeacherCourseSemesterResponse>().
                ForMember(destination => destination.CourseName, src => src.MapFrom(u => u.CourseSemester != null ? u.CourseSemester.Course!.CourseName : string.Empty)).
                ForMember(destination => destination.SemesterDescription, src => src.MapFrom(u => u.CourseSemester != null ? u.CourseSemester.Semester!.Description : string.Empty)).
                ForMember(destination => destination.TeacherName, src => src.MapFrom(u => u.Teacher != null ? u.Teacher.Name : string.Empty));
            CreateMap<Enrollment, EnrollmentResponse>().
                ForMember(destination => destination.StudentName, opt => opt.MapFrom(u => u.Student != null ? u.Student.Name : string.Empty)).
                ForMember(destination => destination.CourseName, opt => opt.MapFrom(u => u.CourseSemester != null ? u.CourseSemester.Course!.CourseName : string.Empty)).
                ForMember(destination => destination.SemesterDescription, opt => opt.MapFrom(u => u.CourseSemester != null ? u.CourseSemester.Semester!.Description : string.Empty));
            CreateMap<RegisterEnrollmentRequest, Enrollment>();
            CreateMap<Grade, GradeResponse>().
                ForMember(destination => destination.CourseName, opt => opt.MapFrom(u => u.Enrollment!.CourseSemester != null ? u.Enrollment.CourseSemester.Course!.CourseName : string.Empty)).
                ForMember(destination => destination.SemesterDescription, opt => opt.MapFrom(u => u.Enrollment!.CourseSemester != null ? u.Enrollment.CourseSemester.Semester!.Description : string.Empty));
            CreateMap<Gpa, GpaResponse>().
                ForMember(destination => destination.StudentName, opt => opt.MapFrom(u => u.Student != null ? u.Student.Name : string.Empty)).
                ForMember(destination => destination.SemesterDescription, opt => opt.MapFrom(u => u.Semester != null ? u.Semester.Description : string.Empty));
            CreateMap<Award, AwardResponse>().
                ForMember(destination => destination.Gpa, opt => opt.MapFrom(u => u.Gpa!.gpa)).
                ForMember(destination => destination.StudentName, opt => opt.MapFrom(u => u.Student!.Name)).
                ForMember(destination => destination.status, opt => opt.MapFrom(u => u.status.ToString()));
            CreateMap<AwardApproval, AwardApprovalResponse>().
                ForMember(destination => destination.TeacherName, opt => opt.MapFrom(u => u.Teacher!.Name));
        }
    }
}
