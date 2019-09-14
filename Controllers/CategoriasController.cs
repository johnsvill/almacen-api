using AutoMapper;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriasController : ControllerBase
    {
        private readonly InventarioDBContext dBContext;
        private readonly IMapper mapper;    

        //Inyección de dependencia
        public CategoriasController(InventarioDBContext dBContext, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await this.dBContext.Categorias.Include("Productos").ToListAsync();
            var categoriasDTO = this.mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name ="GetCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await this.dBContext.Categorias.FirstOrDefaultAsync(x => x.CodigoCategoria == id);
            if(categoria == null)
            {
                return NotFound();
            }
            var categoriaDTO = this.mapper.Map<CategoriaDTO>(categoria);
            return categoriaDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaCreacionDTO categoriaCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var categoria = this.mapper.Map<Categoria>(categoriaCreacion);
            this.dBContext.Add(categoria);
            await this.dBContext.SaveChangesAsync();
            var categoriaDTO = this.mapper.Map<CategoriaDTO>(categoria);
            return new CreatedAtRouteResult("GetCategoria", new { id = categoria.CodigoCategoria }, categoriaDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaCreacionDTO categoriaActualizacion)
        {
            var categoria = this.mapper.Map<Categoria>(categoriaActualizacion);
            categoria.CodigoCategoria = id;
            this.dBContext.Entry(categoria).State = EntityState.Modified;
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var codigoCategoria = await this.dBContext.Categorias.Select(x => x.CodigoCategoria)
                .FirstOrDefaultAsync(x => x == id);
            if(codigoCategoria == default(int))
            {
                return NotFound();
            }
            this.dBContext.Remove(new Categoria { CodigoCategoria = id });
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
