using Task9.Models;

namespace Task9.Services.Interfaces
{
    public interface IStudentService
    {
        Task<Student> GetDetailsAsync(int id);
        Task<int> ExpelAsync(int id);
    }
}
