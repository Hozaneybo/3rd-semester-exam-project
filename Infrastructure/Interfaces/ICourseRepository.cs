using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface ICourseRepository
{
    /// <summary>
    /// Retrieves all courses from the database.
    /// </summary>
    /// <returns>An enumerable collection of Course objects.</returns>
    IEnumerable<Course> GetAllCourses();

    /// <summary>
    /// Get a course by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the course.</param>
    /// <returns>The course with the specified ID.</returns>
    Course GetCourseById(int id);

    /// <summary>
    /// Adds a new course to the system.
    /// </summary>
    /// <param name="title">The title of the course.</param>
    /// <param name="description">The description of the course.</param>
    /// <param name="courseImgUrl">The URL of the course image.</param>
    /// <returns>The newly added Course object.</returns>
    Course AddCourse(string title, string description, string courseImgUrl);

    /// <summary>
    /// Updates the details of a course.
    /// </summary>
    /// <param name="id">The ID of the course to update.</param>
    /// <param name="title">The new title of the course.</param>
    /// <param name="description">The new description of the course.</param>
    /// <param name="courseImgUrl">The new image URL of the course.</param>
    /// <returns>The updated Course object.</returns>
    Course UpdateCourse(int id, string title, string description, string courseImgUrl);

    /// <summary>
    /// Deletes a course from the system.
    /// </summary>
    /// <param name="id">The ID of the course to be deleted.</param>
    void DeleteCourse(int id);
}