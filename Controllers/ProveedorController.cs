﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly InventarioDBContext contexto;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public ProveedorController (InventarioDBContext contexto, IMapper mapper)
        {
            this.contexto = contexto;
            this.mapper = mapper;
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorDTO>>> Get()
        {
            var proveedores = await contexto.Proveedores.ToListAsync();
            var proveedoresDTO = mapper.Map<List<ProveedorDTO>>(proveedores);
            return proveedoresDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetProveedor")]
        public async Task<ActionResult<ProveedorDTO>> Get(int id)
        {
            var proveedor = await contexto.Proveedores.FirstOrDefaultAsync(x => x.CodigoProveedor == id);
            if (proveedor == null)
            {
                return NotFound();
            }
            var proveedoresDTO = mapper.Map<ProveedorDTO>(proveedor);
            return proveedoresDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProveedorCreacionDTO proveedorCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var proveedor = mapper.Map<Proveedor>(proveedorCreacion);
            contexto.Add(proveedor);
            await contexto.SaveChangesAsync();
            var proveedorDTO = mapper.Map<ProveedorDTO>(proveedor);
            return new CreatedAtRouteResult("GetProveedor", new { id = proveedor.CodigoProveedor }, proveedorDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProveedorCreacionDTO proveedorActualizacion)
        {
            var proveedor = mapper.Map<Proveedor>(proveedorActualizacion);
            proveedor.CodigoProveedor = id;
            contexto.Entry(proveedor).State = EntityState.Modified;
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var codigoProveedor = await contexto.Proveedores.Select(x => x.CodigoProveedor)
                .FirstOrDefaultAsync(x => x == id);
            if(codigoProveedor == default(int))
            {
                return NotFound();
            }
            contexto.Remove(new Proveedor { CodigoProveedor = id });
            await contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}
