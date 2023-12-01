namespace Infrastructure.Models;

public class Lesson
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public IEnumerable<LessonPicture>? ImgUrls { get; set; }
    public IEnumerable<LessonVideo>? VideoUrls { get; set; }
    public int CourseId { get; set; }
}