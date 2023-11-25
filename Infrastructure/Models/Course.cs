namespace Infrastructure.Models;

public class Course
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string CourseImgUrl { get; set; }
    public ICollection<Lesson>? Lessons { get; set; }
}