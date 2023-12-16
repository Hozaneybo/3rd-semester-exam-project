
using System.ComponentModel.DataAnnotations;

namespace Service.CQ.Queries;

public class UserQuery
{ 
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string? Email { get; set; }
}

