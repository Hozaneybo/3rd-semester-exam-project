using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Service.CQ.Commands;

namespace Service;
public class AdminService
{
    private readonly ILogger<AdminService> _logger;
    private readonly IAdminRepository _adminRepository;
    private readonly IAccountRepository _accountRepository;

    public AdminService(ILogger<AdminService> logger, IAdminRepository adminRepository,
        IAccountRepository accountRepository)
    {
        _logger = logger;
        _adminRepository = adminRepository;
        _accountRepository = accountRepository;

    }

    public IEnumerable<User> GetAll()
    {
        try
        {
            return _adminRepository.GetAll();
        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving all users: {Message}", ex.Message);
            throw new Exception("An error occurred while retrieving users.");
        }
    }

    public User UpdateUser(UpdateUserCommand command)
    {
        try
        {
            return _adminRepository.UpdateUser(command.Id, command.FullName, command.Email, command.AvatarUrl, command.Role);
        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating user: {Message}", ex.Message);
            throw new Exception("An error occurred while updating the user.");
        }
    }

    public User GetUserById(int id)
    {
        try
        {
            return _accountRepository.GetById(id);
        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving user by ID: {Message}", ex.Message);
            throw new Exception("An error occurred while retrieving the user.");
        }
    }

    public void DeleteUser(int id)
    {
        try
        {
            _adminRepository.DeleteUser(id);
        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting user: {Message}", ex.Message);
            throw new Exception("An error occurred while deleting the user.");
        }
    }
}