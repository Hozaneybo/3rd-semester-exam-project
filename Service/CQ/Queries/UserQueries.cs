
using System.ComponentModel.DataAnnotations;

namespace Service.CQ.Queries;

public class UserQuery
{ 
    [EmailAddress]
    public string? Email { get; set; }
}

