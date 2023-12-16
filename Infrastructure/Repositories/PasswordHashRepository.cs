using Dapper;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class PasswordHashRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public PasswordHashRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public PasswordHash GetByEmail(string email)
    {
        try
        {
            const string sql = $@"
                SELECT 
                    user_id as {nameof(PasswordHash.UserId)},
                    hash as {nameof(PasswordHash.Hash)},
                    salt as {nameof(PasswordHash.Salt)},
                    algorithm as {nameof(PasswordHash.Algorithm)}
                FROM learning_platform.password_hash
                JOIN learning_platform.users ON learning_platform.password_hash.user_id = users.id
                WHERE email = @email;
                ";
            using var connection = _dataSource.OpenConnection();
            return connection.QuerySingle<PasswordHash>(sql, new { email });
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("An error occurred while accessing the database.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new Exception("The requested email does not exist or returns multiple entries.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred.", ex);
        }
    }

    public void Create(int userId, string hash, string salt, string algorithm)
    {
        try
        {
            const string sql = $@"
                INSERT INTO learning_platform.password_hash (user_id, hash, salt, algorithm)
                VALUES (@userId, @hash, @salt, @algorithm)
                ";
            using var connection = _dataSource.OpenConnection();
            connection.Execute(sql, new { userId, hash, salt, algorithm });
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("An error occurred while creating a new password hash.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred during the creation process.", ex);
        }
    }

    public void Update(int userId, string hash, string salt, string algorithm)
    {
        try
        {
            const string sql = $@"
                UPDATE learning_platform.password_hash
                SET hash = @hash, salt = @salt, algorithm = @algorithm
                WHERE user_id = @userId
                ";
            using var connection = _dataSource.OpenConnection();
            connection.Execute(sql, new { userId, hash, salt, algorithm });
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("An error occurred while updating the password hash.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred during the update process.", ex);
        }
    }
}