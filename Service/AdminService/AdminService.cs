using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Service.AdminService;
public class AdminService
{
    private readonly AdminRepository _repository;
    private readonly UserRepository _userRepository;

    public AdminService(AdminRepository repository, UserRepository userRepository )
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public IEnumerable<User> GetAll()
    {
        return _repository.GetAll();
    }

    public User UpdateUser(int id, string fullname, string email, Role role)
    {
        return _repository.UpdateUser(id, fullname, email, role );
    }

    public User GetUserById(int id)
    {
        return _userRepository.GetById(id);
    }
}