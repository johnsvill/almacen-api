using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly InventarioDBContext dBContext;
        private readonly IMapper mapper;

        //Inyección de dependencia  
        public FacturaController(InventarioDBContext dBContext, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturaDTO>>> Get()
        {
            var facturas = await this.dBContext.Categorias.ToListAsync();
            var facturasDTO = this.mapper.Map<List<FacturaDTO>>(facturas);
            return facturasDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetFactura")]
        public async Task<ActionResult<FacturaDTO>> Get(int id)
        {
            var factura = await this.dBContext.Facturas.FirstOrDefaultAsync(x => x.NumeroFactura == id);
            if (factura == null)
            {
                return NotFound();
            }
            var facturaDTO = this.mapper.Map<FacturaDTO>(factura);
            return facturaDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FacturaCreacionDTO facturaCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var factura = this.mapper.Map<Factura>(facturaCreacion);
            this.dBContext.Add(factura);
            await this.dBContext.SaveChangesAsync();
            var facturaDTO = this.mapper.Map<FacturaDTO>(factura);
            return new CreatedAtRouteResult("GetFactura", new { id = factura.NumeroFactura }, facturaDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] FacturaCreacionDTO facturaActualizacion)
        {
            var factura = this.mapper.Map<Factura>(facturaActualizacion);
            factura.NumeroFactura = id;
            this.dBContext.Entry(factura).State = EntityState.Modified;
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<FacturaDTO>> Delete(int id)
        {
            var codigoFactura = await this.dBContext.Facturas.Select(x => x.NumeroFactura)
                .FirstOrDefaultAsync(x => x == id);
            if (codigoFactura == default(int))
            {
                return NotFound();
            }
            this.dBContext.Remove(new Factura { NumeroFactura = id });
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }
    }  
}
