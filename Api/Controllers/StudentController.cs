using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace _3rd_semester_exam_project.Controllers;

[RequireAuthentication]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly CourseService _courseService;
    private readonly LessonService _lessonService;

    public StudentController(CourseService courseService, LessonService lessonService)
    {
        _courseService = courseService;
        _lessonService = lessonService;
    }
    
    [HttpGet("courses")]
    public ResponseDto GetAllCourses()
    {
       
        var courses =  _courseService.GetAllCourses();
        return new ResponseDto()
        {
            MessageToClient = "Successfully fetched",
            ResponseData = _courseService.GetAllCourses(),
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
    
    [HttpGet("courses/lessons/{id}")] 
    public ResponseDto GetLessonById([FromQuery] int id)
    {
        return new ResponseDto()
        {
            MessageToClient = "Successfully found",
            ResponseData = _lessonService.GetLessonById(id)
        };
    }
    
}