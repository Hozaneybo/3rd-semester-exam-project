using System.ComponentModel.DataAnnotations;

namespace Service.CQ.Commands;

public class CreateCourseCommand
{
    [Required, MinLength(2), MaxLength(20)]
    public string Title { get; set; }
    
    [Required, MinLength(30), MaxLength(250)]
    public string Description { get; set; }
    [Required]
    public string CourseImgUrl { get; set; }
}

public class UpdateCourseCommand
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string CourseImgUrl { get; set; }
}

