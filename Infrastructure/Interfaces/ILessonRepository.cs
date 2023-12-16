using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface ILessonRepository
{
    /// <summary>
    /// Retrieves all the lessons.
    /// </summary>
    /// <returns>An enumerable collection of Lesson objects.</returns>
    IEnumerable<Lesson> GetAllLessons();

    /// <summary>
    /// Retrieves a lesson by its course ID and ID.
    /// </summary>
    /// <param name="courseId">The ID of the course the lesson belongs to.</param>
    /// <param name="id">The ID of the lesson.</param>
    /// <returns>The lesson with the specified course ID and ID.</returns>
    Lesson GetLessonById(int courseId, int id);

    /// <summary>
    /// Adds a new lesson to the specified course.
    /// </summary>
    /// <param name="title">The title of the lesson.</param>
    /// <param name="content">The content of the lesson.</param>
    /// <param name="courseId">The ID of the course.</param>
    /// <param name="pictureUrls">A collection of URLs for pictures related to the lesson.</param>
    /// <param name="videoUrls">A collection of URLs for videos related to the lesson.</param>
    /// <returns>The newly created Lesson object.</returns>
    Lesson AddLesson(string title, string content, int courseId, IEnumerable<string> pictureUrls,
        IEnumerable<string> videoUrls);

    /// <summary>
    /// Updates the details of a lesson in the course.
    /// </summary>
    /// <param name="id">The ID of the lesson.</param>
    /// <param name="title">The title of the lesson.</param>
    /// <param name="content">The content of the lesson.</param>
    /// <param name="courseId">The ID of the course that the lesson belongs to.</param>
    /// <param name="pictureUrls">The URLs of the pictures associated with the lesson.</param>
    /// <param name="videoUrls">The URLs of the videos associated with the lesson.</param>
    /// <returns>The updated Lesson object.</returns>
    Lesson UpdateLesson(int id, string title, string content, int courseId, IEnumerable<string> pictureUrls,
        IEnumerable<string> videoUrls);

    /// <summary>
    /// Deletes a lesson with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the lesson to be deleted.</param>
    void DeleteLesson(int id);
}