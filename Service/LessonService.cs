using Infrastructure.Interfaces;
using Infrastructure.Models;
using Service.CQ.Commands;


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
            return _lessonRepository.GetAllLessons();
        }

        public Lesson GetLessonById(int courseId, int id)
        {
            return _lessonRepository.GetLessonById(courseId, id);
        }
        
        
        public Lesson AddLesson(CreateLessonCommand command)
        {

            return _lessonRepository.AddLesson(command.Title, command.Content, command.CourseId, command.PictureUrls, command.VideoUrls);
        }

        public Lesson UpdateLesson(UpdateLessonCommand command)
        {
            return _lessonRepository.UpdateLesson(command.Id, command.Title, command.Content, command.CourseId, command.PictureUrls, command.VideoUrls);
        }

        public void DeleteLesson(int id)
        { 
            _lessonRepository.DeleteLesson(id);
        }
    }
}