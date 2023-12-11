using AutoMapper;
using FilmAPI.DTOs;
using FilmAPI.Entities;
using FilmAPI.Helpers;
using FilmAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;

namespace FilmAPI.Controllers
{
    [ApiController]
    [Route("api/film")]
    public class FilmController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;
        private readonly string container = "films"; //Folder name in azure

        public FilmController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }
        
        [HttpGet]
        public async Task<ActionResult<MovieIndexDto>> GetAll()
        {
            var top = 5;
            var today = DateTime.Now;
            
            var nextReleases = await context.Films
                .Where(x=> x.ReleaseDate > today)
                .Take(top)
                .ToArrayAsync();
            var moviesInTheaters = await context.Films
                .Where(x => x.InTheaters == true)
                .ToArrayAsync();

            var result = new MovieIndexDto();
            result.NextReleases = mapper.Map<List<FilmDto>>(nextReleases);
            result.MoviesInTheaters = mapper.Map<List<FilmDto>>(moviesInTheaters);
            return result;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<FilmDto>>> GetByFilter([FromQuery] FilmFilterDto filmFilterDto)
        {
            var filmQueryable = context.Films.AsQueryable();
            if (!string.IsNullOrEmpty(filmFilterDto.Title))
            {
                filmQueryable = filmQueryable.Where(x => x.Title.Contains(filmFilterDto.Title));
            }
            if (filmFilterDto.InTheaters )
            {
                filmQueryable = filmQueryable.Where(x => x.InTheaters);
            }
            if (filmFilterDto.CommigSoon)
            {
                var today = DateTime.Now;
                filmQueryable = filmQueryable.Where(x => x.ReleaseDate > today);
            }

            if (filmFilterDto.GenderId != 0)
            {                
                filmQueryable = filmQueryable.Where(x => x.FilmGender.Select(y => y.GenderId).Contains(filmFilterDto.GenderId));
            
            }

            if (!string.IsNullOrEmpty(filmFilterDto.SortBy))
            {
                var order = filmFilterDto.Asc ? "asc" : "desc";
                filmQueryable = filmQueryable.OrderBy($"{filmFilterDto.SortBy} {order}");
            }

            await HttpContext.InsertParameterPaginator(filmQueryable, filmFilterDto.NumberOfRecordsPerPage);
            var films = await filmQueryable.Paginator(filmFilterDto.Paginator).ToListAsync();
            
            return mapper.Map<List<FilmDto>>(films);
        }

        [HttpGet("{id:int}", Name = "getFilm")]
        public async Task<ActionResult<FilmDetailDto>> GetById([FromRoute] int id)
        {
            var entity = await context.Films                
                .Include(x => x.FilmActor).ThenInclude(x=> x.Actor)
                .Include(x => x.FilmGender).ThenInclude(x => x.Gender)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (entity == null)
            {
                return NotFound();
            }
            entity.FilmActor = entity.FilmActor.OrderBy(x=> x.Order).ToList(); //To make sure about the order of actors
            return mapper.Map<FilmDetailDto>(entity);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] FilmAddDto filmAddDto)
        {
            var film = mapper.Map<Film>(filmAddDto);
            if (filmAddDto.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await filmAddDto.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var length = Path.GetExtension(filmAddDto.Poster.FileName); 
                    film.Poster = await fileStorage.SaveFile(
                        content, length, container, filmAddDto.Poster.ContentType);
                }
            }
            orderOfActors(film);
            context.Add(film);
            await context.SaveChangesAsync();

            var filmDto = mapper.Map<FilmDto>(film);
            return new CreatedAtRouteResult("getFilm", new { id = filmDto.Id }, filmDto); 
        }

        private void orderOfActors(Film film)
        {
            if (film.FilmActor !=null)
            {
                for (int i=0 ; i < film.FilmActor.Count; i++)
                {
                    film.FilmActor[i].Order = i;
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] FilmAddDto filmAddDto)
        {
            var filmDB = await context.Films
                .Include(x => x.FilmActor)
                .Include( x => x.FilmGender)
                .FirstOrDefaultAsync(film => film.Id == id);
            if (filmDB == null) { return NotFound(); }
            filmDB = mapper.Map(filmAddDto, filmDB);

            if (filmAddDto.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await filmAddDto.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var length = Path.GetExtension(filmAddDto.Poster.FileName);
                    filmDB.Poster = await fileStorage.EditFile(
                        content, length, container,
                        filmDB.Poster,
                        filmAddDto.Poster.ContentType);
                }
            }
            orderOfActors(filmDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<FilmPatchDto> patchDocument)
        {
            if (patchDocument == null) //Client doesn't send nothing
            {
                return BadRequest();
            }
            var entity = await context.Films.FirstOrDefaultAsync(film => film.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            var entityDto = mapper.Map<FilmPatchDto>(entity);
            patchDocument.ApplyTo(entityDto, ModelState);

            var isValid = TryValidateModel(entityDto);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(entityDto, entity);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteFilm")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await context.Films.AnyAsync(film => film.Id == id);

            if (!exists)
            {
                return NotFound("There isn´t film with that Id");
            }

            context.Remove(new Film() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
