using System.ComponentModel.DataAnnotations;

namespace _3rd_semester_exam_project.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    [Required] 
    public string Title { get; set; }
    [Required] 
    public string Description { get; set; }
    public string CourseImgUrl { get; set; }
    
    public ICollection<LessonDto>? Lessons { get; set; }
}