using Infrastructure.Interfaces;
using Infrastructure.Models;
using Service.CQ.Commands;

namespace Service;

public class CourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public IEnumerable<Course> GetAllCourses()
    {
        return _courseRepository.GetAllCourses();
    }

    public Course GetCourseById(int id)
    {
        return _courseRepository.GetCourseById(id);
    }

    public Course CreateCourse(CreateCourseCommand command)
    {
        return _courseRepository.AddCourse(command.Title, command.Description, command.CourseImgUrl);
    }

    public Course UpdateCourse(UpdateCourseCommand command)
    {
        return _courseRepository.UpdateCourse(command.Id, command.Title, command.Description, command.CourseImgUrl);
    }

    public void DeleteCourse(int id)
    {
        _courseRepository.DeleteCourse(id);
    }
}