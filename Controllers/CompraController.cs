using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly InventarioDBContext contexto;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public CompraController(InventarioDBContext contexto, IMapper mapper)
        {
            this.contexto = contexto;
            this.mapper = mapper;
        }   
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompraDTO>>> Get()
        {
            var compra = await contexto.Categorias.ToListAsync();
            var compraDTO = mapper.Map<List<CompraDTO>>(compra);
            return compraDTO;
        }

        [HttpGet("{id}", Name = "GetCompra")]
        public async Task<ActionResult<CompraDTO>> Get(int id)
        {
            var compra = await contexto.Compras.FirstOrDefaultAsync(x => x.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }
            var compraDTO = mapper.Map<CompraDTO>(compra);
            return compraDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CompraCreacionDTO compraCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var compra = mapper.Map<Compra>(compraCreacion);
            contexto.Add(compra);
            await contexto.SaveChangesAsync();
            var compraDTO = mapper.Map<CompraDTO>(compra);
            return new CreatedAtRouteResult("GetCompra", new { id = compra.IdCompra }, compraDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CompraCreacionDTO compraActualizacion)
        {
            var compra = mapper.Map<Compra>(compraActualizacion);
            compra.IdCompra = id;
            contexto.Entry(compra).State = EntityState.Modified;
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CompraDTO>> Delete(int id)
        {
            var compra = await contexto.Compras.Select(x => x.IdCompra)
                .FirstOrDefaultAsync(x => x == id);
            if (compra == default(int))
            {
                return NotFound();
            }
            contexto.Remove(new Compra { IdCompra = id });
            await contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}
