using AutoMapper;
using FilmAPI.DTOs;
using FilmAPI.Entities;
using FilmAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.ComponentModel;

namespace FilmAPI.Controllers
{
    [ApiController]
    [Route("api/cinema")]
    public class CinemaController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly GeometryFactory geometryFactory;

        public CinemaController(ApplicationDbContext context, IMapper mapper,
            GeometryFactory geometryFactory)
        {
            this.context = context;
            this.mapper = mapper;
            this.geometryFactory = geometryFactory;
        }

        [HttpGet]
        public async Task<ActionResult<List<CinemaDto>>> GetAll()
        {
            var cinemas = await context.Cinemas.ToListAsync();
            var cinemasDtos = mapper.Map<List<CinemaDto>>(cinemas);
            return cinemasDtos;
        }

        [HttpGet("{id:int}", Name = "getCinema")]
        public async Task<ActionResult<CinemaDto>> GetById([FromRoute] int id)
        {
            var entity = await context.Cinemas
                .FirstOrDefaultAsync(g => g.Id == id);

            if (entity == null)
            {
                return NotFound();
            }            
            return mapper.Map<CinemaDto>(entity);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromForm] CinemaAddDto cinemaAddDto)
        {
            var cinema = mapper.Map<Cinema>(cinemaAddDto);            
            context.Add(cinema);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("getCinema", new { id = cinema.Id }, cinemaAddDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] CinemaAddDto cinemaAddDto)
        {
            var entity = mapper.Map<Cinema>(cinemaAddDto);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("Nearby")]
        public async Task<ActionResult<List<CinemaNearbyDto>>> GetNearby(
            [FromQuery]CinemaNearbyFilterDto cinemaNearbyFilterDto)
        {
            //To get the userLocation we need Geometry Factory Class. Inyected in the contructor:
            var userLocation = geometryFactory.CreatePoint(
                new Coordinate(cinemaNearbyFilterDto.Length, cinemaNearbyFilterDto.Latitude));

            var cinemaList = await context.Cinemas
                .OrderBy(x => x.Location.Distance(userLocation))
                .Where(x => x.Location.IsWithinDistance(userLocation, cinemaNearbyFilterDto.distanceInKilometers * 1000))
                .Select(x => new CinemaNearbyDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Latitude = x.Location.Y,
                    Length = x.Location.X,
                    DistanceInMeters = Math.Round(x.Location.Distance(userLocation))
                }).ToListAsync();
            return cinemaList;
        }
    }
}
