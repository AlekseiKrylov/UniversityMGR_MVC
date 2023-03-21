using Microsoft.EntityFrameworkCore;
using Task9.Models;

namespace Task9.Data
{
    public class Task9Context : DbContext
    {
        public Task9Context(DbContextOptions<Task9Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
