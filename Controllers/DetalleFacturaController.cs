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
    public class DetalleFacturaController : ControllerBase
    {
        private readonly InventarioDBContext contexto;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public DetalleFacturaController(InventarioDBContext contexto, IMapper mapper)
        {
            this.contexto = contexto;
            this.mapper = mapper;
        }
        //Método Asíncrono  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleFacturaDTO>>> Get()
        {
            var detalleFactura = await contexto.Categorias.ToListAsync();
            var detalleFacturaDTO = mapper.Map<List<DetalleFacturaDTO>>(detalleFactura);
            return detalleFacturaDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetDetalleFactura")]
        public async Task<ActionResult<DetalleFacturaDTO>> Get(int id)
        {
            var detalleFactura = await contexto.DetalleFacturas.FirstOrDefaultAsync(x => x.CodigoDetalle == id);
            if (detalleFactura == null)
            {
                return NotFound();
            }
            var detalleFacturaDTO = mapper.Map<DetalleFacturaDTO>(detalleFactura);
            return detalleFacturaDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DetalleFacturaCreacionDTO detalleFacturaCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var detalleFactura = mapper.Map<DetalleFactura>(detalleFacturaCreacion);
            contexto.Add(detalleFactura);
            await contexto.SaveChangesAsync();
            var detalleFacturaDTO = mapper.Map<DetalleFacturaDTO>(detalleFactura);
            return new CreatedAtRouteResult("GetDetalleFactura", new { id = detalleFactura.CodigoDetalle }, detalleFacturaDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] DetalleFacturaCreacionDTO detalleFacturaActualizacion)
        {
            var detalleFactura = mapper.Map<DetalleFactura>(detalleFacturaActualizacion);
            detalleFactura.CodigoDetalle = id;
            contexto.Entry(detalleFactura).State = EntityState.Modified;
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<DetalleFacturaDTO>> Delete(int id)
        {
            var detalleFactura = await contexto.DetalleFacturas.Select(x => x.CodigoDetalle)
                .FirstOrDefaultAsync(x => x == id);
            if (detalleFactura == default(int))
            {
                return NotFound();
            }
            contexto.Remove(new DetalleFactura { CodigoDetalle = id });
            await contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}
