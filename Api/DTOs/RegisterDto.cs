using System.ComponentModel.DataAnnotations;

namespace _3rd_semester_exam_project.DTOs;

public class RegisterDto
{
    [Required] public required string FullName { get; set; }

    [Required] [EmailAddress] public required string Email { get; set; }

    [Required] [MinLength(8)] public required string Password { get; set; }

    public string? AvatarUrl { get; set; }
    
}