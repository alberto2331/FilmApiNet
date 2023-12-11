namespace FilmAPI.DTOs
{
    public class FilmDetailDto: FilmDto
    {
        public List<GenderDto> Genders { get; set; }
        public List<ActorDetailDto> Actors { get; set; }

    }
}
