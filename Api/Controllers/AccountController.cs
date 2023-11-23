using _3rd_semester_exam_project.DTOs;
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
        return new ResponseDto
        {
            MessageToClient = "Successfully authenticated"
        };
    }

    [HttpPost]
    [Route("/api/account/register")]
    public ResponseDto Register([FromBody] RegisterDto dto)
    {
        try
        {
            var user = _service.Register(dto.FullName, dto.Email, dto.Password, avatarUrl: dto.AvatarUrl);
            HttpContext.Response.StatusCode = 201;
            return new ResponseDto
            {
                MessageToClient = "Successfully registered"
            };
        }
        catch (InvalidOperationException ex)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return new ResponseDto
            {
                MessageToClient = ex.Message
            };
        }
    }
}