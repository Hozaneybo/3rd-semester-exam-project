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
        try
        {
            return connection.Query<Course>(sql);
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("An error occurred while retrieving courses.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while retrieving courses.", ex);
        }
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
        try
        {
            var course = connection.QuerySingleOrDefault<Course>(courseSql, new { Id = id });

            if (course != null)
            {
                var lessons = connection.Query<Lesson>(lessonsSql, new { Id = id });
                course.Lessons = lessons.ToList();
            }

            return course;
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("An error occurred while retrieving the course by ID.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while retrieving the course by ID.", ex);
        }
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
        try
        {
            return connection.QueryFirst<Course>(sql,
                new { Title = title, Description = description, CourseImgUrl = courseImgUrl });
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("An error occurred while adding a new course.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while adding a new course.", ex);
        }
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
        try
        {
            return connection.QueryFirstOrDefault<Course>(sql,
                new { Id = id, Title = title, Description = description, CourseImgUrl = courseImgUrl });
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("An error occurred while updating the course.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while updating the course.", ex);
        }
    }


    public void DeleteCourse(int id)
    {
        const string sql = @"
DELETE FROM learning_platform.courses
WHERE id = @Id;
";
        using var connection = _dataSource.OpenConnection();
        try
        {
            connection.Execute(sql, new { Id = id });
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("An error occurred while deleting the course.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while deleting the course.", ex);
        }
    }
}
