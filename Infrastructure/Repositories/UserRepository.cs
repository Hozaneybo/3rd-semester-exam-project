using Dapper;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class UserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public User Create(string fullName, string email, string? avatarUrl)
    {
        const string sql = $@"
INSERT INTO learning_platform.users (full_name, email, avatar_url)
VALUES (@fullName, @email, @avatarUrl)
RETURNING
    id as {nameof(User.Id)},
    full_name as {nameof(User.Fullname)},
    email as {nameof(User.Email)},
    avatar_url as {nameof(User.AvatarUrl)};
"; 
        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.QueryFirst<User>(sql, new { fullName, email, avatarUrl });
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == "23505")
            {
                throw new InvalidOperationException("A user with this email address already exists.");
            }
            throw;
        }

    }

    public User? GetById(int id)
    {
        const string sql = $@"
SELECT
    id as {nameof(User.Id)},
    full_name as {nameof(User.Fullname)},
    email as {nameof(User.Email)},
    avatar_url as {nameof(User.AvatarUrl)},
    role as {nameof(User.Role)}
FROM learning_platform.users
WHERE id = @id;
";
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirstOrDefault<User>(sql, new { id });
    }

    public IEnumerable<User> GetAll()
    {
        const string sql = $@"
SELECT
    id as {nameof(User.Id)},
    full_name as {nameof(User.Fullname)},
    email as {nameof(User.Email)},
    avatar_url as {nameof(User.AvatarUrl)},
    role as {nameof(User.Role)}
FROM learning_platform.users
";
        using var connection = _dataSource.OpenConnection();
        return connection.Query<User>(sql);
    }
}