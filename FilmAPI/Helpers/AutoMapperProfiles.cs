using AutoMapper;
using FilmAPI.DTOs;
using FilmAPI.Entities;
using NetTopologySuite.Geometries;

namespace FilmAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        
        public AutoMapperProfiles(GeometryFactory geometryFactory) 
        {
            CreateMap<Gender, GenderDto>().ReverseMap();            
            CreateMap<GenderAddDto, Gender>();
            
            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<ActorAddDto, Actor>()
                .ForMember(x=> x.Photo, options=> options.Ignore());
            CreateMap<ActorPatchDto, Actor>().ReverseMap();
            CreateMap<Film, FilmDto>().ReverseMap();
            CreateMap<FilmAddDto, Film>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.FilmGender, options => options.MapFrom(MapFilmGenders))
                .ForMember(x => x.FilmActor, options => options.MapFrom(MapFilmActors));
            CreateMap<Film, FilmDetailDto>()
                .ForMember(x => x.Genders, options => options.MapFrom(MapFilmGender))
                .ForMember(x => x.Actors, options => options.MapFrom(MapFilmActor));

            CreateMap<FilmPatchDto, Film>().ReverseMap();
            CreateMap<Cinema, CinemaDto>()
                .ForMember(y => y.Latitude, y => y.MapFrom(y => y.Location.Y))
                .ForMember(x => x.Length, x => x.MapFrom(x => x.Location.X));
            CreateMap<CinemaDto, Cinema>()
                .ForMember(x => x.Location, x => x.MapFrom(
                    y => geometryFactory.CreatePoint(new Coordinate(y.Length, y.Latitude))));
                
            CreateMap<CinemaAddDto, Cinema>()
                .ForMember(x => x.Location, x => x.MapFrom(
                    y => geometryFactory.CreatePoint(new Coordinate(y.Length, y.Latitude))));
        }

        private List<ActorDetailDto> MapFilmActor(Film film, FilmDetailDto filmDetailDto)
        {
            var result = new List<ActorDetailDto>();
            if (film.FilmActor == null) { return result; }
            foreach (var filmActor in film.FilmActor)
            {
                result.Add(new ActorDetailDto() { 
                    ActorId = filmActor.ActorId, 
                    Character = filmActor.Character, 
                    ActorName = filmActor.Actor.Name });
            }
            return result;
        }
        private List<GenderDto> MapFilmGender(Film film, FilmDetailDto filmDetailDto)
        {
            var result = new List<GenderDto>();
            if (film.FilmGender == null) { return result; }
            foreach (var filmGender in film.FilmGender)
            {
                result.Add(new GenderDto() { Id = filmGender.GenderId, Name = filmGender.Gender.Name });
            }
            return result;
        }
        private List<FilmGender> MapFilmGenders(FilmAddDto filmAddDto, Film film)
        {
            var result = new List<FilmGender>();
            if (filmAddDto.GendersIds == null) { return result; }
            foreach(var id in filmAddDto.GendersIds)
            {
                result.Add(new FilmGender() { GenderId = id });
            }
            return result;
        }

        private List<FilmActor> MapFilmActors(FilmAddDto filmAddDto, Film film)
        {
            var result = new List<FilmActor>();
            if(filmAddDto.Actors == null) { return result; }
            foreach(var actor in filmAddDto.Actors)
            {
                result.Add(new FilmActor() { ActorId = actor.ActorId , Character = actor.Character});
            }
            return result;
        }
    }
}
