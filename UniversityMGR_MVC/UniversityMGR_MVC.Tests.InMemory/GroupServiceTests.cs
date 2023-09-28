using Microsoft.EntityFrameworkCore;
using UniversityMGR_MVC.Data;
using UniversityMGR_MVC.Services;

namespace UniversityMGR_MVC.Tests.InMemory
{
    public class GroupServiceTests : IClassFixture<TestInMemoryFixture>
    {
        private readonly TestInMemoryFixture _fixture;

        public GroupServiceTests(TestInMemoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DeleteAsync_NoStudentsExist_GroupDeleted()
        {
            using (var context = new UniversityMGRContext(_fixture.TestContextOptions))
            {
                var service = new GroupService(context);

                // Act
                await service.DeleteAsync(1);

                // Assert
                var deletedGroup = await context.Groups.FirstOrDefaultAsync(g => g.Id == 1);
                Assert.Null(deletedGroup);
            }
        }

        [Fact]
        public async Task DeleteAsync_GroupHasStudents_ThrowsException()
        {
            using (var context = new UniversityMGRContext(_fixture.TestContextOptions))
            {
                var controller = new GroupService(context);

                // Act
                var ex = await Assert.ThrowsAsync<DbUpdateException>(() => controller.DeleteAsync(2));

                // Assert
                Assert.Equal("You cannot delete a group with students", ex.Message);

                var group = await context.Groups.FirstOrDefaultAsync(g => g.Id == 2);
                Assert.NotNull(group);
            }
        }

        [Fact]
        public async Task DeleteAsync_GroupNotFound_ThrowsException()
        {
            using (var context = new UniversityMGRContext(_fixture.TestContextOptions))
            {
                var service = new GroupService(context);

                // Act
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(99));

                // Assert
                Assert.Equal($"Exeption! The group with ID=99 was not found in the context.", ex.Message);
            }
        }

        [Fact]
        public async Task ExpelAllStudentsAsync_StudentsExist_StudentsExpelled()
        {
            using (var context = new UniversityMGRContext(_fixture.TestContextOptions))
            {
                var service = new GroupService(context);

                // Act
                await service.ExpelAllStudentsAsync(3);

                // Assert
                var students = await context.Students.Where(s => s.GroupId == 3).ToListAsync();
                Assert.Empty(students);
            }
        }
    }
}