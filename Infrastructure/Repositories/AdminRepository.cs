using Dapper;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class AdminRepository
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
        return connection.Query<User>(sql);
    }
    
    public User UpdateUser(int id, string fullname, string email, string? avatarUrl, Role role)
    {
        if (!Enum.IsDefined(typeof(Role), role))
        {
            throw new ArgumentException("The role value is not valid.", nameof(role));
        }
        
        const string sql = @"
    UPDATE learning_platform.users
    SET full_name = @fullname, email = @email, avatar_url = @avatarUrl, role = @role
    WHERE id = @id RETURNING id, full_name, email, avatar_url, role, email_verified;";
        
        using var connection = _dataSource.OpenConnection();
        return connection.QuerySingle<User>(sql, new { id, fullname, email, avatarUrl, role = role.ToString()});
    }
    
    public async Task DeleteUser(int id)
    {
        const string sql = @"DELETE FROM learning_platform.users WHERE id = @Id;";
        using (var connection = _dataSource.OpenConnection())
        {
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
    
    

}