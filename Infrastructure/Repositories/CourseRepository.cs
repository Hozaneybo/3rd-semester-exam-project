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
        const string sql = @"
SELECT id, title, description
FROM learning_platform.courses;
";
        using var connection = _dataSource.OpenConnection();
        return await connection.QueryAsync<Course>(sql);
    }

    public async Task<Course> GetCourseById(int id)
    {
        const string sql = @"
SELECT id, title, description
FROM learning_platform.courses
WHERE id = @id;
";
        using var connection = _dataSource.OpenConnection();
        return await connection.QuerySingleOrDefaultAsync<Course>(sql, new { id });
    }

    public async Task<Course> AddCourse(Course course)
    {
        const string sql = @"
INSERT INTO learning_platform.courses (title, description)
VALUES (@Title, @Description)
RETURNING *;
";
        using var connection = _dataSource.OpenConnection();
        return await connection.QueryFirstAsync<Course>(sql, course);
    }

    public async Task<Course> UpdateCourse(Course course)
    {
        const string sql = @"
UPDATE learning_platform.courses
SET title = @Title, description = @Description
WHERE id = @Id
RETURNING id, title, description;
";
        using var connection = _dataSource.OpenConnection();
        return await connection.QueryFirstOrDefaultAsync<Course>(sql, course);
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
