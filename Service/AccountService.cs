using System.Security.Authentication;
using System.Security.Cryptography;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Service.CQ.Commands;
using Service.CQ.Queries;
using Service.PasswordService;

namespace Service;

public class AccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly PasswordHashRepository _passwordHashRepository;
    private readonly IAccountRepository _accountRepository;

    public AccountService(ILogger<AccountService> logger, IAccountRepository accountRepository,
        PasswordHashRepository passwordHashRepository)
    {
        _logger = logger;
        _accountRepository = accountRepository;
        _passwordHashRepository = passwordHashRepository;
    }

    public User? Authenticate(UserLoginCommand command)
    {
        try
        {
            var passwordHash = _passwordHashRepository.GetByEmail(command.Email);
            var hashAlgorithm = PasswordHashAlgorithm.Create(passwordHash.Algorithm);
            var isValid = hashAlgorithm.VerifyHashedPassword(command.Password, passwordHash.Hash, passwordHash.Salt);
            if (!isValid)
            {
                throw new InvalidCredentialException("Invalid credentials provided.");
            }
            return _accountRepository.GetById(passwordHash.UserId);
        }
        catch (InvalidCredentialException ex)
        {
            _logger.LogError("Authentication failed: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error during authentication: {Message}", ex.Message);
            throw new AuthenticationException("An error occurred during authentication.");
        }
    }

    public User Register(CreateUserCommand command)
    {
        try
        {
            var hashAlgorithm = PasswordHashAlgorithm.Create();
            var salt = hashAlgorithm.GenerateSalt();
            var hash = hashAlgorithm.HashPassword(command.Password, salt);
            var user = _accountRepository.Create(command.FullName, command.Email, command.AvatarUrl);
            _passwordHashRepository.Create(user.Id, hash, salt, hashAlgorithm.GetName());
            
            GenerateAndSendEmailVerificationToken(user);
            
            return user;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Registration failed due to invalid operation: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected error during registration: {Message}", ex.Message);
            throw new Exception("An error occurred during user registration.");
        }
    }

    public User? Get(SessionData data)
    {
        try
        {
            return _accountRepository.GetById(data.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving user: {Message}", ex.Message);
            throw new Exception("An error occurred while retrieving user information.");
        }
    }

    public void GenerateAndSendEmailVerificationToken(User user)
    {
        try
        {
            var token = GenerateToken();
            var expiresAt = DateTime.UtcNow.AddHours(72);
            _accountRepository.SetEmailVerificationToken(user.Id, token, expiresAt);
            MailService.SendVerificationEmail(user.Email, token);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error generating or sending email verification token: {Message}", ex.Message);
            throw new Exception("An error occurred while processing email verification.");
        }
    }

    public bool VerifyEmailToken(string token)
    {
        try
        {
            var user = _accountRepository.GetUserByVerificationToken(token);
            if (user != null)
            {
                _accountRepository.VerifyEmail(user.Id);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error verifying email token: {Message}", ex.Message);
            throw new Exception("An error occurred while verifying the email token.");
        }
    }

    public void GenerateAndSendPasswordResetToken(User user)
    {
        try
        {
            var token = GenerateToken();
            var expiresAt = DateTime.UtcNow.AddHours(4);
            _accountRepository.SetPasswordResetToken(user.Id, token, expiresAt);
             MailService.SendPasswordResetEmail(user.Email, token);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error generating or sending password reset token: {Message}", ex.Message);
            throw new Exception("An error occurred while processing password reset.");
        }
    }

    public bool ResetPasswordWithToken(string token, string newPassword)
    {
        try
        {
            var user = _accountRepository.GetUserByPasswordResetToken(token);
            if (user != null)
            {
                var hashAlgorithm = PasswordHashAlgorithm.Create();
                var salt = hashAlgorithm.GenerateSalt();
                var hash = hashAlgorithm.HashPassword(newPassword, salt);
                _passwordHashRepository.Update(user.Id, hash, salt, hashAlgorithm.GetName());
                _accountRepository.SetPasswordResetToken(user.Id, null, null);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error resetting password with token: {Message}", ex.Message);
            throw new Exception("An error occurred while resetting the password.");
        }
    }

    public User? GetUserByEmail(UserQuery query)
    {
        try
        {
            return _accountRepository.GetUserByEmail(query.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving user by email: {Message}", ex.Message);
            throw new Exception("An error occurred while retrieving user by email.");
        }
    }

    private string GenerateToken()
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var randomBytes = new byte[32];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
