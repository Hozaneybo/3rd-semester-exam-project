﻿using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.CQ.Commands;
using Service.CQ.Queries;

namespace _3rd_semester_exam_project.Controllers;

[RequireAdminAuthentication]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly AdminService _service;
    private readonly CourseService _courseService;
    private readonly LessonService _lessonService;
    private readonly SharedService _sharedService;

    public AdminController(AdminService service, CourseService courseService, LessonService lessonService, SharedService sharedService)
    {
        _service = service;
        _courseService = courseService;
        _lessonService = lessonService;
        _sharedService = sharedService;
    }
    
    
    [HttpGet("users")]
    public ResponseDto Get()
    {
        var users = _service.GetAll().Select(user => new UserResult
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
    
    [HttpGet("users/{id}")]
    public ResponseDto GetUserById(int id)
    {
        var user = _service.GetUserById(id);
        var userResult = new UserResult()
        {
            Id = user.Id,
            Fullname = user.Fullname,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            Role = user.Role,
            EmailVerified = user.EmailVerified

        };

        return new ResponseDto
        {
            MessageToClient = "Successfully fetched",
            ResponseData = userResult
        };
    }


    [HttpPut("users/update/{id}")]
    public ResponseDto UpdateUser(int id , [FromBody] UpdateUserCommand user)
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
            var updated = _service.UpdateUser(user);
            if (updated != null)
            {
                return new ResponseDto()
                {
                    MessageToClient = "Successfully updated",
                    ResponseData = new { id, user.FullName, user.Email, user.AvatarUrl, user.Role }
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
       
        var courses =  _courseService.GetAllCourses().Select(course => new AllCoursesResult
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            CourseImgUrl = course.CourseImgUrl
            
        }).ToList();
        return new ResponseDto()
        {
            MessageToClient = "Successfully fetched",
            ResponseData = courses
        };
    }
    
    [HttpPost("courses/create")]
    public ResponseDto CreateCourse( [FromBody] CreateCourseCommand command)
    {
      
        return new ResponseDto
        {
            MessageToClient = "Successfully created",
            ResponseData = _courseService.CreateCourse(command)
                
        };
    }

    [HttpPut("courses/update/{id}")]
    public ResponseDto UpdateCourse(int id, [FromBody] UpdateCourseCommand command)
    {

        try
        {
            var existingCourse = GetCourseById(id);
            if (existingCourse == null)
            {
                HttpContext.Response.StatusCode = 404;
                return new ResponseDto() { MessageToClient = "Course with given id not found", ResponseData = null };
            }
            command.Id = id;
            var updatedCourse =  _courseService.UpdateCourse(command);
            if (updatedCourse != null)
            {
                return new ResponseDto
                {
                    MessageToClient = "Successfully updated",
                    ResponseData = command
                };
            }
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return new ResponseDto
        {
            MessageToClient = "Successfully updated",
            ResponseData = command

        };
    }
    
    [HttpGet("courses/{id}")]
    public ResponseDto GetCourseById(int id)
    {
        var course =  _courseService.GetCourseById(id);
        var courseResult = new CourseContentById()
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            CourseImgUrl = course.CourseImgUrl,
            Lessons = course.Lessons.Select(lesson => new LessonIdAndTitleResult()
            {
                Id = lesson.Id,
                Title = lesson.Title
            }).ToList()
        };
        if (course == null)
        {
            return new ResponseDto
            {
                MessageToClient = "Course not found",
                
            };
        }
        return new ResponseDto
        {
            MessageToClient = "Successfully found ",
            ResponseData = courseResult,
                
        };
    }
    
    [HttpGet("users/role")]
    public IActionResult GetUsersByRole(RoleQueryModel roleQueryModel)
    {
        try
        {
            var users = _sharedService.GetUsersByRole(roleQueryModel).Select(user => new UserResult
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Role = user.Role,
                EmailVerified = user.EmailVerified
            }).ToList();

            return Ok(new ResponseDto
            {
                MessageToClient = "Successfully fetched users by role",
                ResponseData = users
            });
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

    [HttpDelete("courses/delete/{id}")]
    public ResponseDto DeleteCourse(int id)
    {
       
        _courseService.DeleteCourse(id);
        
        return new ResponseDto
        {
            MessageToClient = "Successfully deleted "
        };
    }
    
    [HttpGet("courses/{courseId}/lessons/{id}")] 
    public ResponseDto GetLessonById(int courseId, int id)
    {
        
        var lesson = _lessonService.GetLessonById(courseId, id);
        var lessonContent = new LessonByIdResult()
        {
            Id = lesson.Id,
            Title = lesson.Title,
            Content = lesson.Content,
            ImgUrls = lesson.ImgUrls.Select(imgUrl => new PictureUrlResult()
            {
                Id = imgUrl.Id,
                PictureUrl = imgUrl.ImgUrl
            }).ToList(),
            VideoUrls = lesson.VideoUrls.Select(videoUrl => new VideoUrlResult()
            {
                Id = videoUrl.Id,
                VideoUrl = videoUrl.VideoUrl
            }).ToList(),
            CourseId = lesson.CourseId

        };
        return new ResponseDto()
        {
            MessageToClient = "Successfully found",
            ResponseData = lessonContent
        };
    }
    
    [HttpPut("lessons/update/{id}")]
    public ResponseDto UpdateLesson(int id, [FromBody] UpdateLessonCommand command)
    {
        
        var updatedLesson =  _lessonService.UpdateLesson(command);

        if (updatedLesson == null)
        {
            return new ResponseDto { MessageToClient = "Lesson not found or update failed" };
        }

        return new ResponseDto
        {
            MessageToClient = "Lesson successfully updated",
            ResponseData = GetLessonById(command.CourseId, id)
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