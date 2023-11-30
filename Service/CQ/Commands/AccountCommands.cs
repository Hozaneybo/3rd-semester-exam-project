using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Service.CQ.Commands;

public class ResetPasswordCommand
{
    [Required]
    public string Token { get; set; }
    [Required, MinLength(8)]
    public string NewPassword { get; set; }
}

public class UserLoginCommand
{
    [EmailAddress, Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}