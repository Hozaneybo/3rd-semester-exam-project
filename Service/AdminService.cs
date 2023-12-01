using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Service.CQ.Commands;

namespace Service;
public class AdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly IAccountRepository _accountRepository;

    public AdminService(IAdminRepository adminRepository, IAccountRepository accountRepository )
    {
        _adminRepository = adminRepository;
        _accountRepository = accountRepository;
    }

    public IEnumerable<User> GetAll()
    {
        return _adminRepository.GetAll();
    }

    public User UpdateUser(UpdateUserCommand command)
    {
        return _adminRepository.UpdateUser(command.Id, command.FullName, command.Email, command.AvatarUrl, command.Role );
    }

    public User GetUserById(int id)
    {
        return _accountRepository.GetById(id);
    }

    public void DeleteUser(int id)
    {
        _adminRepository.DeleteUser(id);
    }
}