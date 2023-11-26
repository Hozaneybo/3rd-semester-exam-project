using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface ILessonRepository
{
    Task<IEnumerable<Lesson>> GetAllLessons();
    Task<Lesson> GetLessonById(int id);
    Task<Lesson> AddLesson(string title, string content, int courseId,  IEnumerable<string> pictureUrls, IEnumerable<string> videoUrls);
    Task<Lesson> UpdateLesson(int id, string title, string content, int courseId, IEnumerable<string> pictureUrls, IEnumerable<string> videoUrls);
    Task DeleteLesson(int id);
    
}