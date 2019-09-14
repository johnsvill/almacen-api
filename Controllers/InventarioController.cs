using AutoMapper;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly InventarioDBContext dBContext;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public InventarioController(InventarioDBContext dBContext, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventarioDTO>>> Get()
        {
            var inventario = await this.dBContext.Inventarios.ToListAsync();
            var inventarioDTO = this.mapper.Map<List<InventarioDTO>>(inventario);
            return inventarioDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name ="GetInventario")]
        public async Task<ActionResult<InventarioDTO>> Get(int id)
        {
            var inventario = await this.dBContext.Inventarios.FirstOrDefaultAsync(x => x.CodigoInventario == id);
            if(inventario == null)
            {
                return NotFound();
            }
            var inventarioDTO = this.mapper.Map<InventarioDTO>(inventario);
            return inventarioDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InventarioCreacionDTO inventarioCreacion)//SE espera recibir un JSON o un DOC.XML
        {
            var inventario = this.mapper.Map<Inventario>(inventarioCreacion);
            this.dBContext.Add(inventario);
            await this.dBContext.SaveChangesAsync();
            var inventarioDTO = this.mapper.Map<InventarioDTO>(inventario);
            return new CreatedAtRouteResult("GetInventario", new { id = inventario.CodigoInventario }, inventarioDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] InventarioCreacionDTO inventarioActualizacion)
        {
            var inventario = this.mapper.Map<Inventario>(inventarioActualizacion);
            inventario.CodigoInventario = id;
            this.dBContext.Entry(inventario).State = EntityState.Modified;
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<InventarioDTO>> Delete(int id)
        {
            var codigoInventario = await this.dBContext.Inventarios.Select(x => x.CodigoInventario)
                .FirstOrDefaultAsync(x => x == id);
            if (codigoInventario == default(int))
            {
                return NotFound();
            }
            this.dBContext.Remove(new Inventario { CodigoInventario = id });
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
