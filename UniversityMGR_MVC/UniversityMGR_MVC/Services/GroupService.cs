using Microsoft.EntityFrameworkCore;
using UniversityMGR_MVC.Data;
using UniversityMGR_MVC.Models;
using UniversityMGR_MVC.Services.Interfaces;

namespace UniversityMGR_MVC.Services
{
    public class GroupService : ICRUDService<Group>, IGroupService
    {
        private readonly UniversityMGRContext _context;

        public GroupService(UniversityMGRContext context)
        {
            _context = context;
        }

        public async Task<List<Group>> GetAllAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<Group> GetByIdAsync(int id)
        {
            return await _context.Groups.FindAsync(id);
        }

        public async Task CreateAsync(Group group)
        {
            if (group == null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(group)} is null.");

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Group group)
        {
            if (group == null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(group)} is null.");

            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Group group = await _context.Groups.Include(g => g.Students)
                            .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
                throw new InvalidOperationException($"Exeption! The group with ID={id} was not found in the context.");

            if (group.Students.Count > 0)
                throw new DbUpdateException("You cannot delete a group with students");

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }

        public async Task<Group> GetDetailsAsync(int id)
        {
            return await _context.Groups.Include(g => g.Course)
                        .Include(g => g.Students)
                            .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task ExpelAllStudentsAsync(int id)
        {
            var students = await _context.Students.Where(s => s.GroupId == id).ToListAsync();

            if (students.Count == 0)
                return;

            foreach (var student in students)
            {
                student.GroupId = null;
                _context.Students.Update(student);
            }
            await _context.SaveChangesAsync();
        }
    }
}
