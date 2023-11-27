

using System.ComponentModel.DataAnnotations;
using Infrastructure.Models;

namespace _3rd_semester_exam_project.DTOs.CommandDTOs.UserDTOs;

public class UpdateUserDto
{
    public int Id { get; set; }
    [Required, MaxLength(16)]
    public string Fullname { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string? AvatarUrl { get; set; }
    public Role Role { get; set; }
}

