using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IAccountRepository
{
    /// <summary>
    /// Creates a new User object with the specified full name, email, and an optional avatar URL.
    /// </summary>
    /// <param name="fullName">The full name of the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="avatarUrl">The URL of the user's avatar image. This parameter is optional and can be null.</param>
    /// <returns>A User object representing the newly created user.</returns>
    public User Create(string fullName, string email, string? avatarUrl);

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user with the specified ID, or null if the user doesn't exist.</returns>
    public User? GetById(int id);

    /// <summary>
    /// Verifies the email for a user.
    /// </summary>
    /// <param name="userId">The ID of the user whose email needs to be verified.</param>
    public void VerifyEmail(int userId);

    /// <summary>
    /// Sets the password reset token for a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="token">The password reset token to be set.</param>
    /// <param name="expiresAt">The optional expiration date and time of the password reset token.</param>
    public void SetPasswordResetToken(int userId, string token, DateTime? expiresAt);

    /// <summary>
    /// Retrieves the user associated with the specified password reset token.
    /// </summary>
    /// <param name="token">The password reset token</param>
    /// <returns>The user associated with the password reset token</returns>
    public User GetUserByPasswordResetToken(string token);

    /// <summary>
    /// Sets the email verification token for a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="token">The email verification token.</param>
    /// <param name="expiresAt">The expiration date and time of the token.</param>
    public void SetEmailVerificationToken(int userId, string token, DateTime expiresAt);

    /// <summary>
    /// Retrieves the user with the specified verification token.
    /// </summary>
    /// <param name="token">The verification token of the user.</param>
    /// <returns>The user associated with the given verification token.</returns>
    public User GetUserByVerificationToken(string token);

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>The User object corresponding to the specified email address, or null if no user with that email address exists.</returns>
    public User? GetUserByEmail(string email);

    /// <summary>
    /// Updates the profile of a user.
    /// </summary>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="fullName">The full name of the user.</param>
    /// <param name="avatarUrl">The URL of the user's avatar.</param>
    /// <returns>The updated User object.</returns>
    User UpdateProfile(int userId, string fullName, string? avatarUrl);
}