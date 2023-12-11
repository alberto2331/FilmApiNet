using System.ComponentModel.DataAnnotations;

namespace FilmAPI.DTOs
{
    public class GenderAddDto
    {
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}
