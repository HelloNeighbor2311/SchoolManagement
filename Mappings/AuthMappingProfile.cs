using AutoMapper;
using SchoolManagement.DTOs;
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
            CreateMap<UpdateUserResponse, Admin>().
                ForMember(destination => destination.PasswordHashed, opt => opt.Ignore());
            CreateMap<UpdateUserResponse, Teacher>().
                ForMember(destination => destination.PasswordHashed, opt => opt.Ignore());
            CreateMap<UpdateUserResponse, Student>().
                ForMember(destination => destination.PasswordHashed, opt => opt.Ignore());
            CreateMap<Admin, UserResponse>().ForMember(destination => destination.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<Student, UserResponse>().ForMember(destination => destination.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<Teacher, UserResponse>().ForMember(destination => destination.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
        }
    }
}
