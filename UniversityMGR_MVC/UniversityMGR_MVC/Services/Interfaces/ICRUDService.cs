namespace UniversityMGR_MVC.Services.Interfaces
{
    public interface ICRUDService<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T item);
        Task UpdateAsync(T course);
        Task DeleteAsync(int id);
    }
}
