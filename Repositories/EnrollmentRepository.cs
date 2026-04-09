using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.DTOs.Enrollment;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories
{
    public class EnrollmentRepository(AppDbContext context, IMapper mapper) : GenericRepository<Enrollment>(context), IEnrollmentRepository
    {
        public async Task DeleteEnrollmentAsync(Enrollment enrollment)
        {
            Context.Remove(enrollment);
        }

        public async Task<List<EnrollmentResponse>> GetAllEnrollmentInformationAsync()
        {
            //var enrollments = await Context.Enrollments.Include(u => u.Student).
            //    Include(u => u.CourseSemester).ThenInclude(u => u!.Course).
            //    Include(u=>u.CourseSemester).ThenInclude(u=>u.Semester).ToListAsync();

            var enrollments = await Context.Enrollments.ProjectTo<EnrollmentResponse>(mapper.ConfigurationProvider).ToListAsync();
            return enrollments;
        }

        public async Task<Enrollment?> GetEnrollmentByIdAsync(int enrollmentId)
        {
            return await Context.Enrollments.Include(u => u.Student).
                Include(u => u.CourseSemester).ThenInclude(u => u!.Course).
                Include(u => u.CourseSemester).ThenInclude(u => u.Semester).FirstOrDefaultAsync(p=>p.EnrollmentId == enrollmentId);
        }

        public async Task RegisterEnrollmentAsync(Enrollment request)
        {
            await Context.Enrollments.AddAsync(request);
        }
    }
}
