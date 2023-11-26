using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllCourses();
    Task<Course> GetCourseById(int id);
    Task<Course> AddCourse(string title, string description, string courseImgUrl);
    Task<Course> UpdateCourse(int id, string title, string description, string courseImgUrl);
    Task DeleteCourse(int id);
}