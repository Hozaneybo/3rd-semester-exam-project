namespace _3rd_semester_exam_project.DTOs;

public class LessonIdAndTitleResult
{
    public int Id { get; set; }
    public string Title { get; set; }
}

public class LessonByIdResult
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public IEnumerable<PictureUrlResult>? ImgUrls { get; set; }
    public IEnumerable<VideoUrlResult>? VideoUrls { get; set; }
    public int CourseId { get; set; }
    
}

public class PictureUrlResult
{
    public int Id { get; set; }
    public string PictureUrl { get; set; }
}

public class VideoUrlResult
{
    public int Id { get; set; }
    public string VideoUrl { get; set; }
}