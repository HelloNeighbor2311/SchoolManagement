using AutoMapper;
using Azure.Core;
using SchoolManagement.DTOs.Enrollment;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class EnrollmentService(IUnitOfWork uow, IMapper mapper, ILogger<EnrollmentService> logger) : IEnrollmentService
    {
        public async Task DeleteEnrollment(int id)
        {
            using (logger.BeginOperationScope("DeleteEnrollment", ("EnrollmentId", id)))
            using (var timer = logger.TimeOperation("LoginAsync"))
            {
                var enrollment = await uow.Enrollment.GetEnrollmentByIdAsync(id);
                if (enrollment is null) throw new NotFoundException($"The enrollment with the Id {id} was not found");
                try
                {
                    await uow.Enrollment.DeleteEnrollmentAsync(enrollment);
                    await uow.SaveChangeAsync();
                    logger.LogEntityDeleted<Enrollment>("Enrollment", id);
                }catch(Exception e)
                {
                    logger.LogOperationError("DeleteEnrollment", e, id);
                    throw;
                }
            }
        }

        public async Task<List<EnrollmentResponse>> GetAllEnrollments()
        {
            using (logger.BeginOperationScope("GetAllEnrollments"))
            using (var timer = logger.TimeOperation("GetAllEnrollments"))
            {
                try
                {
                    var result = await uow.Enrollment.GetAllEnrollmentInformationAsync();
                    logger.LogInformation("Retrieved {Count} Enrollments", result.Count);
                    return result;
                }catch(Exception e)
                {
                    logger.LogOperationError("GetAllEnrollments", e);
                    throw;
                }
            }
        }

        public async Task<EnrollmentResponse> RegisterEnrollment(int studentId, RegisterEnrollmentRequest request)
        {
            using (logger.BeginOperationScope("RegisterEnrollment", ("StudentId", studentId), ("CourseSemesterId", request.CourseSemesterId)))
            using (var timer = logger.TimeOperation("RegisterEnrollment"))
            {
                if (!await uow.User.IsStudentAsync(studentId)) throw new NotFoundException($"The student with the Id {studentId} was not found");
                if (!await uow.CourseSemester.ExistsAsync(u => u.CourseSemesterId == request.CourseSemesterId)) throw new NotFoundException($"The course semester with the Id {request.CourseSemesterId} was not found");
                if (await uow.Enrollment.ExistsAsync(p => p.StudentId == studentId && p.CourseSemesterId == request.CourseSemesterId)) throw new ConflictException($"The student with Id {studentId} is already enrolled in the Course with Id {request.CourseSemesterId}");
                var courseSemester = await uow.CourseSemester.GetCourseSemesterByIdAsync(request.CourseSemesterId);
                var enrollment = mapper.Map<Enrollment>(request);
                enrollment.Grade = new Grade
                {
                    FirstGrade = null,
                    SecondGrade = null,
                    FinalGrade = null
                };
                var isGpaExist = await uow.Gpa.ExistsAsync(g => g.StudentId == studentId && g.SemesterId == courseSemester!.SemesterId);
                if (!isGpaExist)
                {
                    var newGpa = new Gpa
                    {
                        rank = null,
                        gpa = null,
                        StudentId = studentId,
                        SemesterId = courseSemester!.SemesterId
                    };
                    await uow.Gpa.AddGpaAsync(newGpa);
                }
                try
                {
                    await uow.Enrollment.RegisterEnrollmentAsync(enrollment);
                    await uow.SaveChangeAsync();
                    var newEnrollment = await uow.Enrollment.GetEnrollmentByIdAsync(enrollment.EnrollmentId);
                    return mapper.Map<EnrollmentResponse>(newEnrollment);
                }catch(Exception e)
                {
                    logger.LogOperationError("RegisterEnrollment", e, studentId, request.CourseSemesterId);
                    throw;
                }
            }
        }
    }
}
