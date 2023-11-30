using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface ICourseRepository
{
    IEnumerable<Course> GetAllCourses();
    Course GetCourseById(int id);
    Course AddCourse(string title, string description, string courseImgUrl);
    Course UpdateCourse(int id, string title, string description, string courseImgUrl);
    void DeleteCourse(int id);
}