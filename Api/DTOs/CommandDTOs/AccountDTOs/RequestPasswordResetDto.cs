using System.ComponentModel.DataAnnotations;

namespace _3rd_semester_exam_project.DTOs.CommandDTOs.AccountDTOs;

public class RequestPasswordResetDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
}