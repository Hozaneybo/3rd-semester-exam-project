using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface ISharedRepository
{
    /// <summary>
    /// Searches for a given search term and returns a collection of search results.
    /// </summary>
    /// <param name="searchTerm">The term to search for.</param>
    /// <returns>A collection of search results matching the search term.</returns>
    IEnumerable<SearchResult> Search(string searchTerm);

    /// <summary>
    /// Retrieves a collection of users by their role.
    /// </summary>
    /// <param name="role">The role of the users to retrieve.</param>
    /// <returns>A collection of users matching the specified role.</returns>
    IEnumerable<User> GetUsersByRole(Role role);
}