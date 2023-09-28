using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.Moq;
using Moq;
using UniversityMGR_MVC.Data;
using UniversityMGR_MVC.Models;
using UniversityMGR_MVC.Services;

namespace UniversityMGR_MVC.Tests.Mock
{
    public class StudentServiceMockTests
    {
        private readonly Mock<Task9Context> _task9ContextMock;
        private readonly StudentService _studentService;

        public StudentServiceMockTests()
        {
            _task9ContextMock = new Mock<Task9Context>();
            _studentService = new StudentService(_task9ContextMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ModelIsNull_Exception()
        {
            //Arrange
            Student student = null;

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _studentService.CreateAsync(student));
        }

        [Fact]
        public async Task CreateAsync_ModelNotNull_StudentAdded()
        {
            //Arrenge
            var studentsList = GetFakeStudentList();
            var newStudent = new Student() { Id = studentsList.Count + 1, FirstName = "Selene", LastName = null, GroupId = null };
            var dbSetMock = studentsList.AsQueryable().BuildMockDbSet();

            dbSetMock.Setup(m => m.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Student student, CancellationToken token) =>
                {
                    var entityEntry = new Mock<EntityEntry<Student>>();
                    entityEntry.Setup(e => e.Entity).Returns(student);
                    entityEntry.Setup(e => e.State).Returns(EntityState.Added);
                    return entityEntry.Object;
                })
                .Callback((Student student, CancellationToken token) => studentsList.Add(student));

            _task9ContextMock.Setup(c => c.Students.Add(newStudent))
                .Returns(dbSetMock.Object.Add)
                .Callback((Student student) => studentsList.Add(student));

            //Act
            await _studentService.CreateAsync(newStudent);

            //Assert
            Assert.Contains(newStudent, studentsList);
            dbSetMock.Verify(m => m.Add(It.IsAny<Student>()), Times.Once());
            _task9ContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ModelIsNull_Exception()
        {
            //Arrange
            Student student = null;

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _studentService.UpdateAsync(student));
        }

        [Fact]
        public async Task UpdateAsync_ModelNotNull_StudentUpdated()
        {
            //Arrenge
            var studentsList = GetFakeStudentList();
            string newLastName = "Walkman";
            var updatedStudent = new Student()
            {
                Id = studentsList[0].Id,
                FirstName = studentsList[0].FirstName,
                LastName = newLastName,
                GroupId = studentsList[0].GroupId
            };
            var dbSetMock = studentsList.AsQueryable().BuildMockDbSet();

            _task9ContextMock.Setup(c => c.Students.Update(updatedStudent))
                .Returns(dbSetMock.Object.Update)
                .Callback((Student student) => studentsList[0] = student);

            //Act
            await _studentService.UpdateAsync(updatedStudent);

            //Assert
            Assert.Contains(updatedStudent, studentsList);
            dbSetMock.Verify(x => x.Update(updatedStudent), Times.Once);
            _task9ContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ModelIsNull_Exception()
        {
            //Arrange
            int studentId = 99;
            _task9ContextMock.Setup(x => x.Students.FindAsync(studentId)).Returns(null);

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _studentService.DeleteAsync(studentId));
        }

        [Fact]
        public async Task DeleteAsync_ModelNotNull_StudentDeleted()
        {
            //Arrenge
            var studentsList = GetFakeStudentList();
            var deletedStudent = studentsList[0];
            int studentId = deletedStudent.Id;
            var dbSetMock = studentsList.AsQueryable().BuildMockDbSet();

            _task9ContextMock.Setup(c => c.Students.FindAsync(studentId))
                .ReturnsAsync(studentsList.Find(s => s.Id == studentId));
            _task9ContextMock.Setup(c => c.Students.Remove(deletedStudent))
                .Returns(dbSetMock.Object.Remove)
                .Callback((Student student) => studentsList.Remove(student));

            //Act
            await _studentService.DeleteAsync(studentId);

            //Assert
            Assert.DoesNotContain(deletedStudent, studentsList);
            dbSetMock.Verify(x => x.Remove(deletedStudent), Times.Once);
            _task9ContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExpelAsync_ModelIsNull_Exception()
        {
            //Arrange
            int studentId = 99;
            _task9ContextMock.Setup(x => x.Students.FindAsync(studentId)).Returns(null);

            //Act + Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _studentService.ExpelAsync(studentId));
        }

        [Fact]
        public async Task ExpelAsync_GroupIdIsNull_Exception()
        {
            //Arrange
            List<Student> studentList = GetFakeStudentList();
            Student student = studentList[3];
            _task9ContextMock.Setup(x => x.Students.FindAsync(student.Id)).ReturnsAsync(student);

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _studentService.ExpelAsync(student.Id));
        }

        [Fact]
        public async Task ExpelAsync_ModelisValid_StudentExpelled()
        {
            //Arrange
            var studentsList = GetFakeStudentList();
            var expelledStudent = studentsList[1];
            int groupId = (int)expelledStudent.GroupId;
            var dbSetMock = studentsList.AsQueryable().BuildMockDbSet();

            _task9ContextMock.Setup(c => c.Students.FindAsync(expelledStudent.Id))
                .ReturnsAsync(studentsList.Find(s => s.Id == expelledStudent.Id));
            _task9ContextMock.Setup(c => c.Students.Update(expelledStudent))
                .Returns(dbSetMock.Object.Update)
                .Callback((Student student) => studentsList[1].GroupId = null);

            //Act
            int result = await _studentService.ExpelAsync(expelledStudent.Id);

            //Assert
            Assert.Equal(groupId, result);
            Assert.True(studentsList[1].GroupId is null);
            Assert.True(studentsList[0].GroupId is not null);
            dbSetMock.Verify(x => x.Update(expelledStudent), Times.Once);
            _task9ContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        private static List<Student> GetFakeStudentList()
        {
            return new List<Student>
            {
                new Student{ Id = 1, FirstName = "John", LastName = "Smith", GroupId = 1 },
                new Student{ Id = 2, FirstName = "Sam", LastName = "Fisher", GroupId = 1 },
                new Student{ Id = 3, FirstName = "Peter", LastName = "Parker", GroupId = 2 },
                new Student{ Id = 4, FirstName = "Wednesday", LastName = "Addams", GroupId = null },
            };
        }
    }
}