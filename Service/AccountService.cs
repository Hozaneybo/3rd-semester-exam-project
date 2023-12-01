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
            if (isValid) return _accountRepository.GetById(passwordHash.UserId);
        }
        catch (Exception e)
        {
            _logger.LogError("Authenticate error: {Message}", e);
        }

        throw new InvalidCredentialException("Invalid credential!");
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
            _logger.LogError(ex, "Registration failed: {Message}", ex.Message);
            throw;
        }

    }
    public User? Get(SessionData data)
    {
        return _accountRepository.GetById(data.UserId);
    }
    
    public void GenerateAndSendEmailVerificationToken(User user)
    {
        var token = GenerateToken();
        var expiresAt = DateTime.UtcNow.AddHours(24);
        _accountRepository.SetEmailVerificationToken(user.Id, token, expiresAt);
        MailService.SendVerificationEmail(user.Email, token);
    }


    public bool VerifyEmailToken(string token)
    {
        var user = _accountRepository.GetUserByVerificationToken(token);
        if (user != null)
        {
            _accountRepository.VerifyEmail(user.Id);
            return true;
        }
        return false;
    }



    public void GenerateAndSendPasswordResetToken(User user)
    {
        var token = GenerateToken();
        var expiresAt = DateTime.UtcNow.AddHours(2);
        _accountRepository.SetPasswordResetToken(user.Id, token, expiresAt);
        MailService.SendPasswordResetEmail(user.Email, token);
    }

    public bool ResetPasswordWithToken(string token, string newPassword)
    {
        var user = _accountRepository.GetUserByPasswordResetToken(token);
        if (user != null)
        {
            var hashAlgorithm = PasswordHashAlgorithm.Create();
            var salt = hashAlgorithm.GenerateSalt();
            var hash = hashAlgorithm.HashPassword(newPassword, salt);
            _passwordHashRepository.Update(user.Id, hash, salt, hashAlgorithm.GetName());
            // Clear the reset token
            _accountRepository.SetPasswordResetToken(user.Id, null, null);
            return true;
        }
        return false;
    }

    private string GenerateToken()
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var randomBytes = new byte[32]; // 256 bits
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
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
            throw;
        }
    }
}