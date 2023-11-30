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
            ResponseData = courses,
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
    
}