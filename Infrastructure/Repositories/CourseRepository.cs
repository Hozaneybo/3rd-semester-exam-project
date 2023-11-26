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

    public async Task<IEnumerable<Course>> GetAllCourses()
    {
        const string sql = @"SELECT * FROM learning_platform.courses;";
        
        using var connection = _dataSource.OpenConnection();
        return await connection.QueryAsync<Course>(sql);
    }

    public async Task<Course> GetCourseById(int id)
    {
        const string courseSql = @"
SELECT * FROM learning_platform.courses
WHERE id = @Id;
";

        const string lessonsSql = @"
SELECT * FROM learning_platform.lessons
WHERE course_id = @Id;
";

        using var connection = _dataSource.OpenConnection();
        var course = await connection.QuerySingleOrDefaultAsync<Course>(courseSql, new { Id = id });

        if (course != null)
        {
            var lessons = await connection.QueryAsync<Lesson>(lessonsSql, new { Id = id });
            course.Lessons = lessons.ToList();
        }

        return course;
    }


    public async Task<Course> AddCourse(string title, string description, string courseImgUrl)
    {
        const string sql = @"
INSERT INTO learning_platform.courses (title, description, course_img_url)
VALUES (@Title, @Description, @CourseImgUrl)
RETURNING *;
";
        using var connection = _dataSource.OpenConnection();
        return await connection.QueryFirstAsync<Course>(sql, new { Title = title, Description = description, CourseImgUrl = courseImgUrl });
    }

    public async Task<Course> UpdateCourse(int id, string title, string description, string courseImgUrl)
    {
        const string sql = @"
UPDATE learning_platform.courses
SET title = @Title, description = @Description, course_img_url = @CourseImgUrl
WHERE id = @Id
RETURNING *;
";
        using var connection = _dataSource.OpenConnection();
        return await connection.QueryFirstOrDefaultAsync<Course>(sql, new { Id = id, Title = title, Description = description, CourseImgUrl = courseImgUrl });
    }


    public async Task DeleteCourse(int id)
    {
        const string sql = @"
DELETE FROM learning_platform.courses
WHERE id = @Id;
";
        using var connection = _dataSource.OpenConnection();
        await connection.ExecuteAsync(sql, new { id });
    }
}
