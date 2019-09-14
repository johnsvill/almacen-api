using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TelefonoClienteController : ControllerBase
    {
        private readonly InventarioDBContext dBContext;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public TelefonoClienteController(InventarioDBContext dBContext, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
        }   
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TelefonoClienteDTO>>> Get()
        {
            var telefonoCliente = await this.dBContext.TelefonoClientes.ToListAsync();
            var telefonoClienteDTO = this.mapper.Map<List<TelefonoClienteDTO>>(telefonoCliente);
            return telefonoClienteDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetTelefonoCliente")]
        public async Task<ActionResult<TelefonoClienteDTO>> Get(int id)
        {
            var telefonoCliente = await this.dBContext.TelefonoClientes.FirstOrDefaultAsync(x => x.CodigoTelefono == id);
            if (telefonoCliente == null)
            {
                return NotFound();
            }
            var telefonoClienteDTO = this.mapper.Map<TelefonoClienteDTO>(telefonoCliente);
            return telefonoClienteDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TelefonoClienteCreacionDTO telefonoClienteCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var telefonoCliente = this.mapper.Map<TelefonoCliente>(telefonoClienteCreacion);
            this.dBContext.Add(telefonoCliente);
            await this.dBContext.SaveChangesAsync();
            var telefonoClienteDTO = this.mapper.Map<TelefonoClienteDTO>(telefonoCliente);
            return new CreatedAtRouteResult("GetTelefonoCliente", new { id = telefonoCliente.CodigoTelefono }, telefonoClienteDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] TelefonoClienteCreacionDTO telefonoClienteActualizacion)
        {
            var telefonoCliente = this.mapper.Map<TelefonoCliente>(telefonoClienteActualizacion);
            telefonoCliente.CodigoTelefono = id;
            this.dBContext.Entry(telefonoCliente).State = EntityState.Modified;
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<TelefonoClienteDTO>> Delete(int id)
        {
            var telefonoCliente = await this.dBContext.TelefonoClientes.Select(x => x.CodigoTelefono)
                .FirstOrDefaultAsync(x => x == id);
            if (telefonoCliente == default(int))
            {
                return NotFound();
            }
            this.dBContext.Remove(new TelefonoCliente { CodigoTelefono = id });
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
