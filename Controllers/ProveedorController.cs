using AutoMapper;
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
        private readonly InventarioDBContext dBContext;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public ProveedorController (InventarioDBContext dBContext, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorDTO>>> Get()
        {
            var proveedores = await this.dBContext.Proveedores.ToListAsync();
            var proveedoresDTO = this.mapper.Map<List<ProveedorDTO>>(proveedores);
            return proveedoresDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetProveedor")]
        public async Task<ActionResult<ProveedorDTO>> Get(int id)
        {
            var proveedor = await this.dBContext.Proveedores.FirstOrDefaultAsync(x => x.CodigoProveedor == id);
            if (proveedor == null)
            {
                return NotFound();
            }
            var proveedoresDTO = this.mapper.Map<ProveedorDTO>(proveedor);
            return proveedoresDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProveedorCreacionDTO proveedorCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var proveedor = this.mapper.Map<Proveedor>(proveedorCreacion);
            this.dBContext.Add(proveedor);
            await this.dBContext.SaveChangesAsync();
            var proveedorDTO = this.mapper.Map<ProveedorDTO>(proveedor);
            return new CreatedAtRouteResult("GetProveedor", new { id = proveedor.CodigoProveedor }, proveedorDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProveedorCreacionDTO proveedorActualizacion)
        {
            var proveedor = this.mapper.Map<Proveedor>(proveedorActualizacion);
            proveedor.CodigoProveedor = id;
            this.dBContext.Entry(proveedor).State = EntityState.Modified;
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var codigoProveedor = await this.dBContext.Proveedores.Select(x => x.CodigoProveedor)
                .FirstOrDefaultAsync(x => x == id);
            if(codigoProveedor == default(int))
            {
                return NotFound();
            }
            this.dBContext.Remove(new Proveedor { CodigoProveedor = id });
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
