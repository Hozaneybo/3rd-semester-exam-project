using System.ComponentModel.DataAnnotations;
using Infrastructure.Models;

namespace Service.CQ.Commands;

public class CreateUserCommand
{
    [Required(ErrorMessage = "Full name is required."), MinLength(3, ErrorMessage = "Full name must be at least 3 characters long."), MaxLength(20, ErrorMessage = "Full name cannot exceed 20 characters.")]
    public string FullName { get; set; }
    [Required(ErrorMessage = "Email is required."), EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required."), MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; }
    public string? AvatarUrl { get; set; }
}

public class UpdateUserCommand
{
    [Required(ErrorMessage = "User ID is required.")]
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? AvatarUrl { get; set; }
    [Required]
    public Role Role { get; set; }
}

public class UpdateProfileCommand
{
    [Required(ErrorMessage = "User ID is required.")]
    public int Id { get; set; }
    public string FullName { get; set; }
    public string? AvatarUrl { get; set; }
    
}

