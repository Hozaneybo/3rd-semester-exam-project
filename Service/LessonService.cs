using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Service.CQ.Commands;


namespace Service
{
    public class LessonService
    {
        private readonly ILogger<LessonService> _logger;
        private readonly ILessonRepository _lessonRepository;
        private readonly ISharedRepository _sharedRepository; 

        public LessonService(ILogger<LessonService> logger, ILessonRepository lessonRepository, ISharedRepository sharedRepository)
        {
            _logger = logger;
            _lessonRepository = lessonRepository;
            _sharedRepository = sharedRepository;
        }

        //This method will not be used for now
        public IEnumerable<Lesson> GetAllLessons()
        {
            try
            {
                return _lessonRepository.GetAllLessons();
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving all lessons: {Message}", ex.Message);
                throw new Exception("An error occurred while retrieving lessons.");
            }
        }

        public Lesson GetLessonById(int courseId, int id)
        {
            try
            {
                return _lessonRepository.GetLessonById(courseId, id);
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving lesson by ID: {Message}", ex.Message);
                throw new Exception("An error occurred while retrieving the lesson.");
            }
        }
        
        public Lesson AddLesson(CreateLessonCommand command)
        {
            try
            {
                var lesson = _lessonRepository.AddLesson(command.Title, command.Content, command.CourseId, command.PictureUrls, command.VideoUrls);

                var studentEmails = GetStudentEmails();
                MailService.SendEmailToMultipleRecipients(studentEmails, $"New Lesson Created", $"A new lesson ('{command.Title}') has been added to the course.");

                return lesson;
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding lesson: {Message}", ex.Message);
                throw new Exception("An error occurred while adding the lesson.");
            }
        }

        private List<string> GetStudentEmails()
        {
            try
            {
                var students = _sharedRepository.GetUsersByRole(Role.Student);
                return students.Select(s => s.Email).ToList();
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving student emails: {Message}", ex.Message);
                throw new Exception("An error occurred while retrieving student emails.");
            }
        }

        public Lesson UpdateLesson(UpdateLessonCommand command)
        {
            try
            {
                return _lessonRepository.UpdateLesson(command.Id, command.Title, command.Content, command.CourseId, command.PictureUrls, command.VideoUrls);
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating lesson: {Message}", ex.Message);
                throw new Exception("An error occurred while updating the lesson.");
            }
        }

        public void DeleteLesson(int id)
        { 
            try
            {
                _lessonRepository.DeleteLesson(id);
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting lesson: {Message}", ex.Message);
                throw new Exception("An error occurred while deleting the lesson.");
            }
        }
    }
}