namespace FilmAPI.DTOs
{
    public class MovieIndexDto
    {
        public List<FilmDto> NextReleases { get; set; }
        public List<FilmDto> MoviesInTheaters { get; set; }
    }
}
