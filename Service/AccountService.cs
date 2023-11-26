using System.Security.Authentication;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Service.PasswordService;

namespace Service;

public class AccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly PasswordHashRepository _passwordHashRepository;
    private readonly AccountRepository _accountRepository;

    public AccountService(ILogger<AccountService> logger, AccountRepository accountRepository,
        PasswordHashRepository passwordHashRepository)
    {
        _logger = logger;
        _accountRepository = accountRepository;
        _passwordHashRepository = passwordHashRepository;
    }

    public User? Authenticate(string email, string password)
    {
        try
        {
            var passwordHash = _passwordHashRepository.GetByEmail(email);
            var hashAlgorithm = PasswordHashAlgorithm.Create(passwordHash.Algorithm);
            var isValid = hashAlgorithm.VerifyHashedPassword(password, passwordHash.Hash, passwordHash.Salt);
            if (isValid) return _accountRepository.GetById(passwordHash.UserId);
        }
        catch (Exception e)
        {
            _logger.LogError("Authenticate error: {Message}", e);
        }

        throw new InvalidCredentialException("Invalid credential!");
    }

    public User Register(string fullName, string email, string password, string? avatarUrl)
    {
        try
        {
            var hashAlgorithm = PasswordHashAlgorithm.Create();
            var salt = hashAlgorithm.GenerateSalt();
            var hash = hashAlgorithm.HashPassword(password, salt);
            var user = _accountRepository.Create(fullName, email, avatarUrl);
            _passwordHashRepository.Create(user.Id, hash, salt, hashAlgorithm.GetName());
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
}