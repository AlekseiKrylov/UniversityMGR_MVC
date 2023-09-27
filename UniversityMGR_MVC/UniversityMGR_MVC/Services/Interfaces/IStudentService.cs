using UniversityMGR_MVC.Models;

namespace UniversityMGR_MVC.Services.Interfaces
{
    public interface IStudentService
    {
        Task<Student> GetDetailsAsync(int id);
        Task<int> ExpelAsync(int id);
    }
}
