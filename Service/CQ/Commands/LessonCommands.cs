using System.ComponentModel.DataAnnotations;

namespace Service.CQ.Commands;

public class CreateLessonCommand
{
    [Required(ErrorMessage = "Title is required."), MinLength(3, ErrorMessage = "Title must be at least 3 characters long."), MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Content is required.")]
    public string Content { get; set; }
    [Required(ErrorMessage = "Course ID is required.")]
    public int CourseId { get; set; }
    public IEnumerable<string> PictureUrls { get; set; }
    public IEnumerable<string> VideoUrls { get; set; }
}


public class UpdateLessonCommand
{
    [Required(ErrorMessage = "Lesson ID is required.")]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int CourseId { get; set; }
    public IEnumerable<string> PictureUrls { get; set; }
    public IEnumerable<string> VideoUrls { get; set; }
}

public class DeleteLessonCommand
{
    [Required(ErrorMessage = "Lesson ID is required.")]
    public int Id { get; set; }
}
