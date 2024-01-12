using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Service.CQ.Commands;

public class ResetPasswordCommand
{
    [Required(ErrorMessage = "Token is required.")]
    public string Token { get; set; }
    [Required(ErrorMessage = "New password is required."), MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string NewPassword { get; set; }
}

public class UserLoginCommand
{
    [EmailAddress(ErrorMessage = "Invalid email address format."), Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}