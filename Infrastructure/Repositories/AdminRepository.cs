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
    role as {nameof(User.Role)}
FROM learning_platform.users
";
        using var connection = _dataSource.OpenConnection();
        return connection.Query<User>(sql);
    }
    
    public User UpdateUser(int id, string fullname, string email, Role role)
    {
        if (!Enum.IsDefined(typeof(Role), role))
        {
            throw new ArgumentException("The role value is not valid.", nameof(role));
        }
        
        const string sql = @"
    UPDATE learning_platform.users
    SET full_name = @fullname, email = @email, role = @role
    WHERE id = @id RETURNING *";
        
        using var connection = _dataSource.OpenConnection();
        return connection.QuerySingle<User>(sql, new { id, fullname, email, role = role.ToString()});
    }
    
    

}