using System.ComponentModel.DataAnnotations;

namespace FilmAPI.Entities
{
    public class Film
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }
        public bool InTheaters{ get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public List<FilmGender> FilmGender { get; set; }
        public List<FilmActor> FilmActor { get; set; }
        public List<FilmCinema> FilmCinema { get; set; }
    }
}
