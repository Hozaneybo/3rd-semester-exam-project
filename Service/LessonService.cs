using Infrastructure.Interfaces;
using Infrastructure.Models;



namespace Service
{
    public class LessonService
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        //This method will not be used for now
        public IEnumerable<Lesson> GetAllLessons()
        {
            return _lessonRepository.GetAllLessons().Result;
        }

        public Lesson GetLessonById(int id)
        {
            return _lessonRepository.GetLessonById(id).Result;
        }
        
        
        public Lesson AddLesson(string title, string content, int courseId,  IEnumerable<string> pictureUrls, IEnumerable<string> videoUrls)
        {
            /*var pictureUrls = lesson.ImgUrls?.Select(p => p.ImgUrl) ?? Enumerable.Empty<string>();
            var videoUrls = lesson.VideoUrls?.Select(v => v.VideoUrl) ?? Enumerable.Empty<string>();*/

            return _lessonRepository.AddLesson(title, content, courseId, pictureUrls, videoUrls).Result;
        }

        public Lesson UpdateLesson(int id, string title, string content, int courseId, IEnumerable<string> pictureUrls, IEnumerable<string> videoUrls)
        {
            return _lessonRepository.UpdateLesson(id, title, content, courseId, pictureUrls, videoUrls).Result;
        }

        public void DeleteLesson(int id)
        { 
            _lessonRepository.DeleteLesson(id);
        }
    }
}