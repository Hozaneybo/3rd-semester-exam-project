using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllCourses();
    Task<Course> GetCourseById(int id);
    Task<Course> AddCourse(Course course);
    Task<Course> UpdateCourse(Course course);
    Task DeleteCourse(int id);
}