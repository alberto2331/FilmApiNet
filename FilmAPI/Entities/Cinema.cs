using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace FilmAPI.Entities
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public List<FilmCinema> FilmCinema { get; set; }
        public Point Location { get; set; }
    }
}
