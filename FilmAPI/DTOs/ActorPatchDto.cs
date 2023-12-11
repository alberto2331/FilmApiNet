using System.ComponentModel.DataAnnotations;

namespace FilmAPI.DTOs
{
    public class ActorPatchDto
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
