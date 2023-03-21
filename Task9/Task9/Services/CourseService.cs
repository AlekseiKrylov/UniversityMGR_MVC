using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task9.Data;
using Task9.Models;
using Task9.Services.Interfaces;

namespace Task9.Services
{
    public class CourseService : ICRUDService<Course>
    {
        private readonly Task9Context _context;

        public CourseService(Task9Context context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllEntitiesAsync()
        {
            return await _context.Courses.Include(c => c.Groups).ToListAsync();
        }

        public async Task<Course> GetEntityByIdAsync(int id)
        {
            return await _context.Courses.Include(c => c.Groups).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Course course = await _context.Courses.Include(c => c.Groups).FirstAsync(c => c.Id == id);
            if (course.Groups.Count > 0)
                throw new DbUpdateException("You cannot delete a course with groups");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
