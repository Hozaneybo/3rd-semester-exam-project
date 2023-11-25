using _3rd_semester_exam_project.DTOs;
using _3rd_semester_exam_project.Filters;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace _3rd_semester_exam_project.Controllers;


[TeacherRequireAuthentication]
public class TeacherController : ControllerBase
{
     private readonly CourseService _courseService;

    public TeacherController(CourseService courseService)
    {
        _courseService = courseService;
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
    [RequireAuthentication]
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
    [RequireAuthentication]
    public ResponseDto CreateCourse(Course course)
    {
      
        return new ResponseDto
        {
            MessageToClient = "Successfully created",
            ResponseData = _courseService.CreateCourse(course)
                
        };
    }

    [HttpPut("/api/courses/update/{id}")]
    [RequireAuthentication]
    public ResponseDto UpdateCourse(int id, [FromBody] CourseDto courseDto)
    {
        
        var courseToUpdate = new Course
        {
            Id = id,
            Title = courseDto.Title,
            Description = courseDto.Description
        };

        var updatedCourse =  _courseService.UpdateCourse(courseToUpdate);
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
            ResponseData = _courseService.UpdateCourse(courseToUpdate)
                
        };
        
    }

    [HttpDelete("/api/courses/delete/{id}")]
    [RequireAuthentication]
    public ResponseDto DeleteCourse(int id)
    {
       
        _courseService.DeleteCourse(id);
        
        return new ResponseDto
        {
            MessageToClient = "Successfully deleted "
        };
    }
}