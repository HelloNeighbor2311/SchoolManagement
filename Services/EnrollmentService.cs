using AutoMapper;
using Azure.Core;
using SchoolManagement.DTOs.Enrollment;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class EnrollmentService(IUnitOfWork uow, IMapper mapper) : IEnrollmentService
    {
        public async Task DeleteEnrollment(int id)
        {
            var enrollment = await uow.Enrollment.GetEnrollmentByIdAsync(id);
            if (enrollment is null) throw new NotFoundException($"The enrollment with the Id {id} was not found");
            await uow.Enrollment.DeleteEnrollmentAsync(enrollment);
            await uow.SaveChangeAsync();
        }

        public async Task<List<EnrollmentResponse>> GetAllEnrollments()
        {
            var result =  await uow.Enrollment.GetAllEnrollmentInformationAsync();
            var enrollmentResponse = result.Select(u=>mapper.Map<EnrollmentResponse>(u)).ToList();
            return enrollmentResponse;
        }

        public async Task<EnrollmentResponse> RegisterEnrollment(RegisterEnrollmentRequest request)
        {
            if (await uow.User.GetUserByIdAsync(request.StudentId) == null) throw new NotFoundException($"The student with the Id {request.StudentId} was not found");
            if (await uow.CourseSemester.GetCourseSemesterByIdAsync(request.CourseSemesterId) == null) throw new NotFoundException($"The course semester with the Id {request.CourseSemesterId} was not found");
            if (await uow.Enrollment.ExistsAsync(p => p.StudentId == request.StudentId && p.CourseSemesterId == request.CourseSemesterId)) throw new ConflictException($"The student with Id {request.StudentId} is already enrolled in the Course with Id {request.CourseSemesterId}");
            var enrollment = mapper.Map<Enrollment>(request);
            await uow.Enrollment.RegisterEnrollmentAsync(enrollment);
            await uow.SaveChangeAsync();
            var newEnrollment = await uow.Enrollment.GetEnrollmentByIdAsync(enrollment.EnrollmentId);
            return mapper.Map<EnrollmentResponse>(newEnrollment);
        }
    }
}
