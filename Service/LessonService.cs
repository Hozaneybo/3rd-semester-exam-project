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
        
        
        /*public async Task<Lesson> AddLesson(LessonDto lessonDto)
        {
            var pictureUrls = lessonDto.ImgUrls?.Select(p => p.ImgUrl) ?? Enumerable.Empty<string>();
            var videoUrls = lessonDto.VideoUrls?.Select(v => v.VideoUrl) ?? Enumerable.Empty<string>();

            return await _lessonRepository.AddLesson(lessonDto.Title, lessonDto.Content, lessonDto.CourseId, pictureUrls, videoUrls);
        }*/

        
        
        public async Task<Lesson> AddLesson(Lesson lessonDto)
        {
            var pictureUrls = lessonDto.ImgUrls?.Select(p => p.ImgUrl) ?? Enumerable.Empty<string>();
            var videoUrls = lessonDto.VideoUrls?.Select(v => v.VideoUrl) ?? Enumerable.Empty<string>();

            return await _lessonRepository.AddLesson(lessonDto.Title, lessonDto.Content, lessonDto.CourseId, pictureUrls, videoUrls);
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