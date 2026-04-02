using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SchoolManagement.Repositories
{
    public class CourseSemesterRepository(AppDbContext context) : GenericRepository<CourseSemester>(context), ICourseSemesterRepository
    {
        public async Task CreateCourseSemesterAsync(CourseSemester courseSemester)
        {
            await Context.AddAsync(courseSemester);
        }

        public async Task DeletetCourseSemesterAsync(CourseSemester courseSemester)
        {
            Context.CourseSemesters.Remove(courseSemester);
        }

        public async Task<CourseSemester?> GetCourseSemesterByIdAsync(int id)
        {
            return await Context.CourseSemesters.Include(u => u.Course).Include(u => u.Semester).FirstOrDefaultAsync(u => u.CourseSemesterId == id);
        }
    }
}
