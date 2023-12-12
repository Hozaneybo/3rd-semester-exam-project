using System.ComponentModel.DataAnnotations;

namespace Service.CQ.Commands;

public class CreateCourseCommand
{
    [Required(ErrorMessage = "Title is required."), MinLength(2, ErrorMessage = "Title must be at least 2 characters long."), MaxLength(20, ErrorMessage = "Title cannot exceed 20 characters.")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Description is required."), MinLength(30, ErrorMessage = "Description must be at least 30 characters long."), MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Course image URL is required.")]
    public string CourseImgUrl { get; set; }
}

public class UpdateCourseCommand
{
    [Required(ErrorMessage = "Course ID is required.")]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string CourseImgUrl { get; set; }
}

