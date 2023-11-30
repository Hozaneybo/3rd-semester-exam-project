using Infrastructure.Models;
using Infrastructure.Repositories;
using Service.CQ.Commands;

namespace Service.AdminService;
public class AdminService
{
    private readonly AdminRepository _repository;
    private readonly AccountRepository _accountRepository;

    public AdminService(AdminRepository repository, AccountRepository accountRepository )
    {
        _repository = repository;
        _accountRepository = accountRepository;
    }

    public IEnumerable<User> GetAll()
    {
        return _repository.GetAll();
    }

    public User UpdateUser(UpdateUserCommand command)
    {
        return _repository.UpdateUser(command.Id, command.FullName, command.Email, command.AvatarUrl, command.Role );
    }

    public User GetUserById(int id)
    {
        return _accountRepository.GetById(id);
    }

    public void DeleteUser(int id)
    {
        _repository.DeleteUser(id);
    }
}