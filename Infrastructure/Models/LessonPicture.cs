﻿namespace Infrastructure.Models;

public class LessonPicture
{
    public int Id { get; set; }
    public string ImgUrl { get; set; }
    public int LessonId { get; set; }
}