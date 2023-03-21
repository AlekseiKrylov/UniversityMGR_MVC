using Microsoft.EntityFrameworkCore;
using Task9.Data;
using Task9.Models;
using Task9.Services.Interfaces;

namespace Task9.Services
{
    public class StudentService : ICRUDService<Student>, IStudentService
    {
        private readonly Task9Context _context;

        public StudentService(Task9Context context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllEntitiesAsync()
        {
            return await _context.Students.Include(s => s.Group)
                        .Include(s => s.Group.Course)
                            .ToListAsync();
        }

        public async Task<Student> GetEntityByIdAsync(int id)
        {
            return await _context.Students.Include(s => s.Group)
                        .Include(s => s.Group.Course)
                            .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Student student = await GetEntityByIdAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        public async Task<int> ExpelAsync(int id)
        {
            Student student = await _context.Students.FindAsync(id);
            int groupId = (int)student.GroupId;
            student.GroupId = null;
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return groupId;
        }
    }
}
