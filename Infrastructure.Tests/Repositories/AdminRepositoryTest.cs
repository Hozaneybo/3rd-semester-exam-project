using System;
using NUnit.Framework;
using Moq;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using System.Linq;
using System.Collections.Generic;

namespace Infrastructure.Tests.Repositories;

[TestFixture]
public class AdminRepositoryTest
{
    private Mock<IAdminRepository> _repositoryMock;
    private List<User> mockUsers = new List<User>()
    {
        new User()
        {
            Id = 1,
            AvatarUrl = "Ferhad Photo",
            Email = "ferhad@rashki.com",
            Fullname = "Ferhad Ahmad Rashki",
            Role = Role.Admin
        },
        new User()
        {
            Id = 2,
            AvatarUrl = "Hozan Photo",
            Email = "hozan@eybo.com",
            Fullname = "Hozan Eybo",
            Role = Role.Teacher
        }, 
        new User()
        {
            Id = 3,
            AvatarUrl = "Ahmad Photo",
            Email = "ahmad@bakran.com",
            Fullname = "Ahmad Amer Bakran",
            Role = Role.Student
        },
    };

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IAdminRepository>();
    }

    [Test]
    public void GetAll_WhenCalled_ReturnsAllUsers()
    {
        // Arrange
        var mockUsers = this.mockUsers;
        _repositoryMock.Setup(x => x.GetAll())
            .Returns(mockUsers);

        // Act
        var users = _repositoryMock.Object.GetAll();

        // Assert
        Assert.AreEqual(mockUsers.Count, users.Count());
    }

    [Test]
    public void UpdateUser_ValidData_UpdatesAndReturnsUser()
    {
        // Arrange
        var userToUpdate = new User {
            Id = 1,
            AvatarUrl = "Ferhad Photo",
            Email = "ferhad@rashki.com",
            Fullname = "Ferhad Ahmad Rashki",
            Role = Role.Admin };
        _repositoryMock.Setup(x => x.UpdateUser(userToUpdate.Id,
                userToUpdate.Fullname, userToUpdate.Email, userToUpdate.AvatarUrl, userToUpdate.Role))
            .Returns(userToUpdate);

        // Act
        var updatedUser = _repositoryMock.Object.UpdateUser(userToUpdate.Id,
            userToUpdate.Fullname, userToUpdate.Email, userToUpdate.AvatarUrl, userToUpdate.Role);

        // Assert
        Assert.AreEqual(1, updatedUser.Id);
        Assert.AreEqual("ferhad@rashki.com", updatedUser.Email);
        Assert.AreEqual("Ferhad Ahmad Rashki", updatedUser.Fullname);
        Assert.AreEqual(Role.Admin, updatedUser.Role);
        Assert.AreEqual("Ferhad Photo", updatedUser.AvatarUrl);
    }

    [Test]
    public void DeleteUser_ValidId_DeletesUser()
    {
        // Arrange
        _repositoryMock.Setup(x => x.DeleteUser(mockUsers[0].Id))
            .Verifiable();

        // Act
        _repositoryMock.Object.DeleteUser(mockUsers[0].Id);

        // Assert
        _repositoryMock.Verify(ds => ds.DeleteUser(mockUsers[0].Id), Times.Once());
    }
    
    [Test]
    public void GetAll_EmptyDatabase_ReturnsEmptyCollection()
    {
        // Arrange

        List<User> usersList = new List<User>();
        _repositoryMock.Setup(x => x.GetAll()).Returns(usersList);

        // Act
        var users = _repositoryMock.Object.GetAll();

        // Assert
        Assert.IsEmpty(users);
    }
    
}
