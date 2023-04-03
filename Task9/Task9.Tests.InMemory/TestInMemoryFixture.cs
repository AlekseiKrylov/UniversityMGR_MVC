using Microsoft.EntityFrameworkCore;
using Task9.Data;
using Task9.Models;

namespace Task9.Tests.InMemory
{
    public class TestInMemoryFixture : IAsyncLifetime
    {
        public DbContextOptions<Task9Context> TestContextOptions { get; private set; }
        public Task9Context TestContext { get; private set; }

        public async Task InitializeAsync()
        {
            var dbSuffix = Guid.NewGuid().ToString();
            TestContextOptions = new DbContextOptionsBuilder<Task9Context>()
                .UseInMemoryDatabase($"Task9Test_DB_{dbSuffix}")
                .Options;

            TestContext = new Task9Context(TestContextOptions);

            var course1 = new Course { Id = 1, Name = "First Course", Description = "This is the first Course", Groups = new List<Group>() };
            var course2 = new Course { Id = 2, Name = "Second Course", Description = "This is the second Course", Groups = new List<Group>() };

            var group1 = new Group { Id = 1, Name = "GR-01", CourseId = 1, Students = new List<Student>() };
            var group2 = new Group { Id = 2, Name = "GR-02", CourseId = 1, Students = new List<Student>() };
            var group3 = new Group { Id = 3, Name = "GR-03", CourseId = 1, Students = new List<Student>() };

            var student1 = new Student { Id = 1, FirstName = "Jerry", LastName = "Smith", GroupId = 3 };
            var student2 = new Student { Id = 2, FirstName = "Samanta", LastName = "Fox", GroupId = 3 };
            var student3 = new Student { Id = 3, FirstName = "Vernon", LastName = "Roshe", GroupId = 2 };
            var student4 = new Student { Id = 4, FirstName = "Wednesday", LastName = "Addams", GroupId = null };

            course1.Groups.Add(group1);
            course1.Groups.Add(group2);
            course1.Groups.Add(group3);

            group3.Students.Add(student1);
            group3.Students.Add(student2);
            group2.Students.Add(student3);

            TestContext.Courses.AddRange(course1, course2);
            await TestContext.SaveChangesAsync();
        }

        public Task DisposeAsync()
        {
            TestContext.Dispose();
            return Task.CompletedTask;
        }
    }
}
