using AutoMapper;
using FilmAPI.DTOs;
using FilmAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmAPI.Controllers
{
    [ApiController]
    [Route("api/genders")]
    public class GenderController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenderController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenderDto>>> GetAll()
        {
            var entities = await context.Genders.ToListAsync();
            var gendersDtos = mapper.Map<List<GenderDto>>(entities);
            return gendersDtos;
        }

        [HttpGet("{id:int}", Name = "getGender")]
        public async Task<ActionResult<GenderDto>> GetById([FromRoute] int genderId)
        {
            var entity = await context.Genders.FirstOrDefaultAsync(g => g.Id == genderId);

            if (entity == null)
            {
                return NotFound();
            }
            var genderDto = mapper.Map<GenderDto>(entity);
            return genderDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenderAddDto genderAddDto)
        {
            var gender = mapper.Map<Gender>(genderAddDto);
            context.Add(gender);
            await context.SaveChangesAsync();

            var genderDto = mapper.Map<GenderDto>(gender);
            return new CreatedAtRouteResult("getGender", new { id = genderDto.Id }, genderDto); /*In quotes we
                                                                                                * put the name of the route in 
                                                                                                * which we can find the newly created gender*/
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenderAddDto genderAddDto)
        {
            var entity = mapper.Map<Gender>(genderAddDto);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteGender")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await context.Genders.AnyAsync(gender => gender.Id == id);

            if (!exists)
            {
                return NotFound("There isn´t gender with that Id");
            }

            context.Remove(new Gender() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
           