using Infrastructure.Interfaces;
using Infrastructure.Models;

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
        return _courseRepository.GetAllCourses().Result;
    }

    public Course GetCourseById(int id)
    {
        return _courseRepository.GetCourseById(id).Result;
    }

    public Course CreateCourse(string title, string description, string courseImgUrl)
    {
        return _courseRepository.AddCourse(title, description, courseImgUrl).Result;
    }

    public Course UpdateCourse(int id, string title, string description, string courseImgUrl)
    {
        return _courseRepository.UpdateCourse(id, title, description, courseImgUrl).Result;
    }

    public void DeleteCourse(int id)
    {
        _courseRepository.DeleteCourse(id).GetAwaiter().GetResult();
    }
}