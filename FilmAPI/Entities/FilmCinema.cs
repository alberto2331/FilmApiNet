namespace FilmAPI.Entities
{
    public class FilmCinema
    {
        public int FilmId { get; set; }
        public int CinemaId { get; set; }
        public Film Film { get; set; }
        public Cinema Cinema { get; set; }
    }
}
