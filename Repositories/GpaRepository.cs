using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.DTOs.Gpa;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories
{
    public class GpaRepository(AppDbContext context, IMapper mapper) : GenericRepository<Gpa>(context), IGpaRepository
    {
        public async Task AddGpaAsync(Gpa gpa)
        {
            await Context.Gpas.AddAsync(gpa);
        }

        public async Task<Gpa> FindGpaViaStudentIdAndSemesterIdAsync(int studentId, int semesterId)
        {
            var gpa = await Context.Gpas.FirstOrDefaultAsync(u => u.StudentId == studentId && u.SemesterId == semesterId);
            return gpa;
        }

        public async Task<(double? gpa, int totalCredit)> CaculateGpaAsync(int studentId, int semesterId)
        {
            var group = await Context.Enrollments.Where(u => u.StudentId == studentId && u.CourseSemester!.SemesterId == semesterId && u.Grade != null && u.Grade.FinalGrade.HasValue).
                Select(g => new
                {
                    FinalGrade = g.Grade!.FinalGrade!.Value,
                    Credit = g.CourseSemester!.Course!.Credits
                }).GroupBy(_ => 1).
                Select(e => new
                {
                    Sum = e.Sum(x => x.FinalGrade * x.Credit),
                    TotalCredit = e.Sum(x => x.Credit)
                }).FirstOrDefaultAsync();
            if (group != null)
            {
                var totalGradeValue = Math.Round(group.Sum / group.TotalCredit, 2);
                var gpaValue = Math.Round(totalGradeValue / 10.0 * 4.0, 2);
                return (gpaValue, group.TotalCredit);
            }
            return (null, 0);
        }

        public async Task UpdateGpaAsync(Gpa gpa)
        {
             Context.Update(gpa);
        }

        public async Task<List<GpaResponse>> GetAllGpaAsync()
        {
            return await Context.Gpas.ProjectTo<GpaResponse>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<Gpa?> FindGpaViaIdAsync(int gpaId)
        {
            return await Context.Gpas.FirstOrDefaultAsync(u => u.GPAId == gpaId);
        }
    }
}
