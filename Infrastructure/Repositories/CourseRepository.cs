using Dapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public CourseRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<Course> GetAllCourses()
    {
        const string sql = $@"
SELECT 
id as {nameof(Course.Id)},
title as {nameof(Course.Title)},
description as {nameof(Course.Description)},
course_img_url as {nameof(Course.CourseImgUrl)}
FROM learning_platform.courses;";
        
        using var connection = _dataSource.OpenConnection();
        return connection.Query<Course>(sql);
    }

    public Course GetCourseById(int id)
    {
        const string courseSql = $@"
SELECT 
    id as {nameof(Course.Id)},
    title as {nameof(Course.Title)},
    description as {nameof(Course.Description)},
    course_img_url as {nameof(Course.CourseImgUrl)}
FROM learning_platform.courses WHERE id = @Id;
";

        const string lessonsSql = $@"
SELECT 
    id as {nameof(Lesson.Id)},
    title as {nameof(Lesson.Title)}
    From learning_platform.lessons
WHERE course_id = @Id;
";

        using var connection = _dataSource.OpenConnection();
        var course = connection.QuerySingleOrDefault<Course>(courseSql, new { Id = id });

        if (course != null)
        {
            var lessons = connection.Query<Lesson>(lessonsSql, new { Id = id });
            course.Lessons = lessons.ToList();
        }

        return course;
    }


    public Course AddCourse(string title, string description, string courseImgUrl)
    {
        const string sql = $@"
INSERT INTO learning_platform.courses (title, description, course_img_url)
VALUES (@Title, @Description, @CourseImgUrl)
RETURNING 
    id as {nameof(Course.Id)},
    title as {nameof(Course.Title)},
    description as {nameof(Course.Description)},
    course_img_url as {nameof(Course.CourseImgUrl)};
";
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirst<Course>(sql, new { Title = title, Description = description, CourseImgUrl = courseImgUrl });
    }

    public Course UpdateCourse(int id, string title, string description, string courseImgUrl)
    {
        const string sql = @"
UPDATE learning_platform.courses
SET 
    title = COALESCE(@Title, title),
    description = COALESCE(@Description, description),
    course_img_url = COALESCE(@CourseImgUrl, course_img_url)
WHERE id = @Id
RETURNING *;
";
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirstOrDefault<Course>(sql, new { Id = id, Title = title, Description = description, CourseImgUrl = courseImgUrl });
    }


    public void DeleteCourse(int id)
    {
        const string sql = @"
DELETE FROM learning_platform.courses
WHERE id = @Id;
";
        using var connection = _dataSource.OpenConnection();
        connection.Execute(sql, new { id });
    }
}
