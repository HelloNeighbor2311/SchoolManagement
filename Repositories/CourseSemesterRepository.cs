using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.DTOs.CourseSemester;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SchoolManagement.Repositories
{
    public class CourseSemesterRepository(AppDbContext context, IMapper mapper) : GenericRepository<CourseSemester>(context), ICourseSemesterRepository
    {
        public async Task CreateCourseSemesterAsync(CourseSemester courseSemester)
        {
            await Context.AddAsync(courseSemester);
        }

        public async Task DeletetCourseSemesterAsync(CourseSemester courseSemester)
        {
            Context.CourseSemesters.Remove(courseSemester);
        }

        public async Task<List<CourseSemesterResponse>> GetAllCourseSemesters()
        {
            var response = await Context.CourseSemesters.ProjectTo<CourseSemesterResponse>(mapper.ConfigurationProvider).ToListAsync();
            return response;
        }

        public async Task<CourseSemester?> GetCourseSemesterByIdAsync(int courseSemesterId)
        {
            return await Context.CourseSemesters.Include(u => u.Course).Include(u => u.Semester).FirstOrDefaultAsync(u => u.CourseSemesterId == courseSemesterId);
        }

        public async Task<CourseSemesterResponse?> GetCourseSemesterResponseByIdAsync(int courseSemesterId)
        {
            return await Context.CourseSemesters.ProjectTo<CourseSemesterResponse>(mapper.ConfigurationProvider).FirstOrDefaultAsync(u => u.CourseSemesterId == courseSemesterId);
        }
    }
}
