using FilmAPI.Helpers;
using FilmAPI.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FilmAPI.DTOs
{
    public class FilmAddDto
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }
        public bool InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }

        [WeightFileValidation(maximumWeightInMegabits: 4)]
        [FileTypeValidator(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GendersIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorFilmAddDto>>))]
        public List<ActorFilmAddDto> Actors { get; set; }

    }
}
