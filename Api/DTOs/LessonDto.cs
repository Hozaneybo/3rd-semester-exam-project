using System.ComponentModel.DataAnnotations;
using _3rd_semester_exam_project.Filters;

namespace _3rd_semester_exam_project.DTOs;

[RequireAuthentication]
public class LessonDto
{
    
    [MinLength(3), MaxLength(25)]
    public required string Title { get; set; }
    public required string Content { get; set; }
    public int CourseId { get; set; }
    public IEnumerable<LessonPictureDto>? ImgUrls { get; set; }
    public IEnumerable<LessonVideoDto>? VideoUrls { get; set; }
   
}

