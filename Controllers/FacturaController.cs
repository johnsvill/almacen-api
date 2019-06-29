﻿using AutoMapper;
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
        private readonly InventarioDBContext contexto;
        private readonly IMapper mapper;

        //Inyección de dependencia  
        public FacturaController(InventarioDBContext contexto, IMapper mapper)
        {
            this.contexto = contexto;
            this.mapper = mapper;
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturaDTO>>> Get()
        {
            var facturas = await contexto.Categorias.ToListAsync();
            var facturasDTO = mapper.Map<List<FacturaDTO>>(facturas);
            return facturasDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetFactura")]
        public async Task<ActionResult<FacturaDTO>> Get(int id)
        {
            var factura = await contexto.Facturas.FirstOrDefaultAsync(x => x.NumeroFactura == id);
            if (factura == null)
            {
                return NotFound();
            }
            var facturaDTO = mapper.Map<FacturaDTO>(factura);
            return facturaDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FacturaCreacionDTO facturaCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var factura = mapper.Map<Factura>(facturaCreacion);
            contexto.Add(factura);
            await contexto.SaveChangesAsync();
            var facturaDTO = mapper.Map<FacturaDTO>(factura);
            return new CreatedAtRouteResult("GetFactura", new { id = factura.NumeroFactura }, facturaDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] FacturaCreacionDTO facturaActualizacion)
        {
            var factura = mapper.Map<Factura>(facturaActualizacion);
            factura.NumeroFactura = id;
            contexto.Entry(factura).State = EntityState.Modified;
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<FacturaDTO>> Delete(int id)
        {
            var codigoFactura = await contexto.Facturas.Select(x => x.NumeroFactura)
                .FirstOrDefaultAsync(x => x == id);
            if (codigoFactura == default(int))
            {
                return NotFound();
            }
            contexto.Remove(new Factura { NumeroFactura = id });
            await contexto.SaveChangesAsync();
            return NoContent();
        }
    }  
}