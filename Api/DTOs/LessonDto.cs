using System.ComponentModel.DataAnnotations;
using _3rd_semester_exam_project.Filters;

namespace _3rd_semester_exam_project.DTOs;

[RequireAuthentication]
public class LessonDto
{
    public int Id { get; set; }
    [MinLength(3), MaxLength(25)]
    public required string Title { get; set; }
    public required string Content { get; set; }
    public ICollection<LessonPictureDto>? ImgUrls { get; set; }
    public ICollection<LessonVideoDto>? VideoUrls { get; set; }
    public int CourseId { get; set; }
}