using Infrastructure.Models;

namespace _3rd_semester_exam_project.DTOs;

public class UserResult
{
    public int Id { get; set; }
    public required string Fullname { get; set; }
    public required string Email { get; set; }
    public string? AvatarUrl { get; set; }
    public Role Role { get; set; }
    public bool EmailVerified { get; set; }
}