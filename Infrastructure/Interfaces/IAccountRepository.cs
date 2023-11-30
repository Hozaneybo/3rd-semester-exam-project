using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IAccountRepository
{
    public User Create(string fullName, string email, string? avatarUrl);
    public User? GetById(int id);
    public void VerifyEmail(int userId);
    public void SetPasswordResetToken(int userId, string token, DateTime? expiresAt);
    public User GetUserByPasswordResetToken(string token);
    public void SetEmailVerificationToken(int userId, string token, DateTime expiresAt);
    public User GetUserByVerificationToken(string token);
    public User? GetUserByEmail(string email);

}