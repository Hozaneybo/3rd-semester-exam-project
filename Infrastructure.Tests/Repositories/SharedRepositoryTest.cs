using NUnit.Framework;
using Moq;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using System.Linq;
using System.Collections.Generic;

namespace Infrastructure.Tests.Repositories;

[TestFixture]
public class SharedRepositoryTest
{
    private Mock<ISharedRepository> _repositoryMock;

    [SetUp]
    public void SetUp()
    {
        // setting up objects for dependency injection 
        _repositoryMock = new Mock<ISharedRepository>();
    }

    [Test]
    public void Search_ValidString_ReturnsSearchResult()
    {
        // Arrange 
        var mockSearchResults = new List<SearchResult>()
        {
            new SearchResult("User", "John Doe"),
            new SearchResult("Lesson", "Java Script")
        };
        string searchTerm = "Jav";
        _repositoryMock.Setup(x => x.Search(searchTerm))
            .Returns(mockSearchResults.Where(s => s.Term.Contains(searchTerm)));

        // Act 
        var result = _repositoryMock.Object.Search(searchTerm);

        // Assert 
        Assert.IsTrue(result.Any());
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("Lesson", result.First().Type);
        Assert.AreEqual("Java Script", result.First().Term);
    }
    
    
    [Test]
    public void Search_EmptyString_ReturnsNoResult()
    {
        // Arrange
        var mockSearchResults = new List<SearchResult>
        {
            new SearchResult(type: "User", term: "John Doe"),
            new SearchResult(type: "Lesson", term: "JavaScript"),
        };
        string searchTerm = "";
        _repositoryMock.Setup(x => x.Search(searchTerm))
            .Returns(mockSearchResults.Where(s => !string.IsNullOrEmpty(searchTerm) && s.Term.Contains(searchTerm)));

        // Act
        var result = _repositoryMock.Object.Search(searchTerm);

        // Assert
        Assert.AreEqual(0, result.Count());
        Assert.IsFalse(result.Any());
    }


    
    
    [TestCase(Role.Admin)]
    [TestCase(Role.Teacher)]
    [TestCase(Role.Student)]
    public void GetUsersByRole_ValidRole_ReturnsUsers(Role role)
    {
        var mockUsers = new List<User>()
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
        // Arrange
        _repositoryMock.Setup(x => x.GetUsersByRole(role)).Returns(mockUsers.Where(u => u.Role == role));

        // Act 
        var result = _repositoryMock.Object.GetUsersByRole(role).ToList();

        // Assert 
        Assert.IsTrue(result.Any());
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(role, result.First().Role);
        var expectedUser = mockUsers.First(u => u.Role == role);
        Assert.AreEqual(expectedUser.Fullname, result.First().Fullname);
        Assert.AreEqual(expectedUser.Email, result.First().Email);
        Assert.AreEqual(expectedUser.Id, result.First().Id);
    }


    /*[Test]
    public void GetUsersByRole_InvalidRole_ThrowsArgumentException()
    {
        // Arrange
        Role role = (Role)1; // Invalid Enum value
        _repositoryMock.Setup(x => x.GetUsersByRole(role)).Throws(new ArgumentException("Invalid role specified."));

        // Act & Assert 
        Assert.Throws<ArgumentException>(() => _repositoryMock.Object.GetUsersByRole(role));
    }*/

}