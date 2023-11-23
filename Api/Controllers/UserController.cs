using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace _3rd_semester_exam_project.Controllers;

[RequireAuthentication]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    [HttpGet("/api/users")]
    public ResponseDto Get()
    {
        return new ResponseDto
        {
            MessageToClient = "Successfully fetched",
            ResponseData = _service.GetAll()
        };
    }
}