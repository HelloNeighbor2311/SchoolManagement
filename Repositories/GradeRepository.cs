using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.DTOs.Grade;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories
{
    public class GradeRepository(AppDbContext context, IMapper mapper) : IGradeRepository
    {
        public async Task<List<GradeResponse>> GetAllGradeWithStudentIdAsync(int id)
        {
            var result = await context.Grades.Where(u=>u.Enrollment!.StudentId == id).ProjectTo<GradeResponse>(mapper.ConfigurationProvider).ToListAsync();
            return result;
        }

        public async Task<Grade?> GetGradeByIdAsync(int id)
        {
            return await context.Grades.Include(u=>u.Enrollment).ThenInclude(u=>u.CourseSemester).FirstOrDefaultAsync(u => u.GradeId == id);
        }

        public async Task<bool> isAllGradedAsync(int studentId, int semesterId)
        {
            var checkedGrades = await context.Enrollments.CountAsync(u => u.StudentId == studentId && u.CourseSemester!.SemesterId == semesterId && (u.Grade == null || !u.Grade!.FinalGrade.HasValue));
            return checkedGrades == 0;
        }

        public void SetRowVersion(Grade grade, byte[] rowVersion)
        {
            context.Entry(grade).Property(u => u.RowVersion).OriginalValue = rowVersion;
        }

        public async Task UpdateGradeAsync(Grade grade)
        {
            context.Grades.Update(grade);
        }
    }
}
