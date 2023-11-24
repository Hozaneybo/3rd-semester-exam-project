using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.Filters;
using Microsoft.AspNetCore.Mvc;
using Service.AdminService;
using Role = Infrastructure.Models.Role;

namespace _3rd_semester_exam_project.Controllers;

[RequireAuthentication]
public class AdminController : ControllerBase
{
    private readonly AdminService _service;

    public AdminController(AdminService service)
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

    [HttpPut("/api/users/update/{id}")]
    public ResponseDto UpdateUser(int id , [FromBody] UpdateUserDto user)
    {
        try
        {
            var existingUser = _service.GetUserById(id);
            if (existingUser == null)
            {
                HttpContext.Response.StatusCode = 404;
                return new ResponseDto()
                {
                    MessageToClient = "User with given id not found",
                    ResponseData = null
                };
            }

            user.Id = id;
            var updated = _service.UpdateUser(user.Id, user.Fullname, user.Email, user.Role);
            if (updated != null)
            {
                return new ResponseDto()
                {
                    MessageToClient = "Successfully updated",
                    ResponseData = updated
                };
            }
            else
                return new ResponseDto()
                {
                    MessageToClient = "User could not be updated",
                    ResponseData = null
                };

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
        /*
        catch (Exception e)
        {
            _logger.LogError("An error occurred while updating the user: {Message}", e.Message);
            return StatusCode(500, new ResponseDto
            {
                MessageToClient = "An error occurred while updating the user."
            });
            */
        
    }
}