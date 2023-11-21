namespace Infrastructure.Models;

public class LessonVideo
{
    public int Id { get; set; }
    public required string VideoUrl { get; set; }
    public int LessonId { get; set; }
}