using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface ILessonRepository
{
    IEnumerable<Lesson> GetAllLessons();
    Lesson GetLessonById(int courseId, int id);
    Lesson AddLesson(string title, string content, int courseId,  IEnumerable<string> pictureUrls, IEnumerable<string> videoUrls);
    Lesson UpdateLesson(int id, string title, string content, int courseId, IEnumerable<string> pictureUrls, IEnumerable<string> videoUrls);
    void DeleteLesson(int id);
    
}