using Microsoft.EntityFrameworkCore;
using UniversityMGR_MVC.Data;
using UniversityMGR_MVC.Services;

namespace UniversityMGR_MVC.Tests.InMemory
{
    public class CourseServiceTests : IClassFixture<TestInMemoryFixture>
    {
        private readonly TestInMemoryFixture _fixture;

        public CourseServiceTests(TestInMemoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DeleteAsync_NoStudentsExist_CourseDeleted()
        {
            using (var context = new UniversityMGRContext(_fixture.TestContextOptions))
            {
                var service = new CourseService(context);

                // Act
                await service.DeleteAsync(2);

                // Assert
                var deletedCourse = await context.Courses.FirstOrDefaultAsync(g => g.Id == 2);
                Assert.Null(deletedCourse);
            }
        }

        [Fact]
        public async Task DeleteAsync_CourseHasStudents_ThrowsException()
        {
            using (var context = new UniversityMGRContext(_fixture.TestContextOptions))
            {
                var controller = new CourseService(context);

                // Act
                var ex = await Assert.ThrowsAsync<DbUpdateException>(() => controller.DeleteAsync(1));

                // Assert
                Assert.Equal("You cannot delete a course with groups", ex.Message);

                var group = await context.Courses.FirstOrDefaultAsync(g => g.Id == 1);
                Assert.NotNull(group);
            }
        }

        [Fact]
        public async Task DeleteAsync_CourseNotFound_ThrowsException()
        {
            using (var context = new UniversityMGRContext(_fixture.TestContextOptions))
            {
                var service = new CourseService(context);

                // Act
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(99));

                // Assert
                Assert.Equal($"Exeption! The course with ID=99 was not found in the context.", ex.Message);
            }
        }
    }
}