using System;
using AutoMapper;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TelefonoProveedorController : ControllerBase
    {
        private readonly InventarioDBContext contexto;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public TelefonoProveedorController(InventarioDBContext contexto, IMapper mapper)
        {
            this.contexto = contexto;
            this.mapper = mapper;   
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TelefonoProveedorDTO>>> Get()
        {
            var telefonoProv = await contexto.TelefonoProveedores.ToListAsync();
            var telefonoProvDTO = mapper.Map<List<TelefonoProveedorDTO>>(telefonoProv);
            return telefonoProvDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetTelefonoProveedor")]
        public async Task<ActionResult<TelefonoProveedorDTO>> Get(int id)
        {
            var telefonoProv = await contexto.TelefonoProveedores.FirstOrDefaultAsync(x => x.CodigoTelefono == id);
            if (telefonoProv == null)
            {
                return NotFound();
            }
            var telefonoProvDTO = mapper.Map<TelefonoProveedorDTO>(telefonoProv);
            return telefonoProvDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TelefonoProveedorCreacionDTO telefonoProvCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var telefonoProv = mapper.Map<TelefonoProveedor>(telefonoProvCreacion);
            contexto.Add(telefonoProv);
            await contexto.SaveChangesAsync();
            var telefonoProvDTO = mapper.Map<TelefonoProveedorDTO>(telefonoProv);
            return new CreatedAtRouteResult("GetTelefonoProveedor", new { id = telefonoProv.CodigoTelefono }, telefonoProvDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] TelefonoProveedorCreacionDTO telefonoProvActualizacion)
        {
            var telefonoProv = mapper.Map<TelefonoProveedor>(telefonoProvActualizacion);
            telefonoProv.CodigoTelefono = id;
            contexto.Entry(telefonoProv).State = EntityState.Modified;
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<TelefonoProveedorDTO>> Delete(int id)
        {
            var telefonoProv = await contexto.TelefonoProveedores.Select(x => x.CodigoTelefono)
                .FirstOrDefaultAsync(x => x == id);
            if (telefonoProv == default(int))
            {
                return NotFound();
            }
            contexto.Remove(new TelefonoProveedor { CodigoTelefono = id });
            await contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}
