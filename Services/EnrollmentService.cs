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
    public class EnrollmentService(IUnitOfWork uow, ILogger<EnrollmentService> logger) : IEnrollmentService
    {
        public async Task DeleteEnrollment(int enrollmentId)
        {
            using (logger.BeginOperationScope("DeleteEnrollment", ("EnrollmentId", enrollmentId)))
            using (var timer = logger.TimeOperation("DeleteEnrollment"))
            {
                var enrollment = await uow.Enrollment.GetEnrollmentByIdAsync(enrollmentId);
                if (enrollment is null) throw new NotFoundException($"The enrollment with the Id {enrollmentId} was not found");
                try
                {
                    await uow.Enrollment.DeleteEnrollmentAsync(enrollment);
                    await uow.SaveChangeAsync();
                    logger.LogEntityDeleted("Enrollment", enrollmentId);
                }catch(Exception e)
                {
                    logger.LogOperationError("DeleteEnrollment", e, enrollmentId);
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
                await ValidateRegisterEnrollmentRequest(studentId, request);
                var courseSemester = await uow.CourseSemester.GetCourseSemesterByIdAsync(request.CourseSemesterId);
                try
                {
                var enrollment = new Enrollment
                {
                    CourseSemesterId = request.CourseSemesterId,
                    StudentId = studentId,
                    Grade = new Grade
                    {
                        FirstGrade = null,
                        SecondGrade = null,
                        FinalGrade = null
                    }
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
                    await uow.Enrollment.RegisterEnrollmentAsync(enrollment);
                    await uow.SaveChangeAsync();
                    var newEnrollment = await uow.Enrollment.GetEnrollmentResponseByIdAsync(enrollment.EnrollmentId);
                    if(newEnrollment is null) throw new NotFoundException($"The enrollment with the Id {enrollment.EnrollmentId} was not found after creation !");
                    logger.LogEntityCreated("Enrollment", enrollment.EnrollmentId);
                    return newEnrollment;
                }catch(Exception e)
                {
                    logger.LogOperationError("RegisterEnrollment", e, studentId, request.CourseSemesterId);
                    throw;
                }
            }
        }

        private async Task ValidateRegisterEnrollmentRequest(int studentId, RegisterEnrollmentRequest request)
        {
            if (!await uow.User.IsStudentAsync(studentId)) throw new NotFoundException($"The student with the Id {studentId} was not found");
            if (!await uow.CourseSemester.ExistsAsync(u => u.CourseSemesterId == request.CourseSemesterId)) throw new NotFoundException($"The course semester with the Id {request.CourseSemesterId} was not found");
            if (!await uow.TeacherCourseSemester.ExistsAsync(u => u.CourseSemesterId == request.CourseSemesterId)) throw new BadRequestException("Cannot register enrollment which haven't been allocated by any teacher");
            if (await uow.Enrollment.ExistsAsync(p => p.StudentId == studentId && p.CourseSemesterId == request.CourseSemesterId)) throw new ConflictException($"The student with Id {studentId} is already enrolled in the Course with Id {request.CourseSemesterId}");
        }
    }
}
