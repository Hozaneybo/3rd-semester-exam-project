using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Service.CQ.Commands;

namespace Service;

public class CourseService
{
    private readonly ILogger<CourseService> _logger;
    private readonly ICourseRepository _courseRepository;

    public CourseService(ILogger<CourseService> logger, ICourseRepository courseRepository)
    {
        _logger = logger;
        _courseRepository = courseRepository;
    }

    public IEnumerable<Course> GetAllCourses()
    {
        try
        {
            return _courseRepository.GetAllCourses();
        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving all courses: {Message}", ex.Message);
            throw new Exception("An error occurred while retrieving courses.");
        }
    }

    public Course GetCourseById(int id)
    {
        try
        {
            return _courseRepository.GetCourseById(id);
        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving course by ID: {Message}", ex.Message);
            throw new Exception("An error occurred while retrieving the course.");
        }
    }

    public Course CreateCourse(CreateCourseCommand command)
    {
        try
        {
            return _courseRepository.AddCourse(command.Title, command.Description, command.CourseImgUrl);
        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating course: {Message}", ex.Message);
            throw new Exception("An error occurred while creating the course.");
        }
    }

    public Course UpdateCourse(UpdateCourseCommand command)
    {
        try
        {
            return _courseRepository.UpdateCourse(command.Id, command.Title, command.Description, command.CourseImgUrl);
        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating course: {Message}", ex.Message);
            throw new Exception("An error occurred while updating the course.");
        }
    }

    public void DeleteCourse(int id)
    {
        try
        {
            _courseRepository.DeleteCourse(id);
        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting course: {Message}", ex.Message);
            throw new Exception("An error occurred while deleting the course.");
        }
    }
}