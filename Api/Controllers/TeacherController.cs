﻿using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.CQ.Commands;
using Service.CQ.Queries;

namespace _3rd_semester_exam_project.Controllers;


[RequireTeacherAuthentication]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{
     private readonly CourseService _courseService;
     private readonly LessonService _lessonService;
     private readonly SharedService _sharedService;

    public TeacherController(CourseService courseService, LessonService lessonService, SharedService sharedService)
    {
        _courseService = courseService;
        _lessonService = lessonService;
        _sharedService = sharedService;
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
    
    
    [HttpPost("courses/lesson/create")]
    public ResponseDto CreateLesson([FromBody] CreateLessonCommand command)
    {

        var createdLesson = _lessonService.AddLesson(command);
        
        return new ResponseDto
        {
            MessageToClient = "Successfully created",
            ResponseData = GetLessonById(createdLesson.CourseId, createdLesson.Id)
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
    
    [HttpGet("users/role")]
    public ResponseDto GetUsersByRole(RoleQueryModel roleQueryModel)
    {

        var users = _sharedService.GetUsersByRole(roleQueryModel).Select(user => new UserResult()
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
    [HttpGet("search")]
    public ResponseDto Search([FromQuery] SearchQueryModel queryModel)
    {
        var searchResults = _sharedService.Search(queryModel).Select(result => new SearchResultDto
        {
            Type = result.Type,
            Term = result.Term
        }).ToList();

        return new ResponseDto
        {
            MessageToClient = "Search results fetched successfully",
            ResponseData = searchResults
        };
    }
}