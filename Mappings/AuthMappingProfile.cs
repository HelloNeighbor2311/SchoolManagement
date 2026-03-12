using AutoMapper;
using SchoolManagement.DTOs;
using SchoolManagement.DTOs.Authentication;
using SchoolManagement.Models;

namespace SchoolManagement.Mappings
{
    public class AuthMappingProfile: Profile
    {
        public AuthMappingProfile()
        {

            //For Admin
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
        }
    }
}
