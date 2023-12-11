using Dapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    
    private readonly NpgsqlDataSource _dataSource;

    public AdminRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<User> GetAll()
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
";
        using var connection = _dataSource.OpenConnection();
        try
        {
            return connection.Query<User>(sql);
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving users.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while retrieving users.", ex);
        }
    }
    
    public User UpdateUser(int id, string fullname, string email, string? avatarUrl, Role role)
    {
        if (!Enum.IsDefined(typeof(Role), role))
        {
            throw new ArgumentException("The role value is not valid.", nameof(role));
        }
        
        const string sql = @"
UPDATE learning_platform.users
SET 
    full_name = COALESCE(@fullname, full_name),
    email = COALESCE(@email, email),
    avatar_url = COALESCE(@avatarUrl, avatar_url),
    role = COALESCE(@role::text, role)
WHERE id = @id 
RETURNING id, full_name, email, avatar_url, role, email_verified;";
        
        using var connection = _dataSource.OpenConnection();
        try
        {
            return connection.QuerySingle<User>(sql, new { id, fullname, email, avatarUrl, role = role.ToString()});
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException("An error occurred while updating the user.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while updating the user.", ex);
        }
    }
    
    public void DeleteUser(int id)
    {
        const string sql = @"DELETE FROM learning_platform.users WHERE id = @Id;";
        using (var connection = _dataSource.OpenConnection())
            try
            {
                connection.Execute(sql, new { Id = id });
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException("An error occurred while deleting the user.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the user.", ex);
            }
    }
}