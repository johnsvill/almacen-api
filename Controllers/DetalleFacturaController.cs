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
        private readonly InventarioDBContext dBContext;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public DetalleFacturaController(InventarioDBContext dBContext, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
        }
        //Método Asíncrono  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleFacturaDTO>>> Get()
        {
            var detalleFactura = await this.dBContext.Categorias.ToListAsync();
            var detalleFacturaDTO = this.mapper.Map<List<DetalleFacturaDTO>>(detalleFactura);
            return detalleFacturaDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetDetalleFactura")]
        public async Task<ActionResult<DetalleFacturaDTO>> Get(int id)
        {
            var detalleFactura = await this.dBContext.DetalleFacturas.FirstOrDefaultAsync(x => x.CodigoDetalle == id);
            if (detalleFactura == null)
            {
                return NotFound();
            }
            var detalleFacturaDTO = this.mapper.Map<DetalleFacturaDTO>(detalleFactura);
            return detalleFacturaDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DetalleFacturaCreacionDTO detalleFacturaCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var detalleFactura = this.mapper.Map<DetalleFactura>(detalleFacturaCreacion);
            this.dBContext.Add(detalleFactura);
            await this.dBContext.SaveChangesAsync();
            var detalleFacturaDTO = this.mapper.Map<DetalleFacturaDTO>(detalleFactura);
            return new CreatedAtRouteResult("GetDetalleFactura", new { id = detalleFactura.CodigoDetalle }, detalleFacturaDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] DetalleFacturaCreacionDTO detalleFacturaActualizacion)
        {
            var detalleFactura = this.mapper.Map<DetalleFactura>(detalleFacturaActualizacion);
            detalleFactura.CodigoDetalle = id;
            this.dBContext.Entry(detalleFactura).State = EntityState.Modified;
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<DetalleFacturaDTO>> Delete(int id)
        {
            var detalleFactura = await this.dBContext.DetalleFacturas.Select(x => x.CodigoDetalle)
                .FirstOrDefaultAsync(x => x == id);
            if (detalleFactura == default(int))
            {
                return NotFound();
            }
            this.dBContext.Remove(new DetalleFactura { CodigoDetalle = id });
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
