namespace Infrastructure.Models;

public class SearchResult
{
    public string Type { get; set; }
    public string Term { get; set; } 

    public SearchResult(string type,  string term)
    {
        Type = type;
        Term = term;
    }
}