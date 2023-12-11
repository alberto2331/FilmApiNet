using System.ComponentModel.DataAnnotations;

namespace FilmAPI.DTOs
{
    public class CinemaNearbyFilterDto
    {
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Length { get; set; }

        public int distanceInKilometers = 10; 
        public readonly int maximumDistanceInKilometers = 50;
        public int MaximumDistanceInKilometers
        {
            get => distanceInKilometers;
            set => distanceInKilometers = (value > maximumDistanceInKilometers) ? maximumDistanceInKilometers : value;
        }
    }
}
