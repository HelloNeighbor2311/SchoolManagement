using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SchoolManagement.Repositories
{
    public class CourseSemesterRepository(AppDbContext context) : GenericRepository<CourseSemester>(context), ICourseSemesterRepository
    {
        public async Task CreateCourseSemester(CourseSemester courseSemester)
        {
            await Context.AddAsync(courseSemester);
        }

        public async Task<CourseSemester?> GetCourseSemesterById(int id)
        {
            return await Context.CourseSemester.Include(u => u.Course).Include(u => u.Semester).FirstOrDefaultAsync(u => u.CourseSemesterId == id);
        }
    }
}
