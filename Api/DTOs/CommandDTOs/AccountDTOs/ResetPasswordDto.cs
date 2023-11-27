using System.ComponentModel.DataAnnotations;

namespace _3rd_semester_exam_project.DTOs.CommandDTOs.AccountDTOs;

public class ResetPasswordDto
{
    [Required]
    public string Token { get; set; }
    [Required]
    public string NewPassword { get; set; }
}