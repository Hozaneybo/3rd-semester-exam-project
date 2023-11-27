using System.ComponentModel.DataAnnotations;

namespace _3rd_semester_exam_project.DTOs.CommandDTOs.AccountDTOs;

public class LoginDto
{
    [EmailAddress, Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}