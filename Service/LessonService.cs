using Infrastructure.Interfaces;
using Infrastructure.Models;
using Service.CQ.Commands;


namespace Service
{
    public class LessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ISharedRepository _sharedRepository; 

        public LessonService(ILessonRepository lessonRepository, ISharedRepository sharedRepository)
        {
            _lessonRepository = lessonRepository;
            _sharedRepository = sharedRepository;
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

            var lesson = _lessonRepository.AddLesson(command.Title, command.Content, command.CourseId, command.PictureUrls, command.VideoUrls);

            
            var studentEmails = GetStudentEmails();
            MailService.SendEmailToMultipleRecipients(studentEmails, $"New Lesson Created", $"A new lesson ('{command.Title}') has been added to the course.");

            return lesson;
        }
        private List<string> GetStudentEmails()
        {
            var students = _sharedRepository.GetUsersByRole(Role.Student);
            return students.Select(s => s.Email).ToList();
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