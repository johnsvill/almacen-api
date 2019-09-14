using System;
using System.Collections.Generic;
using AutoMapper;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmailClienteController : ControllerBase
    {
        private readonly InventarioDBContext dBContext;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public EmailClienteController(InventarioDBContext dBContext, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmailClienteDTO>>> Get()
        {
            var emailCliente = await this.dBContext.EmailClientes.ToListAsync();
            var emailClienteDTO = this.mapper.Map<List<EmailClienteDTO>>(emailCliente);
            return emailClienteDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetEmailCliente")]
        public async Task<ActionResult<EmailClienteDTO>> Get(int id)
        {
            var emailCliente = await this.dBContext.EmailClientes.FirstOrDefaultAsync(x => x.CodigoEmail == id);
            if (emailCliente == null)
            {
                return NotFound();
            }
            var emailClienteDTO = this.mapper.Map<EmailClienteDTO>(emailCliente);
            return emailClienteDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmailClienteCreacionDTO emailClienteCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var emailCliente = this.mapper.Map<EmailCliente>(emailClienteCreacion);
            this.dBContext.Add(emailCliente);
            await this.dBContext.SaveChangesAsync();
            var emailClienteDTO = this.mapper.Map<EmailClienteDTO>(emailCliente);
            return new CreatedAtRouteResult("GetEmailCliente", new { id = emailCliente.CodigoEmail }, emailClienteDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] EmailClienteCreacionDTO emailClienteActualizacion)
        {
            var emailCliente = this.mapper.Map<EmailCliente>(emailClienteActualizacion);
            emailCliente.CodigoEmail = id;
            this.dBContext.Entry(emailCliente).State = EntityState.Modified;
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmailClienteDTO>> Delete(int id)
        {
            var CodigoEmailCliente = await this.dBContext.EmailClientes.Select(x => x.CodigoEmail)
                .FirstOrDefaultAsync(x => x == id);
            if (CodigoEmailCliente == default(int))
            {
                return NotFound();
            }
            this.dBContext.Remove(new EmailCliente { CodigoEmail = id });
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
