using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace _3rd_semester_exam_project.Controllers;


[RequireTeacherAuthentication]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{
     private readonly CourseService _courseService;
     private readonly LessonService _lessonService;

    public TeacherController(CourseService courseService, LessonService lessonService)
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
    
    
    [HttpPost("courses/lesson/create")]
    public ResponseDto CreateLesson([FromBody] LessonDto lessonDto)
    {

        var createdLesson = _lessonService.AddLesson(lessonDto.Title, lessonDto.Content, lessonDto.CourseId, lessonDto.ImgUrls?.Select(p => p.ImgUrl) ?? Enumerable.Empty<string>(), 
            lessonDto.VideoUrls?.Select(v => v.VideoUrl) ?? Enumerable.Empty<string>());
        return new ResponseDto
        {
            MessageToClient = "Successfully created",
            ResponseData = createdLesson
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