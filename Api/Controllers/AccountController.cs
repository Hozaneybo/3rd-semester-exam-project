using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.DTOs.CommandDTOs.AccountDTOs;
using _3rd_semester_exam_project.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace _3rd_semester_exam_project.Controllers;

public class AccountController : ControllerBase
{
    private readonly AccountService _service;

    public AccountController(AccountService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("/api/account/login")]
    public ResponseDto Login([FromBody] LoginDto dto)
    {
        var user = _service.Authenticate(dto.Email, dto.Password);
        HttpContext.SetSessionData(SessionData.FromUser(user));
        return new ResponseDto
        {
            MessageToClient = "Successfully authenticated"
        };
    }

    [HttpPost]
    [Route("/api/account/logout")]
    public ResponseDto Logout()
    {
        HttpContext.Session.Clear();
        return new ResponseDto
        {
            MessageToClient = "Successfully logged out"
        };
    }
    
    [HttpPost]
    [Route("/api/account/register")]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        try
        {
            var user = _service.Register(dto.FullName, dto.Email, dto.Password, dto.AvatarUrl);

            return CreatedAtAction(nameof(WhoAmI), new { id = user.Id }, new ResponseDto
            {
                MessageToClient = "Registration successful. Please check your email to verify your account."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto
            {
                MessageToClient = ex.Message
            });
        }
    }
    
    [HttpGet]
    [Route("/api/account/verify-email")]
    public IActionResult VerifyEmail([FromQuery] string token)
    {
        try
        {
            var result = _service.VerifyEmailToken(token);
            if (result)
            {
                return Ok(new ResponseDto { MessageToClient = "Email successfully verified." });
            }
            else
            {
                return BadRequest(new ResponseDto { MessageToClient = "Invalid or expired verification token." });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = "An error occurred during email verification." });
        }
    }
    
    [HttpPost]
    [Route("/api/account/request-password-reset")]
    public IActionResult RequestPasswordReset([FromBody] RequestPasswordResetDto dto)
    {
        try
        {
            var user = _service.GetUserByEmail(dto.Email);
            if (user != null)
            {
                _service.GenerateAndSendPasswordResetToken(user);
                return Ok(new ResponseDto { MessageToClient = "Password reset email sent. Please check your inbox." });
            }
            return NotFound(new ResponseDto { MessageToClient = "User not found." });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = "An error occurred during password reset request." });
        }
    }

    [HttpPost]
    [Route("/api/account/reset-password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordDto dto)
    {
        try
        {
            var result = _service.ResetPasswordWithToken(dto.Token, dto.NewPassword);
            if (result)
            {
                return Ok(new ResponseDto { MessageToClient = "Password has been reset successfully." });
            }
            else
            {
                return BadRequest(new ResponseDto { MessageToClient = "Invalid or expired password reset token." });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = "An error occurred during password reset." });
        }
    }


    
    [RequireAuthentication]
    [HttpGet]
    [Route("/api/account/whoami")]
    public ResponseDto WhoAmI()
    {
        var data = HttpContext.GetSessionData();
        var user = _service.Get(data);
        return new ResponseDto
        {
            ResponseData = user
        };
    }
}