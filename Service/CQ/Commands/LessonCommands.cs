using System.ComponentModel.DataAnnotations;

namespace Service.CQ.Commands;

public class CreateLessonCommand
{
    [Required, MinLength(3), MaxLength(50)]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    public int CourseId { get; set; }
    public IEnumerable<string> PictureUrls { get; set; }
    public IEnumerable<string> VideoUrls { get; set; }
}


public class UpdateLessonCommand
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int CourseId { get; set; }
    public IEnumerable<string> PictureUrls { get; set; }
    public IEnumerable<string> VideoUrls { get; set; }
}

public class DeleteLessonCommand
{
    public int Id { get; set; }
}
