using Infrastructure.Interfaces;
using Infrastructure.Models;
using Service.CQ.Queries;

namespace Service;

public class SharedService
{
    private readonly ISharedRepository _sharedRepository;

    public SharedService(ISharedRepository sharedRepository)
    {
        _sharedRepository = sharedRepository;
    }

    public IEnumerable<SearchResult> Search(SearchQueryModel queryModel)
    {
        return _sharedRepository.Search(queryModel.SearchTerm);
    }


    public IEnumerable<User> GetUsersByRole(RoleQueryModel roleQueryModel)
    {
        return _sharedRepository.GetUsersByRole(roleQueryModel.Role);
    }
}