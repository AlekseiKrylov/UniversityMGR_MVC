namespace Task9.Services.Interfaces
{
    public interface ICRUDService<T> where T : class
    {
        Task<List<T>> GetAllEntitiesAsync();
        Task<T> GetEntityByIdAsync(int id);
        Task CreateAsync(T item);
        Task UpdateAsync(T course);
        Task DeleteAsync(int id);
    }
}
