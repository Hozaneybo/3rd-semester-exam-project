using System.Security.Authentication;
using System.Security.Cryptography;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Npgsql;
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
                _logger.LogWarning("Invalid credentials attempt for email: {Email}", command.Email);
                throw new InvalidCredentialException("Invalid credentials provided. Please check your email and password.");
            }
            return _accountRepository.GetById(passwordHash.UserId);
        }
        catch (InvalidCredentialException ex)
        {
            throw; 
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError(ex, "Database error during authentication for email: {Email}", command.Email);
            throw new AuthenticationException("A database error occurred during authentication. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during authentication for email: {Email}", command.Email);
            throw new AuthenticationException("An unexpected error occurred during authentication. Please try again later.");
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
            _logger.LogError("Registration failed: {Message}", ex.Message);
            throw;
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError("Database error during registration: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected error during registration: {Message}", ex.Message);
            throw;
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
        catch (NpgsqlException ex)
        {
            _logger.LogError("Database error retrieving user: {Message}", ex.Message);
            throw new InvalidOperationException("A database error occurred while retrieving user information. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected error retrieving user: {Message}", ex.Message);
            throw new Exception("An unexpected error occurred while retrieving user information. Please try again later.");
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
        catch (NpgsqlException ex)
        {
            _logger.LogError("Database error verifying email token: {Message}", ex.Message);
            throw new InvalidOperationException("A database error occurred while verifying the email token. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected error verifying email token: {Message}", ex.Message);
            throw new Exception("An unexpected error occurred while verifying the email token. Please try again later.");
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
        catch (NpgsqlException ex)
        {
            _logger.LogError("Database error resetting password with token: {Message}", ex.Message);
            throw new InvalidOperationException("A database error occurred while resetting the password. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected error resetting password with token: {Message}", ex.Message);
            throw new Exception("An unexpected error occurred while resetting the password. Please try again later.");
        }
    }

    public User? GetUserByEmail(UserQuery query)
    {
        try
        {
            return _accountRepository.GetUserByEmail(query.Email);
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError("Database error retrieving user by email: {Message}", ex.Message);
            throw new InvalidOperationException("A database error occurred while retrieving user information. Please try again later.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected error retrieving user by email: {Message}", ex.Message);
            throw new Exception("An unexpected error occurred while retrieving user information. Please try again later.");
        }
    }

    private string GenerateToken()
    {
        try
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[32];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        catch (CryptographicException ex)
        {
            _logger.LogError("Cryptographic error generating token: {Message}", ex.Message);
            throw new InvalidOperationException("An error occurred while generating a security token.");
        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected error generating token: {Message}", ex.Message);
            throw new Exception("An unexpected error occurred while generating a security token.");
        }
    }
    
    public User? UpdateUserProfile(SessionData data, UpdateProfileCommand command)
    {
        try
        {
            return _accountRepository.UpdateProfile(data.UserId, command.FullName, command.AvatarUrl);
        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating user: {Message}", ex.Message);
            throw new Exception("An error occurred while updating the profile.");
        }
    }

}
