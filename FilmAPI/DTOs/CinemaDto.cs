using FilmAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace FilmAPI.DTOs
{
    public class CinemaDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Length { get; set; }
    }
}
