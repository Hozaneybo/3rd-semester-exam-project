using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface ISharedRepository
{
    IEnumerable<SearchResult> Search(string searchTerm);
    IEnumerable<User> GetUsersByRole(Role role);
}