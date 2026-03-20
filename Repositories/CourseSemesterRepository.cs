using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories
{
    public class CourseSemesterRepository(AppDbContext context) : ICourseSemesterRepository
    {
        public async Task<bool> CheckValidCurrentId(int id)
        {
            return await context.CourseSemester.AnyAsync(u => u.CourseSemesterId == id);
        }

        
    }
}
