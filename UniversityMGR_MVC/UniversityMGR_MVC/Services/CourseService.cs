using Microsoft.EntityFrameworkCore;
using UniversityMGR_MVC.Data;
using UniversityMGR_MVC.Models;
using UniversityMGR_MVC.Services.Interfaces;

namespace UniversityMGR_MVC.Services
{
    public class CourseService : ICRUDService<Course>, ICourseService
    {
        private readonly UniversityMGRContext _context;

        public CourseService(UniversityMGRContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task CreateAsync(Course course)
        {
            if (course == null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(course)} is null.");

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            if (course == null)
                throw new ArgumentNullException($"Exeption! Lost data. The object {nameof(course)} is null.");

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Course course = await _context.Courses.Include(c => c.Groups).FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                throw new InvalidOperationException($"Exeption! The course with ID={id} was not found in the context.");

            if (course.Groups.Count > 0)
                throw new DbUpdateException("You cannot delete a course with groups");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        public async Task<Course> GetDetailsAsync(int id)
        {
            return await _context.Courses.Include(c => c.Groups).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
