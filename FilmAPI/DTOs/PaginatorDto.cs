using System.Runtime.ConstrainedExecution;

namespace FilmAPI.DTOs
{
    public class PaginatorDto
    {
        public int Page { get; set; } = 1;
        public int numberOfRecordsPerPage  = 10;
        public readonly int maximumNumberOfRecordsPerPage = 50;
        public int NumberOfRecordsPerPage
        {
            get => numberOfRecordsPerPage;
            set => numberOfRecordsPerPage = (value > maximumNumberOfRecordsPerPage) ? maximumNumberOfRecordsPerPage : value;
        }
    }
}
