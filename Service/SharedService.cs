using Infrastructure.Models;
using Infrastructure.Repositories;
using Service.CQ.Queries;

namespace Service;

public class SharedService
{
    private readonly SharedRepository _sharedRepository;

    public SharedService(SharedRepository sharedRepository)
    {
        _sharedRepository = sharedRepository;
    }

    public IEnumerable<SearchResult> Search(string searchTerm)
    {
        return _sharedRepository.Search(searchTerm);
    }

    public IEnumerable<User> GetUsersByRole(RoleQueryModel roleQueryModel)
    {
        return _sharedRepository.GetUsersByRole(roleQueryModel.Role);
    }
}