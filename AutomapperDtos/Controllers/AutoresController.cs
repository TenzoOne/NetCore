using AutoMapper;
using AutomapperDtos.Context;
using AutomapperDtos.Entities;
using AutomapperDtos.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomapperDtos.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]

    public class AutoresController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get()
        {
            var autores = await context.Autores.Include(x => x.Books).ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id}", Name = "ObtenerAutor")] //ObtenerAutor es la regla de ruteo
        public async Task<ActionResult<AutorDTO>> Get(int id)
        {
            var autor = await context.Autores.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            return this.mapper.Map<AutorDTO>(autor);

        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacion)
        {
            var autor = mapper.Map<Autor>(autorCreacion);
            context.Add(autor);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, mapper.Map<AutorDTO>(autor));
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Autor autor)
        {
            context.Entry(autor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }


        //api/autores/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AutorCreacionDTO autorActualizacion)
        {
            var autor = mapper.Map<Autor>(autorActualizacion);
            autor.Id = id;
            context.Entry(autor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        //autolizacion parcial PATCH
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<AutorCreacionDTO> patchdocument)
        {
            if (patchdocument == null)
            {
                return BadRequest();
            }

            var autordelaBD = await context.Autores.FirstOrDefaultAsync(x => x.Id == id) ;

            if (autordelaBD == null)
            {
                return NotFound();
            }
            var autorDTO = mapper.Map<AutorCreacionDTO>(autordelaBD);


            patchdocument.ApplyTo(autorDTO, ModelState);

            mapper.Map(autorDTO,autordelaBD);

            var isvalid = TryValidateModel(autordelaBD);

            if (!isvalid)
            {
                return BadRequest(ModelState);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
