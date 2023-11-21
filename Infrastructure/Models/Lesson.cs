namespace Infrastructure.Models;

public class Lesson
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public ICollection<LessonPicture>? ImgUrls { get; set; }
    public ICollection<LessonVideo>? VideoUrls { get; set; }
    public int CourseId { get; set; }
}