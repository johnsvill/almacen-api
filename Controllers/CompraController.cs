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
        private readonly InventarioDBContext dBContext;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public CompraController(InventarioDBContext dBContext, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
        }   
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompraDTO>>> Get()
        {
            var compra = await this.dBContext.Categorias.ToListAsync();
            var compraDTO = this.mapper.Map<List<CompraDTO>>(compra);
            return compraDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetCompra")]
        public async Task<ActionResult<CompraDTO>> Get(int id)
        {
            var compra = await this.dBContext.Compras.FirstOrDefaultAsync(x => x.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }
            var compraDTO = this.mapper.Map<CompraDTO>(compra);
            return compraDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CompraCreacionDTO compraCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var compra = this.mapper.Map<Compra>(compraCreacion);
            this.dBContext.Add(compra);
            await this.dBContext.SaveChangesAsync();
            var compraDTO = this.mapper.Map<CompraDTO>(compra);
            return new CreatedAtRouteResult("GetCompra", new { id = compra.IdCompra }, compraDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CompraCreacionDTO compraActualizacion)
        {
            var compra = this.mapper.Map<Compra>(compraActualizacion);
            compra.IdCompra = id;
            this.dBContext.Entry(compra).State = EntityState.Modified;
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<CompraDTO>> Delete(int id)
        {
            var compra = await this.dBContext.Compras.Select(x => x.IdCompra)
                .FirstOrDefaultAsync(x => x == id);
            if (compra == default(int))
            {
                return NotFound();
            }
            this.dBContext.Remove(new Compra { IdCompra = id });
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
