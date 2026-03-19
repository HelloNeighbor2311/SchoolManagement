using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;
using System.Reflection;

namespace SchoolManagement.Datas
{
    public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Semester> Semester => Set<Semester>();
        public DbSet<TeacherCourseSemester> TeacherCourseSemester => Set<TeacherCourseSemester>();
 
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
