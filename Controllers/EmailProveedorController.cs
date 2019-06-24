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
        private readonly InventarioDBContext contexto;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public EmailProveedorController(InventarioDBContext contexto, IMapper mapper)
        {
            this.contexto = contexto;
            this.mapper = mapper;   
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmailProveedorDTO>>> Get()
        {
            var emailProveedor = await contexto.EmailProveedores.ToListAsync();
            var emailProveedorDTO = mapper.Map<List<EmailProveedorDTO>>(emailProveedor);
            return emailProveedorDTO;
        }

        [HttpGet("{id}", Name = "GetEmailProveedor")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var emailProveedor = await contexto.EmailProveedores.FirstOrDefaultAsync(x => x.CodigoEmail == id);
            if (emailProveedor == null)
            {
                return NotFound();
            }
            var emailProveedorDTO = mapper.Map<CategoriaDTO>(emailProveedor);
            return emailProveedorDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmailProveedorCreacionDTO emailProveedorCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var emailProveedor = mapper.Map<EmailProveedor>(emailProveedorCreacion);
            contexto.Add(emailProveedor);
            await contexto.SaveChangesAsync();
            var emailProveedorDTO = mapper.Map<EmailProveedorDTO>(emailProveedor);
            return new CreatedAtRouteResult("GetEmailProveedor", new { id = emailProveedor.CodigoEmail }, emailProveedorDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] EmailProveedorCreacionDTO emailProveedorActualizacion)
        {
            var emailProveedor = mapper.Map<EmailProveedor>(emailProveedorActualizacion);
            emailProveedor.CodigoEmail = id;
            contexto.Entry(emailProveedor).State = EntityState.Modified;
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EmailProveedorDTO>> Delete(int id)
        {
            var emailProveedor = await contexto.EmailProveedores.Select(x => x.CodigoEmail)
                .FirstOrDefaultAsync(x => x == id);
            if (emailProveedor == default(int))
            {
                return NotFound();
            }
            contexto.Remove(new EmailProveedor { CodigoEmail = id });
            await contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}
