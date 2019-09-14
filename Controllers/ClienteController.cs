using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly InventarioDBContext dBContext;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public ClienteController(InventarioDBContext dBContext, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> Get()
        {
            var clientes = await this.dBContext.Clientes.ToListAsync();
            var clientesDTO = this.mapper.Map<List<ClienteDTO>>(clientes);
            return clientesDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name ="GetCliente")]
        public async Task<ActionResult<ClienteDTO>> Get(string id)
        {
            var clientes = await this.dBContext.Clientes.FirstOrDefaultAsync(x => x.Nit == id);
            if(clientes == null)
            {
                return NotFound();
            }
            var clienteDTO = this.mapper.Map<ClienteDTO>(clientes);
            return clienteDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClienteCreacionDTO clienteCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var cliente = this.mapper.Map<Cliente>(clienteCreacion);
            this.dBContext.Add(clienteCreacion);
            await this.dBContext.SaveChangesAsync();
            var clienteDTO = this.mapper.Map<ClienteDTO>(cliente);
            return new CreatedAtRouteResult("GetCliente", new { id = cliente.Nit }, clienteDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ClienteCreacionDTO clienteActualizacion)
        {
            var cliente = this.mapper.Map<Cliente>(clienteActualizacion);
            cliente.Nit = id;
            this.dBContext.Entry(cliente).State = EntityState.Modified;
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClienteDTO>> Delete(string id)
        {
            var codigoCliente = await this.dBContext.Clientes.Select(x => x.Nit)
                .FirstOrDefaultAsync(x => x == id);
            if (codigoCliente == default(string))
            {
                return NotFound();
            }
            this.dBContext.Remove(new Cliente { Nit = id });
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

