using Dapper;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class SharedRepository
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

        using var connection = _dataSource.OpenConnection();
        return connection.Query<SearchResult>(sql, new { SearchTerm = $"%{searchTerm}%" });
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

        using var connection = _dataSource.OpenConnection();
        return connection.Query<User>(sql, new { Role = role.ToString() });

    }

}