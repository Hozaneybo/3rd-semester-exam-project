using Infrastructure.Interfaces;
using Infrastructure.Models;
using System.Collections.Generic;

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

    public Course CreateCourse(Course course)
    {
        return _courseRepository.AddCourse(course).Result;
    }

    public Course UpdateCourse(Course course)
    {
        return _courseRepository.UpdateCourse(course).Result;
    }

    public void DeleteCourse(int id)
    {
        _courseRepository.DeleteCourse(id).GetAwaiter().GetResult();
    }
}