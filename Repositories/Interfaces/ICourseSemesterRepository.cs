namespace SchoolManagement.Repositories.Interfaces
{
    public interface ICourseSemesterRepository
    {
        public Task<bool> CheckValidCurrentId(int id);
    }
}
