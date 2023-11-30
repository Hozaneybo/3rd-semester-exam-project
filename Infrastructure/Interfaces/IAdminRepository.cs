using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IAdminRepository
{
    public IEnumerable<User> GetAll();
    public User UpdateUser(int id, string fullname, string email, string? avatarUrl, Role role);
    public void DeleteUser(int id);

}