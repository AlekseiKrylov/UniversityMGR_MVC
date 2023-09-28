using Microsoft.EntityFrameworkCore;
using UniversityMGR_MVC.Models;

namespace UniversityMGR_MVC.Data
{
    public class UniversityMGRContext : DbContext
    {
        protected UniversityMGRContext() { }
        public UniversityMGRContext(DbContextOptions<UniversityMGRContext> options) : base(options) { }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }
    }
}
