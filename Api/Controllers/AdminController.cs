using _3rd_semester_exam_project.DTOs;
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
    public IActionResult Get()
    {
        try
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

            return Ok(new ResponseDto
            {
                MessageToClient = "Successfully fetched",
                ResponseData = users
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
    }

    
    [HttpGet("users/{id}")]
    public IActionResult GetUserById(int id)
    {
        try
        {
            var user = _service.GetUserById(id);
            if (user == null)
            {
                return NotFound(new ResponseDto
                {
                    MessageToClient = "User not found"
                });
            }

            var userResult = new UserResult()
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Role = user.Role,
                EmailVerified = user.EmailVerified
            };

            return Ok(new ResponseDto
            {
                MessageToClient = "Successfully fetched",
                ResponseData = userResult
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
    }



    [HttpPut("users/update/{id}")]
    public IActionResult UpdateUser(int id, [FromBody] UpdateUserCommand user)
    {
        try
        {
            var existingUser = _service.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound(new ResponseDto
                {
                    MessageToClient = "User with given id not found",
                    ResponseData = null
                });
            }

            user.Id = id;
            var updatedUser = _service.UpdateUser(user);
            if (updatedUser != null)
            {
                return Ok(new ResponseDto
                {
                    MessageToClient = "Successfully updated",
                    ResponseData = new { id, user.FullName, user.Email, user.AvatarUrl, user.Role }
                });
            }
            else
            {
                return BadRequest(new ResponseDto
                {
                    MessageToClient = "User could not be updated",
                    ResponseData = null
                });
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
    }


    [HttpDelete("users/delete/{id}")]
    public IActionResult DeleteUser(int id)
    {
        try
        {
            _service.DeleteUser(id);
            return Ok(new ResponseDto
            {
                MessageToClient = "Successfully deleted"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
    }

    
    [HttpGet("courses")]
    public IActionResult GetAllCourses()
    {
        try
        {
            var courses = _courseService.GetAllCourses().Select(course => new AllCoursesResult
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                CourseImgUrl = course.CourseImgUrl
            
            }).ToList();
            return Ok(new ResponseDto
            {
                MessageToClient = "Successfully fetched",
                ResponseData = courses
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
    }
    
    [HttpPost("courses/create")]
    public IActionResult CreateCourse([FromBody] CreateCourseCommand command)
    {
        try
        {
            var createdCourse = _courseService.CreateCourse(command);
            if (createdCourse == null)
            {
                return BadRequest(new ResponseDto { MessageToClient = "Course could not be created." });
            }

            return Ok(new ResponseDto
            {
                MessageToClient = "Successfully created",
                ResponseData = createdCourse
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
    }


    [HttpPut("courses/update/{id}")]
    public IActionResult UpdateCourse(int id, [FromBody] UpdateCourseCommand command)
    {
        try
        {
            var existingCourse = _courseService.GetCourseById(id);
            if (existingCourse == null)
            {
                return NotFound(new ResponseDto { MessageToClient = "Course with given id not found", ResponseData = null });
            }

            command.Id = id;
            var updatedCourse = _courseService.UpdateCourse(command);
            if (updatedCourse == null)
            {
                return BadRequest(new ResponseDto { MessageToClient = "Course could not be updated", ResponseData = null });
            }

            return Ok(new ResponseDto
            {
                MessageToClient = "Successfully updated",
                ResponseData = updatedCourse
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
    }

    
    [HttpGet("courses/{id}")]
    public IActionResult GetCourseById(int id)
    {
        try
        {
            var course = _courseService.GetCourseById(id);

            if (course == null)
            {
                return NotFound(new ResponseDto
                {
                    MessageToClient = "Course not found"
                });
            }

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

            return Ok(new ResponseDto
            {
                MessageToClient = "Successfully found",
                ResponseData = courseResult
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
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
    public IActionResult DeleteCourse(int id)
    {
        try
        {
            _courseService.DeleteCourse(id);
            return Ok(new ResponseDto
            {
                MessageToClient = "Successfully deleted"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
    }

    
    [HttpGet("courses/{courseId}/lessons/{id}")] 
    public IActionResult GetLessonById(int courseId, int id)
    {
        try
        {
            var lesson = _lessonService.GetLessonById(courseId, id);

            if (lesson == null)
            {
                return NotFound(new ResponseDto { MessageToClient = "Lesson not found" });
            }

            var lessonContent = new LessonByIdResult()
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Content = lesson.Content,
                ImgUrls = lesson.ImgUrls.Select(imgUrl => new PictureUrlResult
                {
                    Id = imgUrl.Id,
                    PictureUrl = imgUrl.ImgUrl
                }).ToList(),
                VideoUrls = lesson.VideoUrls.Select(videoUrl => new VideoUrlResult
                {
                    Id = videoUrl.Id,
                    VideoUrl = videoUrl.VideoUrl
                }).ToList(),
                CourseId = lesson.CourseId
            };

            return Ok(new ResponseDto
            {
                MessageToClient = "Successfully found",
                ResponseData = lessonContent
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
    }

    
    [HttpPut("lessons/update/{id}")]
    public IActionResult UpdateLesson(int id, [FromBody] UpdateLessonCommand command)
    {
        try
        {
            var updatedLesson = _lessonService.UpdateLesson(command);

            if (updatedLesson == null)
            {
                return NotFound(new ResponseDto { MessageToClient = "Lesson not found or update failed" });
            }

            return Ok(new ResponseDto
            {
                MessageToClient = "Lesson successfully updated",
                ResponseData = GetLessonById(command.CourseId, id)
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
    }

    
    [HttpDelete("lessons/delete/{id}")]
    public IActionResult DeleteLesson(int id)
    {
        try
        {
            _lessonService.DeleteLesson(id);
            return Ok(new ResponseDto
            {
                MessageToClient = "Successfully deleted"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An internal error occurred. Please try again later." });
        }
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