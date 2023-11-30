using System.ComponentModel.DataAnnotations;
using Infrastructure.Models;

namespace Service.CQ.Commands;

public class CreateUserCommand
{
    [Required, MinLength(3), MaxLength(20)]
    public string FullName { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, MinLength(8)]
    public string Password { get; set; }
    public string? AvatarUrl { get; set; }
}

public class UpdateUserCommand
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? AvatarUrl { get; set; }
    [Required]
    public Role Role { get; set; }
}

