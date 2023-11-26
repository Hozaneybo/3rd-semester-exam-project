using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.Filters;
using Infrastructure.Models;
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
    
    
    
    /*
    [HttpPost("/api/courses/lesson/create")]
    public async Task<ResponseDto> CreateLesson([FromBody] LessonDto lessonDto)
    {
        var lesson = await _lessonService.AddLesson(lessonDto);
        return new ResponseDto
        {
            MessageToClient = "Successfully created",
            ResponseData = lesson
        };
    }
    */

    
    
    [HttpPost("/api/courses/lesson/create")]
    public async Task<ResponseDto> CreateLesson([FromBody] LessonDto lessonDto)
    {
        
        var lessonPictures = lessonDto.ImgUrls?
            .Select(p => new LessonPicture { ImgUrl = p.ImgUrl, LessonId = p.LessonId })
            .ToList();

        var lessonVideos = lessonDto.VideoUrls?
            .Select(v => new LessonVideo { VideoUrl = v.VideoUrl, LessonId = v.LessonId })
            .ToList();
        var lesson = new Lesson
        {
            Title = lessonDto.Title,
            Content = lessonDto.Content,
            CourseId = lessonDto.CourseId,
            ImgUrls = lessonPictures, 
            VideoUrls = lessonVideos 
        };

        var createdLesson = await _lessonService.AddLesson(lesson);
        return new ResponseDto
        {
            MessageToClient = "Successfully created",
            ResponseData = createdLesson
        };
    }
    
    
    [HttpPut("/api/lessons/update/{id}")]
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

    
    [HttpDelete("/api/lessons/delete/{id}")]
    public ResponseDto DeleteLesson(int id)
    {
         _lessonService.DeleteLesson(id);
         return new ResponseDto
         {
             MessageToClient = "Successfully deleted "
         };
    }

    

}