using SchoolManagement.DTOs.Gpa;

namespace SchoolManagement.Services.Interfaces
{
    public interface IGpaService
    {
        Task<List<GpaResponse>> GetAllGpas();
    }
}
