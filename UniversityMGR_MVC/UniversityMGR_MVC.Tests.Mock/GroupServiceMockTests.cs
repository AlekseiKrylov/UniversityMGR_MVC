﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.Moq;
using Moq;
using UniversityMGR_MVC.Data;
using UniversityMGR_MVC.Models;
using UniversityMGR_MVC.Services;

namespace UniversityMGR_MVC.Tests.Mock
{
    public class GroupServiceMockTests
    {
        private readonly Mock<UniversityMGRContext> _UniversityMGR_MVCContextMock;
        private readonly GroupService _groupService;

        public GroupServiceMockTests()
        {
            _UniversityMGR_MVCContextMock = new Mock<UniversityMGRContext>();
            _groupService = new GroupService(_UniversityMGR_MVCContextMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ModelIsNull_Exception()
        {
            //Arrange
            Group group = null;

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _groupService.CreateAsync(group));
        }

        [Fact]
        public async Task CreateAsync_ModelNotNull_GroupAdded()
        {
            //Arrenge
            var groupsList = GetFakeGroupList();
            var newGroup = new Group()
            {
                Id = groupsList.Count + 1,
                Name = "GR-04",
                CourseId = 1,
                Students = new List<Student>()
            };
            var dbSetMock = groupsList.AsQueryable().BuildMockDbSet();

            dbSetMock.Setup(m => m.AddAsync(It.IsAny<Group>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Group group, CancellationToken token) =>
                {
                    var entityEntry = new Mock<EntityEntry<Group>>();
                    entityEntry.Setup(e => e.Entity).Returns(group);
                    entityEntry.Setup(e => e.State).Returns(EntityState.Added);
                    return entityEntry.Object;
                })
                .Callback((Group group, CancellationToken token) => groupsList.Add(group));

            _UniversityMGR_MVCContextMock.Setup(c => c.Groups.Add(newGroup))
                .Returns(dbSetMock.Object.Add)
                .Callback((Group group) => groupsList.Add(group));

            //Act
            await _groupService.CreateAsync(newGroup);

            //Assert
            Assert.Contains(newGroup, groupsList);
            dbSetMock.Verify(m => m.Add(It.IsAny<Group>()), Times.Once());
            _UniversityMGR_MVCContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ModelIsNull_Exception()
        {
            //Arrange
            Group group = null;

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _groupService.UpdateAsync(group));
        }

        [Fact]
        public async Task UpdateAsync_ModelNotNull_StudentUpdated()
        {
            //Arrenge
            var groupsList = GetFakeGroupList();
            int newCourseId = 3;
            var updatedGroup = new Group()
            {
                Id = groupsList[0].Id,
                Name = groupsList[0].Name,
                CourseId = newCourseId
            };
            var dbSetMock = groupsList.AsQueryable().BuildMockDbSet();

            _UniversityMGR_MVCContextMock.Setup(c => c.Groups.Update(updatedGroup))
                .Returns(dbSetMock.Object.Update)
                .Callback((Group group) => groupsList[0] = group);

            //Act
            await _groupService.UpdateAsync(updatedGroup);

            //Assert
            Assert.Contains(updatedGroup, groupsList);
            dbSetMock.Verify(x => x.Update(updatedGroup), Times.Once);
            _UniversityMGR_MVCContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        private static List<Group> GetFakeGroupList()
        {
            return new List<Group>
            {
                new Group{ Id = 1, Name = "GR-01", CourseId = 1, Students = new List<Student>() },
                new Group{ Id = 2, Name = "GR-02", CourseId = 2, Students = new List<Student>() },
                new Group{ Id = 3, Name = "GR-03", CourseId = 1, Students = new List<Student>() },
            };
        }
    }
}
