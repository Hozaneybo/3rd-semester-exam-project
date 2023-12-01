namespace _3rd_semester_exam_project.DTOs;

public class AllCoursesResult
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string CourseImgUrl { get; set; }
}

public class CourseContentById
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string CourseImgUrl { get; set; }
    public IEnumerable<LessonIdAndTitleResult> Lessons { get; set; }
}