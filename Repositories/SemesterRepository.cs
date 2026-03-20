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
            await context.Semester.AddAsync(semester);
            return semester;
        }

        public async Task DeleteSemesterAsync(Semester semester)
        {
             context.Semester.Remove(semester);
        }

        public async Task<List<Semester>> GetAllSemesterAsync()
        {
            return await context.Semester.ToListAsync();
        }

        public async Task<Semester?> GetSemesterByIdAsync(int id)
        {
            var semester = await context.Semester.FirstOrDefaultAsync(p => p.SemesterId == id);
            return semester;
        }

        public async Task<Semester?> GetSemesterDetailAsync(int id)
        {
            var semesterDetail = await context.Semester.Include(u => u.CourseSemester).ThenInclude(u => u.Course).FirstOrDefaultAsync(s => s.SemesterId == id);
            return semesterDetail;
        }
    }
}
