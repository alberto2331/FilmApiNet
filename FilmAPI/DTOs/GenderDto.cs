using System.ComponentModel.DataAnnotations;

namespace FilmAPI.DTOs
{
    public class GenderDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}
