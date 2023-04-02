using Task9.Models;

namespace Task9.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Course> GetDetailsAsync(int id);
    }
}
