using Microsoft.EntityFrameworkCore;
using Task9.Data;
using Task9.Models;
using Task9.Services.Interfaces;

namespace Task9.Services
{
    public class GroupService : ICRUDService<Group>, IGroupService
    {
        private readonly Task9Context _context;

        public GroupService(Task9Context context)
        {
            _context = context;
        }

        public async Task<List<Group>> GetAllEntitiesAsync()
        {
            return await _context.Groups.Include(g => g.Course)
                        .Include(g => g.Students)
                            .ToListAsync();
        }

        public async Task<Group> GetEntityByIdAsync(int id)
        {
            return await _context.Groups.Include(g => g.Course)
                        .Include(g => g.Students)
                            .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task CreateAsync(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Group course)
        {
            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Group group = await _context.Groups.Include(g => g.Students).FirstAsync(g => g.Id == id);
            if (group.Students.Count > 0)
                throw new DbUpdateException("You cannot delete a group with students");

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }

        public async Task ExpelAllStudentsAsync(int id)
        {
            List<Student> students = await _context.Students.Where(s => s.GroupId == id).ToListAsync();
            if (students.Count > 0)
                foreach (var student in students)
                {
                    student.GroupId = null;
                    _context.Entry(student).State = EntityState.Modified;
                }
            await _context.SaveChangesAsync();

        }
    }
}
