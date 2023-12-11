using System.ComponentModel.DataAnnotations;

namespace FilmAPI.DTOs
{
    public class FilmPatchDto
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
