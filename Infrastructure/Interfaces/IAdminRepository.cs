using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IAdminRepository
{
    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>All users as an IEnumerable<User> object.</returns>
    public IEnumerable<User> GetAll();

    /// <summary>
    /// Updates the details of a user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="fullname">The new full name of the user.</param>
    /// <param name="email">The new email of the user.</param>
    /// <param name="avatarUrl">The new avatar URL of the user (optional).</param>
    /// <param name="role">The new role of the user.</param>
    /// <returns>The updated User object.</returns>
    public User UpdateUser(int id, string fullname, string email, string? avatarUrl, Role role);

    /// <summary>
    /// Deletes the user with the specified id.
    /// </summary>
    /// <param name="id">The id of the user to delete.</param>
    public void DeleteUser(int id);
}