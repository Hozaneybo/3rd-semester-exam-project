using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class User
{
    public int Id { get; set; }
    public required string Fullname { get; set; }
    public required string Email { get; set; }
    public string? AvatarUrl { get; set; }
    public UserRole Role { get; set; }
}