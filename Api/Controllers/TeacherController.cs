using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace _3rd_semester_exam_project.Controllers;


[RequireTeacherAuthentication]
public class TeacherController : ControllerBase
{
     private readonly CourseService _courseService;
     private readonly LessonService _lessonService;

    public TeacherController(CourseService courseService, LessonService lessonService)
    {
        _courseService = courseService;
        _lessonService = lessonService;
    }

    [HttpGet("/api/courses")]
    [RequireAuthentication]
    public ResponseDto GetAllCourses()
    {
       
        var courses =  _courseService.GetAllCourses();
        return new ResponseDto()
        {
            MessageToClient = "Successfully fetched",
            ResponseData = _courseService.GetAllCourses(),
        };
    }

    [HttpGet("/api/courses/{id}")]
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

    [HttpPost("/api/courses/create")]
    public ResponseDto CreateCourse( [FromBody] CourseDto courseDto)
    {
      
        return new ResponseDto
        {
            MessageToClient = "Successfully created",
            ResponseData = _courseService.CreateCourse(courseDto.Title, courseDto.Description, courseDto.CourseImgUrl)
                
        };
    }

    [HttpPut("/api/courses/update/{id}")]
    public ResponseDto UpdateCourse(int id, [FromBody] CourseDto courseDto)
    {
        
        var courseToUpdate = new CourseDto()
        {
            Id = id,
            Title = courseDto.Title,
            Description = courseDto.Description,
            CourseImgUrl = courseDto.CourseImgUrl
        };

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

    [HttpDelete("/api/courses/delete/{id}")]
    public ResponseDto DeleteCourse(int id)
    {
       
        _courseService.DeleteCourse(id);
        
        return new ResponseDto
        {
            MessageToClient = "Successfully deleted "
        };
    }

    
    [HttpGet("/api/courses/lessons/{id}")] 
    public ResponseDto GetLessonById([FromQuery] int id)
    {
        return new ResponseDto()
        {
            MessageToClient = "Successfully found",
            ResponseData = _lessonService.GetLessonById(id)
        };
    }
    
    [HttpPost("/api/courses/lesson/create")]
    public ResponseDto CreateLesson( [FromBody] LessonDto lessonDto)
    {
      
        return new ResponseDto
        {
            MessageToClient = "Successfully created",
            ResponseData = _lessonService.AddLesson(lessonDto.Title, lessonDto.Content, lessonDto.CourseId, null, null)
                
        };
    }
}