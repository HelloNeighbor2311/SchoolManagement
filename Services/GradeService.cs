using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.DTOs.Grade;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class GradeService(IUnitOfWork uow, IMapper mapper, ILogger<GradeService> logger) : IGradeService
    {
        public async Task<List<GradeResponse>> GetGradeWithStudentId(int studentId)
        {
            using (logger.BeginOperationScope("GetGradeWithStudentId", ("StudentId", studentId)))
            using (var timer = logger.TimeOperation("GetGradeWithStudentId"))
            {
                var result = await uow.Grade.GetAllGradeWithStudentIdAsync(studentId);
                if (!result.Any())
                    throw new NotFoundException($"No grades found for student {studentId}");
                try
                {
                    var gradeResponse = result.Select(u => mapper.Map<GradeResponse>(u)).ToList();
                    return gradeResponse;
                }catch(Exception e)
                {
                    logger.LogOperationError("GetGradeWithStudentId", e);
                    throw;
                }
            }
        }

        public async Task UpdateGrade(int gradeId, UpdateGradeRequest request)
        {
            using (logger.BeginOperationScope("UpdateGrade", ("GradeId", gradeId)))
            using (var timer = logger.TimeOperation("UpdateGrade"))
            {
                var grade = await uow.Grade.GetGradeByIdAsync(gradeId);
                if (grade is null) throw new NotFoundException($"Grade with the Id {gradeId} was not found");
                var rowVersionBytes = Convert.FromBase64String(request.RowVersion);
                uow.Grade.SetRowVersion(grade, rowVersionBytes);
                if (request.FirstGrade.HasValue)
                    grade.FirstGrade = request.FirstGrade;

                if (request.SecondGrade.HasValue)
                    grade.SecondGrade = request.SecondGrade;
                grade.FinalGrade = CaculateFinalGrade(grade.FirstGrade, grade.SecondGrade);

                await uow.Grade.UpdateGradeAsync(grade);
                try
                {
                    await uow.SaveChangeAsync();
                    logger.LogEntityUpdated("Grade", gradeId);
                }
                catch (DbUpdateException)
                {
                    throw new ConflictException("Grade has been modified by someone. Please try again later");
                }

                var studentId = grade.Enrollment!.StudentId;
                var semesterId = grade.Enrollment.CourseSemester!.SemesterId;
                var studentGpa = await uow.Gpa.FindGpaViaStudentIdAndSemesterIdAsync(studentId, semesterId);
                if (await uow.Grade.isAllGradedAsync(studentId, semesterId))
                {
                    var group = await uow.Gpa.CaculateGpaAsync(studentId, semesterId);
                    studentGpa.gpa = group.gpa;
                    studentGpa.TotalCredits = group.totalCredit;
                    studentGpa.SetRank(studentGpa.gpa);
                    await uow.Gpa.UpdateGpaAsync(studentGpa);
                }
                try
                {
                    await uow.SaveChangeAsync();
                }catch(Exception e)
                {
                    logger.LogOperationError("UpdateGrade", e);
                    throw;
                }
            }
        }
        private double? CaculateFinalGrade(double? FirstGrade, double? SecondGrade)
        {
            if (FirstGrade is null || SecondGrade is null) return null;
            var final = (FirstGrade.Value * 0.4) + (SecondGrade.Value * 0.6);
            return Math.Round(final,2);
        }
    }
}
