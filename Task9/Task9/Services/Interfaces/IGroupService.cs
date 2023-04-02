using Task9.Models;

namespace Task9.Services.Interfaces
{
    public interface IGroupService
    {
        Task<Group> GetDetailsAsync(int id);
        Task ExpelAllStudentsAsync(int id);
    }
}
