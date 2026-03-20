using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SchoolManagement.Repositories
{
    public class CourseSemesterRepository(AppDbContext context) : GenericRepository<CourseSemester>(context), ICourseSemesterRepository
    {
        

        
    }
}
