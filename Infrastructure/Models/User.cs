using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class User
{
    public int Id { get; set; }
    public required string Fullname { get; set; }
    public required string Email { get; set; }
    public string? AvatarUrl { get; set; }
    public Role Role { get; set; }
    public string EmailVerificationToken { get; set; }
    public DateTime? EmailTokenExpiresAt { get; set; }
    public string PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiresAt { get; set; }
    public bool EmailVerified { get; set; }
}