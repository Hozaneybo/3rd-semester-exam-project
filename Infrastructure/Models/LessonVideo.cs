namespace Infrastructure.Models;

public class LessonVideo
{
    public int Id { get; set; }
    public string VideoUrl { get; set; }
    public int LessonId { get; set; }
}