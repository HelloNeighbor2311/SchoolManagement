using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.DTOs.Enrollment;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories
{
    public class EnrollmentRepository(AppDbContext context) : GenericRepository<Enrollment>(context), IEnrollmentRepository
    {
        public async Task DeleteEnrollmentAsync(Enrollment enrollment)
        {
            Context.Remove(enrollment);
        }

        public async Task<List<Enrollment>> GetAllEnrollmentInformationAsync()
        {
            var enrollments = await Context.Enrollments.Include(u => u.Student).
                Include(u => u.CourseSemester).ThenInclude(u => u!.Course).
                Include(u=>u.CourseSemester).ThenInclude(u=>u.Semester).ToListAsync();
            return enrollments;
        }

        public async Task<Enrollment?> GetEnrollmentByIdAsync(int id)
        {
            return await Context.Enrollments.Include(u => u.Student).
                Include(u => u.CourseSemester).ThenInclude(u => u!.Course).
                Include(u => u.CourseSemester).ThenInclude(u => u.Semester).FirstOrDefaultAsync(p=>p.EnrollmentId == id);
        }

        public async Task RegisterEnrollmentAsync(Enrollment request)
        {
            await Context.AddAsync(request);
        }
    }
}
