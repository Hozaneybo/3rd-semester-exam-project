using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.CQ.Queries;

namespace _3rd_semester_exam_project.Controllers;

[RequireAuthentication]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly CourseService _courseService;
    private readonly LessonService _lessonService;
    private readonly SharedService _sharedService;

    public StudentController(CourseService courseService, LessonService lessonService, SharedService sharedService)
    {
        _courseService = courseService;
        _lessonService = lessonService;
        _sharedService = sharedService;
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
                ResponseData = courses,
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = "An error occurred while retrieving courses." });
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
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
            {
                MessageToClient = "An error occurred while retrieving the course."
            });
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
                return NotFound(new ResponseDto
                {
                    MessageToClient = "Lesson not found"
                });
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
        catch (Exception ex)
        {
            // Log the exception if necessary
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
            {
                MessageToClient = "An error occurred while retrieving the lesson."
            });
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