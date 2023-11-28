using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.DTOs.CommandDTOs.UserDTOs;
using _3rd_semester_exam_project.DTOs.QueryDTOs.UserDTOs;
using _3rd_semester_exam_project.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.AdminService;

namespace _3rd_semester_exam_project.Controllers;

[RequireAdminAuthentication]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly AdminService _service;
    private readonly CourseService _courseService;
    private readonly LessonService _lessonService;

    public AdminController(AdminService service, CourseService courseService, LessonService lessonService)
    {
        _service = service;
        _courseService = courseService;
        _lessonService = lessonService;
    }
    
    
    [HttpGet("users")]
    public ResponseDto Get()
    {
        var users = _service.GetAll().Select(user => new UserDto
        {
            Id = user.Id,
            Fullname = user.Fullname,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            Role = user.Role,
            EmailVerified = user.EmailVerified
        }).ToList();

        return new ResponseDto
        {
            MessageToClient = "Successfully fetched",
            ResponseData = users
        };
    }


    [HttpPut("users/update/{id}")]
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
            
            var updated = _service.UpdateUser(id, user.Fullname, user.Email, user.AvatarUrl, user.Role);
            if (updated != null)
            {
                return new ResponseDto()
                {
                    MessageToClient = "Successfully updated",
                    ResponseData = new { id, user.Fullname, user.Email, user.AvatarUrl, user.Role }
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
        
    }

    [HttpDelete("users/delete/{id}")]
    public ResponseDto DeleteUser(int id)
    {
        _service.DeleteUser(id);
        return new ResponseDto()
        {
            MessageToClient = "Successfully deleted"
        };
    }
    
    [HttpGet("courses")]
    public ResponseDto GetAllCourses()
    {
       
        var courses =  _courseService.GetAllCourses();
        return new ResponseDto()
        {
            MessageToClient = "Successfully fetched",
            ResponseData = courses
        };
    }
    
    [HttpPost("courses/create")]
    public ResponseDto CreateCourse( [FromBody] CourseDto courseDto)
    {
      
        return new ResponseDto
        {
            MessageToClient = "Successfully created",
            ResponseData = _courseService.CreateCourse(courseDto.Title, courseDto.Description, courseDto.CourseImgUrl)
                
        };
    }

    [HttpPut("courses/update/{id}")]
    public ResponseDto UpdateCourse(int id, [FromBody] CourseDto courseDto)
    {
        

        var updatedCourse =  _courseService.UpdateCourse(id, courseDto.Title, courseDto.Description, courseDto.CourseImgUrl);
        if (updatedCourse == null)
        {
            return new ResponseDto
            {
                MessageToClient = "This Course's id not founded",
                
            };
        }
        
        return new ResponseDto
        {
            MessageToClient = "Successfully updated",
            ResponseData = _courseService.UpdateCourse(id, courseDto.Title, courseDto.Description, courseDto.CourseImgUrl)
                
        };
    }
    
    [HttpGet("courses/{id}")]
    public ResponseDto GetCourseById(int id)
    {
        var course =  _courseService.GetCourseById(id);
        if (course == null)
        {
            return new ResponseDto
            {
                MessageToClient = "No Course be funded",
                
            };
        }
        return new ResponseDto
        {
            MessageToClient = "Successfully found ",
            ResponseData = _courseService.GetCourseById(id),
                
        };
    }

    [HttpDelete("courses/delete/{id}")]
    public ResponseDto DeleteCourse(int id)
    {
       
        _courseService.DeleteCourse(id);
        
        return new ResponseDto
        {
            MessageToClient = "Successfully deleted "
        };
    }
    
    [HttpGet("courses/lessons/{id}")] 
    public ResponseDto GetLessonById([FromQuery] int id)
    {
        return new ResponseDto()
        {
            MessageToClient = "Successfully found",
            ResponseData = _lessonService.GetLessonById(id)
        };
    }
    
    [HttpPut("lessons/update/{id}")]
    public ResponseDto UpdateLesson(int id, [FromBody] LessonDto lessonDto)
    {
        var updatedLesson =  _lessonService.UpdateLesson(
            id, 
            lessonDto.Title, 
            lessonDto.Content, 
            lessonDto.CourseId, 
            lessonDto.ImgUrls?.Select(p => p.ImgUrl) ?? Enumerable.Empty<string>(), 
            lessonDto.VideoUrls?.Select(v => v.VideoUrl) ?? Enumerable.Empty<string>()
        );

        if (updatedLesson == null)
        {
            return new ResponseDto { MessageToClient = "Lesson not found or update failed" };
        }

        return new ResponseDto
        {
            MessageToClient = "Lesson successfully updated",
            ResponseData = updatedLesson
        };
    }
    
    [HttpDelete("lessons/delete/{id}")]
    public ResponseDto DeleteLesson(int id)
    {
        _lessonService.DeleteLesson(id);
        return new ResponseDto
        {
            MessageToClient = "Successfully deleted "
        };
    }
    
}