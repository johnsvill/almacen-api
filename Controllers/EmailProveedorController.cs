using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmailProveedorController : ControllerBase
    {
        private readonly InventarioDBContext dBContext;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public EmailProveedorController(InventarioDBContext dBContext, IMapper mapper)
        {
            this.dBContext = dBContext;
            this.mapper = mapper;   
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmailProveedorDTO>>> Get()
        {
            var emailProveedor = await this.dBContext.EmailProveedores.ToListAsync();
            var emailProveedorDTO = this.mapper.Map<List<EmailProveedorDTO>>(emailProveedor);
            return emailProveedorDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetEmailProveedor")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var emailProveedor = await this.dBContext.EmailProveedores.FirstOrDefaultAsync(x => x.CodigoEmail == id);
            if (emailProveedor == null)
            {
                return NotFound();
            }
            var emailProveedorDTO = this.mapper.Map<CategoriaDTO>(emailProveedor);
            return emailProveedorDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmailProveedorCreacionDTO emailProveedorCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var emailProveedor = this.mapper.Map<EmailProveedor>(emailProveedorCreacion);
            this.dBContext.Add(emailProveedor);
            await this.dBContext.SaveChangesAsync();
            var emailProveedorDTO = this.mapper.Map<EmailProveedorDTO>(emailProveedor);
            return new CreatedAtRouteResult("GetEmailProveedor", new { id = emailProveedor.CodigoEmail }, emailProveedorDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] EmailProveedorCreacionDTO emailProveedorActualizacion)
        {
            var emailProveedor = this.mapper.Map<EmailProveedor>(emailProveedorActualizacion);
            emailProveedor.CodigoEmail = id;
            this.dBContext.Entry(emailProveedor).State = EntityState.Modified;
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmailProveedorDTO>> Delete(int id)
        {
            var emailProveedor = await this.dBContext.EmailProveedores.Select(x => x.CodigoEmail)
                .FirstOrDefaultAsync(x => x == id);
            if (emailProveedor == default(int))
            {
                return NotFound();
            }
            this.dBContext.Remove(new EmailProveedor { CodigoEmail = id });
            await this.dBContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
