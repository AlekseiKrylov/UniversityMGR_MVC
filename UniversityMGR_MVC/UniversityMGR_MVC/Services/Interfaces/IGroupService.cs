using UniversityMGR_MVC.Models;

namespace UniversityMGR_MVC.Services.Interfaces
{
    public interface IGroupService
    {
        Task<Group> GetDetailsAsync(int id);
        Task ExpelAllStudentsAsync(int id);
    }
}
