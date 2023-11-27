using Dapper;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class AccountRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public AccountRepository(NpgsqlDataSource dataSource)
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

    public void VerifyEmail(int userId)
    {
        const string sql = @"
UPDATE learning_platform.users
SET email_verified = TRUE, email_verification_token = NULL, email_token_expires_at = NULL
WHERE id = @UserId;
";
        using var connection = _dataSource.OpenConnection();
        connection.Execute(sql, new { UserId = userId });
    }

    public void SetPasswordResetToken(int userId, string token, DateTime? expiresAt)
    {
        const string sql = @"
UPDATE learning_platform.users
SET password_reset_token = @Token, password_reset_token_expires_at = @ExpiresAt
WHERE id = @UserId;
";
        using var connection = _dataSource.OpenConnection();
        connection.Execute(sql, new { UserId = userId, Token = token, ExpiresAt = expiresAt });
    }

    public User GetUserByPasswordResetToken(string token)
    {
        const string sql = @"
SELECT * FROM learning_platform.users
WHERE password_reset_token = @Token AND password_reset_token_expires_at > CURRENT_TIMESTAMP;
";
        using var connection = _dataSource.OpenConnection();
        return connection.QuerySingleOrDefault<User>(sql, new { Token = token });
    }
    
    
    public void SetEmailVerificationToken(int userId, string token, DateTime expiresAt)
    {

        const string sql = @"
UPDATE learning_platform.users
SET email_verification_token = @Token, email_token_expires_at = @ExpiresAt
WHERE id = @UserId;
";
        using var connection = _dataSource.OpenConnection();
        connection.Execute(sql, new { UserId = userId, Token = token, ExpiresAt = expiresAt });
    }


    public User GetUserByVerificationToken(string token)
    {
        const string sql = @"
SELECT * FROM learning_platform.users
WHERE email_verification_token = @Token AND email_token_expires_at > CURRENT_TIMESTAMP;
";
        using var connection = _dataSource.OpenConnection();
        return connection.QuerySingleOrDefault<User>(sql, new { Token = token });
    }
    
    public User? GetUserByEmail(string email)
    {
        const string sql = $@"
SELECT
    id as {nameof(User.Id)},
    full_name as {nameof(User.Fullname)},
    email as {nameof(User.Email)},
    avatar_url as {nameof(User.AvatarUrl)},
    role as {nameof(User.Role)}
FROM learning_platform.users
WHERE email = @Email;
";
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirstOrDefault<User>(sql, new { Email = email }); 
    }
    
  
}