namespace _3rd_semester_exam_project.DTOs
{
    public class SearchResultDto
    {
        public string Type { get; set; }
        public string Term { get; set; }
        

        public SearchResultDto()
        {
        }

        public SearchResultDto(string type, string term)
        {
            Type = type;
            Term = term;
        }
    }
}