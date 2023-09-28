using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.Moq;
using Moq;
using UniversityMGR_MVC.Data;
using UniversityMGR_MVC.Models;
using UniversityMGR_MVC.Services;

namespace UniversityMGR_MVC.Tests.Mock
{
    public class CourseServiceMockTests
    {
        private readonly CourseService _courseService;
        private readonly Mock<Task9Context> _task9ContextMock;

        public CourseServiceMockTests()
        {
            _task9ContextMock = new Mock<Task9Context>();
            _courseService = new CourseService(_task9ContextMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ModelIsNull_Exception()
        {
            //Arrange
            Course course = null;

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _courseService.CreateAsync(course));
        }

        [Fact]
        public async Task CreateAsync_ModelNotNull_CourseAdded()
        {
            //Arrenge
            var groupsList = GetFakeCourseList();
            var newCourse = new Course()
            {
                Id = groupsList.Count + 1,
                Name = "New Course",
                Description = "New course deskription",
                Groups = new List<Group>()
            };
            var dbSetMock = groupsList.AsQueryable().BuildMockDbSet();

            dbSetMock.Setup(m => m.AddAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Course course, CancellationToken token) =>
                {
                    var entityEntry = new Mock<EntityEntry<Course>>();
                    entityEntry.Setup(e => e.Entity).Returns(course);
                    entityEntry.Setup(e => e.State).Returns(EntityState.Added);
                    return entityEntry.Object;
                })
                .Callback((Course course, CancellationToken token) => groupsList.Add(course));

            _task9ContextMock.Setup(c => c.Courses.Add(newCourse))
                .Returns(dbSetMock.Object.Add)
                .Callback((Course course) => groupsList.Add(course));

            //Act
            await _courseService.CreateAsync(newCourse);

            //Assert
            Assert.Contains(newCourse, groupsList);
            dbSetMock.Verify(m => m.Add(It.IsAny<Course>()), Times.Once());
            _task9ContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ModelIsNull_Exception()
        {
            //Arrange
            Course course = null;

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _courseService.UpdateAsync(course));
        }

        [Fact]
        public async Task UpdateAsync_ModelNotNull_CourseUpdated()
        {
            //Arrenge
            var coursesList = GetFakeCourseList();
            string newDescription = "Updated description";
            var updatedCourse = new Course()
            {
                Id = coursesList[0].Id,
                Name = coursesList[0].Name,
                Description = newDescription
            };
            var dbSetMock = coursesList.AsQueryable().BuildMockDbSet();

            _task9ContextMock.Setup(c => c.Courses.Update(updatedCourse))
                .Returns(dbSetMock.Object.Update)
                .Callback((Course course) => coursesList[0] = course);

            //Act
            await _courseService.UpdateAsync(updatedCourse);

            //Assert
            Assert.Contains(updatedCourse, coursesList);
            dbSetMock.Verify(x => x.Update(updatedCourse), Times.Once);
            _task9ContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        private static List<Course> GetFakeCourseList()
        {
            return new List<Course>
            {
                new Course{ Id = 1, Name = "First Course", Description = "Description first course", Groups = new List<Group>() },
                new Course{ Id = 2, Name = "Second Course", Description = "Description second course", Groups = new List<Group>() },
            };
        }
    }
}