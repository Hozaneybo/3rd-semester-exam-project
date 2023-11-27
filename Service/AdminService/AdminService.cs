using Infrastructure.Models;
using Infrastructure.Repositories;

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

    public User UpdateUser(int id, string fullname, string email, string avatarUrl, Role role)
    {
        return _repository.UpdateUser(id, fullname, email, avatarUrl, role );
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