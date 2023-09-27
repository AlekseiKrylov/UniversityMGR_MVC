using Microsoft.EntityFrameworkCore;
using UniversityMGR_MVC.Data;
using UniversityMGR_MVC.Models;
using UniversityMGR_MVC.Services.Interfaces;

namespace UniversityMGR_MVC.Services
{
    public class StudentService : ICRUDService<Student>, IStudentService
    {
        private readonly Task9Context _context;

        public StudentService(Task9Context context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task CreateAsync(Student student)
        {
            if (student == null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(student)} is null.");

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            if (student == null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(student)} is null.");

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Student student = await _context.Students.FindAsync(id);

            if (student == null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(student)} Id={id} is null.");

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        public async Task<Student> GetDetailsAsync(int id)
        {
            return await _context.Students.Include(s => s.Group)
                        .Include(s => s.Group.Course)
                            .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> ExpelAsync(int id)
        {
            Student student = await _context.Students.FindAsync(id);

            if (student == null)
                throw new InvalidOperationException($"Exeption! The student with ID={id} was not found in the context.");

            if (student.GroupId == null)
                throw new ArgumentNullException($"Exeption! Student {student.FullName} Id={id} is not in a group.");

            int groupId = (int)student.GroupId;
            student.GroupId = null;
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return groupId;
        }
    }
}
