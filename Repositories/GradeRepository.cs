using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories
{
    public class GradeRepository(AppDbContext context) : IGradeRepository
    {
        public async Task<List<Grade>> GetAllGradeWithStudentIdAsync(int id)
        {
            var query = context.Grades.Include(u => u.Enrollment).ThenInclude(u=>u!.Student).AsQueryable();
            var result = await query.Where(u => u.Enrollment!.StudentId == id).ToListAsync();
            return result;
        }
    }
}
