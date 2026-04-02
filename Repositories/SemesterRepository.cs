using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories
{
    public class SemesterRepository(AppDbContext context) : ISemesterRepository
    {
        public async Task<Semester> CreateSemesterAsync(Semester semester)
        {
            await context.Semesters.AddAsync(semester);
            return semester;
        }

        public async Task DeleteSemesterAsync(Semester semester)
        {
             context.Semesters.Remove(semester);
        }

        public async Task<List<Semester>> GetAllSemesterAsync()
        {
            return await context.Semesters.ToListAsync();
        }

        public async Task<Semester?> GetSemesterByIdAsync(int id)
        {
            var semester = await context.Semesters.FirstOrDefaultAsync(p => p.SemesterId == id);
            return semester;
        }

        public async Task<Semester?> GetSemesterDetailAsync(int id)
        {
            var semesterDetail = await context.Semesters.Include(u => u.CourseSemester).ThenInclude(u => u.Course).FirstOrDefaultAsync(s => s.SemesterId == id);
            return semesterDetail;
        }
    }
}
