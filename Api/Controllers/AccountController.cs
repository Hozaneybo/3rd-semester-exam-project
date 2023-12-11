using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.CQ.Commands;
using Service.CQ.Queries;
using System;
using MailKit.Security;
using Npgsql;

namespace _3rd_semester_exam_project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _service;
    private readonly SharedService _sharedService;

    public AccountController(AccountService service, SharedService sharedService)
    {
        _service = service;
        _sharedService = sharedService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginCommand command)
    {
        try
        {
            var user = _service.Authenticate(command);
            if (user != null)
            {
                HttpContext.SetSessionData(SessionData.FromUser(user));
                return Ok(new ResponseDto
                {
                    MessageToClient = "Successfully authenticated",
                    ResponseData = new { Role = user.Role }
                });
            }

            return Unauthorized(new ResponseDto { MessageToClient = "Invalid credentials." });
        }
        catch (AuthenticationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = "Authentication failed." });
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new ResponseDto { MessageToClient = "Successfully logged out" });
    }
    
    [HttpPost("register")]
    public IActionResult Register([FromBody] CreateUserCommand command)
    {
        try
        {
            var user = _service.Register(command);
            return CreatedAtAction(nameof(WhoAmI), new { id = user.Id }, new ResponseDto
            {
                MessageToClient = "Registration successful. Please check your email to verify your account."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (NpgsqlException ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
        
    }
    
    [HttpGet("verify-email")]
    public IActionResult VerifyEmail([FromQuery] string token)
    {
        try
        {
            var result = _service.VerifyEmailToken(token);
            if (result)
            {
                return Ok(new ResponseDto { MessageToClient = "Email successfully verified." });
            }
            return BadRequest(new ResponseDto { MessageToClient = "Invalid or expired verification token." });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = "An error occurred during email verification." });
        }
    }
    
    [HttpPost("request-password-reset")]
    public IActionResult RequestPasswordReset([FromBody] UserQuery query)
    {
        try
        {
            var user = _service.GetUserByEmail(query);
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

    [HttpPost("reset-password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordCommand command)
    {
        try
        {
            var result = _service.ResetPasswordWithToken(command.Token, command.NewPassword);
            if (result)
            {
                return Ok(new ResponseDto { MessageToClient = "Password has been reset successfully." });
            }
            return BadRequest(new ResponseDto { MessageToClient = "Invalid or expired password reset token." });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = "An error occurred during password reset." });
        }
    }

    [RequireAuthentication]
    [HttpGet("whoami")]
    public IActionResult WhoAmI()
    {
        try
        {
            var data = HttpContext.GetSessionData();
            var user = _service.Get(data);
            if (user != null)
            {
                return Ok(new ResponseDto
                {
                    ResponseData = new { user.Id, user.Fullname, user.Email, user.AvatarUrl, user.Role, user.EmailVerified }
                });
            }
            return NotFound(new ResponseDto { MessageToClient = "User not found." });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = "An error occurred." });
        }
    }
    
    [RequireAuthentication]
    [HttpGet("search")]
    public IActionResult Search([FromQuery] SearchQueryModel queryModel)
    { 
        try 
        { 
            var searchResults = _sharedService.Search(queryModel).Select(result => new SearchResultDto 
            { 
                Type = result.Type, 
                Term = result.Term 
            }).ToList();
            
            return Ok(new ResponseDto { MessageToClient = "Search results fetched successfully", ResponseData = searchResults }); 
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
    }
}
