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
        private readonly InventarioDBContext contexto;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public ClienteController(InventarioDBContext contexto, IMapper mapper)
        {
            this.contexto = contexto;
            this.mapper = mapper;
        }
        //Métodos Asíncronos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> Get()
        {
            var clientes = await contexto.Clientes.ToListAsync();
            var clientesDTO = mapper.Map<List<ClienteDTO>>(clientes);
            return clientesDTO;
        }

        [HttpGet("{id}", Name ="GetCliente")]
        public async Task<ActionResult<ClienteDTO>> Get(string id)
        {
            var clientes = await contexto.Clientes.FirstOrDefaultAsync(x => x.Nit == id);
            if(clientes == null)
            {
                return NotFound();
            }
            var clienteDTO = mapper.Map<ClienteDTO>(clientes);
            return clienteDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClienteCreacionDTO clienteCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var cliente = mapper.Map<Cliente>(clienteCreacion);
            contexto.Add(clienteCreacion);
            await contexto.SaveChangesAsync();
            var clienteDTO = mapper.Map<ClienteDTO>(cliente);
            return new CreatedAtRouteResult("GetCliente", new { id = cliente.Nit }, clienteDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ClienteCreacionDTO clienteActualizacion)
        {
            var cliente = mapper.Map<Cliente>(clienteActualizacion);
            cliente.Nit = id;
            contexto.Entry(cliente).State = EntityState.Modified;
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ClienteDTO>> Delete(string id)
        {
            var codigoCliente = await contexto.Clientes.Select(x => x.Nit)
                .FirstOrDefaultAsync(x => x == id);
            if (codigoCliente == default(string))
            {
                return NotFound();
            }
            contexto.Remove(new Cliente { Nit = id });
            await contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}

