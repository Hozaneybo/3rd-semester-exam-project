using _3rd_semester_exam_project.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace _3rd_semester_exam_project.Controllers;

[Route("api/[controller]")]
public class GuestController : ControllerBase
{
    private readonly CourseService _courseService;

    public GuestController(CourseService courseService)
    {
        _courseService = courseService;
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
}