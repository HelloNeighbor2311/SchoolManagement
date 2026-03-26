using AutoMapper;
using SchoolManagement.DTOs.Grade;
using SchoolManagement.Exceptions;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class GradeService(IUnitOfWork uow, IMapper mapper) : IGradeService
    {
        public async Task<List<GradeResponse>> GetGradeWithStudentId(int studentId)
        {
            var result = await uow.Grade.GetAllGradeWithStudentIdAsync(studentId);
            if (!result.Any())
                throw new NotFoundException($"No grades found for student {studentId}");
            var gradeResponse = result.Select(u => mapper.Map<GradeResponse>(u)).ToList();
            return gradeResponse;
        }

        public async Task UpdateGrade(int id, UpdateGradeRequest request)
        {
            var grade = await uow.Grade.GetGradeByIdAsync(id);
            if (grade is null) throw new NotFoundException($"Grade with the Id {id} was not found");
            if (request.FirstGrade.HasValue)
                grade.FirstGrade = request.FirstGrade;

            if (request.SecondGrade.HasValue)
                grade.SecondGrade = request.SecondGrade;
            grade.FinalGrade = CaculateFinalGrade(grade.FirstGrade, grade.SecondGrade);

            await uow.Grade.UpdateGradeAsync(grade);
            await uow.SaveChangeAsync();

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
            await uow.SaveChangeAsync();
        }
        private double? CaculateFinalGrade(double? FirstGrade, double? SecondGrade)
        {
            if (FirstGrade is null || SecondGrade is null) return null;
            var final = (FirstGrade.Value * 0.4) + (SecondGrade.Value * 0.6);
            return Math.Round(final,2);
        }
    }
}
