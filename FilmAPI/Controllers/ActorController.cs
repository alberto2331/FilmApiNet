using AutoMapper;
using FilmAPI.DTOs;
using FilmAPI.Entities;
using FilmAPI.Helpers;
using FilmAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmAPI.Controllers
{
    [ApiController]
    [Route("api/actor")]
    public class ActorController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;
        private readonly string container = "actors"; //Folder name in azure

        public ActorController( ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDto>>> GetAll()
        {
            var entities = await context.Actors.ToListAsync();
            var actorsDtos = mapper.Map<List<ActorDto>>(entities);
            return actorsDtos;
        }

        [HttpGet("GetActorsByPaginator")]
        public async Task<ActionResult<List<ActorDto>>> GetActorPaginator([FromQuery] PaginatorDto paginatorDto)
        {
            var queryable = context.Actors.AsQueryable();
            await HttpContext.InsertParameterPaginator(queryable, paginatorDto.numberOfRecordsPerPage);
            var entities = await queryable.Paginator(paginatorDto).ToListAsync();
            return mapper.Map<List<ActorDto>>(entities);
        }

        [HttpGet("{id:int}", Name = "getActor")]
        public async Task<ActionResult<ActorDto>> GetById([FromRoute] int actorId)
        {
            var entity = await context.Actors.FirstOrDefaultAsync(g => g.Id == actorId);

            if (entity == null)
            {
                return NotFound();
            }
            var actorDto = mapper.Map<ActorDto>(entity);
            return actorDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorAddDto actorAddDto)
        {
            var actor = mapper.Map<Actor>(actorAddDto);
            if (actorAddDto.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    //Here instantiate a memory stream. This is used to extract an array of bytes from the IFormFile:
                    await actorAddDto.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();// "content" is an array of bytes. 
                    //"content" will be uploaded to azure File Storage
                    //Now we need the extension of the file:
                    var length = Path.GetExtension(actorAddDto.Photo.FileName); //
                    //Now add the photo to entity "Actor":
                    actor.Photo = await fileStorage.SaveFile(
                        content, length, container, actorAddDto.Photo.ContentType);//actor.Photo = This is a string, an url
                }
            }
            context.Add(actor);
            await context.SaveChangesAsync();

            var actorDto = mapper.Map<ActorDto>(actor);
            return new CreatedAtRouteResult("getActor", new { id = actorDto.Id }, actorDto); /*In quotes we
                                                                                                * put the name of the route in 
                                                                                                * which we can find the newly created actor*/
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDto> patchDocument)
        {
            if(patchDocument == null) //Client doesn't send nothing
            {
                return BadRequest();
            }
            var entity = await context.Actors.FirstOrDefaultAsync(actor => actor.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            var entityDto = mapper.Map<ActorPatchDto>(entity);
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorAddDto actorAddDto)
        {
            var actorDB = await context.Actors.FirstOrDefaultAsync(actor => actor.Id == id);
            if(actorDB == null) { return NotFound(); }
            actorDB = mapper.Map(actorAddDto, actorDB); //This line allows detect each field that changes 
                                                        // only the fields that are different in actorDB and actorAddDTO will be updated, the rest are not touched.
                                                        // This allows for greater efficiency
            if (actorAddDto.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorAddDto.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var length = Path.GetExtension(actorAddDto.Photo.FileName); 
                    actorDB.Photo = await fileStorage.EditFile(
                        content, length, container,
                        actorDB.Photo, 
                        actorAddDto.Photo.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteActor")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await context.Actors.AnyAsync(actor => actor.Id == id);

            if (!exists)
            {
                return NotFound("There isn´t actor with that Id");
            }

            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
