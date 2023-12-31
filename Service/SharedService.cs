﻿using Infrastructure.Interfaces;
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
        try 
        { 
            if (string.IsNullOrWhiteSpace(queryModel.SearchTerm)) 
                throw new ArgumentException("Search term cannot be empty."); 
            return _sharedRepository.Search(queryModel.SearchTerm);

        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            
            throw new Exception("An unexpected error occurred in the search service. Please try again.");
        }
    }


    public IEnumerable<User> GetUsersByRole(RoleQueryModel roleQueryModel)
    {
        try
        {
            return _sharedRepository.GetUsersByRole(roleQueryModel.Role);
        }
        catch (InvalidOperationException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred in the GetUsersByRole service. Please try again later.", ex);
        }
    }
}