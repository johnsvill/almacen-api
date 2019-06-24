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
    public class DetalleCompraController : ControllerBase
    {
        private readonly InventarioDBContext contexto;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public DetalleCompraController(InventarioDBContext contexto, IMapper mapper)
        {
            this.contexto = contexto;
            this.mapper = mapper;   
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleCompraDTO>>> Get()
        {
            var detalleCompras = await contexto.DetalleCompras.ToListAsync();
            var detalleComprasDTO = mapper.Map<List<DetalleCompraDTO>>(detalleCompras);
            return detalleComprasDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetDetalleCompra")]
        public async Task<ActionResult<DetalleCompraDTO>> Get(int id)
        {
            var detalleCompras = await contexto.DetalleCompras.FirstOrDefaultAsync(x => x.IdCompra == id);
            if (detalleCompras == null)
            {
                return NotFound();
            }
            var detalleComprasDTO = mapper.Map<DetalleCompraDTO>(detalleCompras);
            return detalleComprasDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DetalleCompraCreacionDTO detalleCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var detalleCompras = mapper.Map<DetalleCompra>(detalleCreacion);
            contexto.Add(detalleCompras);
            await contexto.SaveChangesAsync();
            var detalleComprasDTO = mapper.Map<DetalleCompraDTO>(detalleCompras);
            return new CreatedAtRouteResult("GetDetalleCompra", new { id = detalleCompras.IdCompra }, detalleCompras);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] DetalleCompraCreacionDTO detalleCompraActualizacion)
        {
            var detalleCompras = mapper.Map<DetalleCompra>(detalleCompraActualizacion);
            detalleCompras.IdCompra = id;
            contexto.Entry(detalleCompras).State = EntityState.Modified;
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<DetalleCompraDTO>> Delete(int id)
        {
            var detalleCompras = await contexto.DetalleCompras.Select(x => x.IdDetalle)
                .FirstOrDefaultAsync(x => x == id);
            if (detalleCompras == default(int))
            {
                return NotFound();
            }
            contexto.Remove(new DetalleCompra { IdDetalle = id });
            await contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}
