using System.Runtime.ConstrainedExecution;

namespace FilmAPI.DTOs
{
    public class FilmFilterDto
    {
        public int Page { get; set; } = 1;
        public int NumberOfRecordsPerPage { get; set; } = 10;
        public PaginatorDto Paginator 
        { 
            get { return new PaginatorDto() { Page = Page, NumberOfRecordsPerPage = NumberOfRecordsPerPage }; }
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool InTheaters { get; set; }        
        public bool CommigSoon { get; set; }
        public int GenderId { get; set; }
        public string SortBy { get; set; }
        public bool Asc { get; set; } = true;
    }
}
