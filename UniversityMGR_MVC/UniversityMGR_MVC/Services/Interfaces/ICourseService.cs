using UniversityMGR_MVC.Models;

namespace UniversityMGR_MVC.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Course> GetDetailsAsync(int id);
    }
}
