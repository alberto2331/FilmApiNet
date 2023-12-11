using FilmAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace FilmAPI.DTOs
{
    public class ActorAddDto
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        
        [WeightFileValidation(maximumWeightInMegabits : 4)]
        [FileTypeValidator(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile Photo { get; set; }
    }
}
