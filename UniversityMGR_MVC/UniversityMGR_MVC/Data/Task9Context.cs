using Microsoft.EntityFrameworkCore;
using UniversityMGR_MVC.Models;

namespace UniversityMGR_MVC.Data
{
    public class Task9Context : DbContext
    {
        protected Task9Context() { }
        public Task9Context(DbContextOptions<Task9Context> options) : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }
    }
}
