using Dapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class SharedRepository : ISharedRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public SharedRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<SearchResult> Search(string searchTerm)
    {
        const string sql = @"
    SELECT 'User' as Type, full_name as Term FROM learning_platform.users WHERE LOWER(full_name) LIKE LOWER(@SearchTerm)
    UNION
    SELECT 'Lesson' as Type, title as Term FROM learning_platform.lessons WHERE LOWER(title) LIKE LOWER(@SearchTerm)
    UNION
    SELECT 'Course' as Type, title as Term FROM learning_platform.courses WHERE LOWER(title) LIKE LOWER(@SearchTerm);";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.Query<SearchResult>(sql, new { SearchTerm = $"%{searchTerm}%" });
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException("Database operation failed. Please try again later.");
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while searching. Please try again later.");
        }

    }


    public IEnumerable<User> GetUsersByRole(Role role)
    {
        const string sql = $@"
SELECT
    id as {nameof(User.Id)},
    full_name as {nameof(User.Fullname)},
    email as {nameof(User.Email)},
    avatar_url as {nameof(User.AvatarUrl)},
    role as {nameof(User.Role)},
    email_verified as {nameof(User.EmailVerified)}
FROM learning_platform.users
WHERE role = @Role;
";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.Query<User>(sql, new { Role = role.ToString() });
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException("Database operation failed in GetUsersByRole. Please try again later.");
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred in GetUsersByRole. Please try again later.");
        }
    }
}