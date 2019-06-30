using AutoMapper;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TipoEmpaquesController : ControllerBase
    {
        private readonly InventarioDBContext contexto;
        private readonly IMapper mapper;

        //Inyección de dependencia
        public TipoEmpaquesController (InventarioDBContext contexto, IMapper mapper)
        {
            this.contexto = contexto;
            this.mapper = mapper;
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoEmpaqueDTO>>> Get()
        {
            var tipoEmpaques = await contexto.TipoEmpaques.ToListAsync();
            var TipoEmpaquesDTO = mapper.Map<List<TipoEmpaqueDTO>>(tipoEmpaques);
            return TipoEmpaquesDTO; 
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetTipoEmpaque")]
        public async Task<ActionResult<TipoEmpaqueDTO>> Get(int id)
        {
            var tipoEmpaques = await contexto.TipoEmpaques.FirstOrDefaultAsync(x => x.CodigoEmpaque == id);
            if(tipoEmpaques == null)
            {
                return NotFound();
            }
            var tipoEmpaquesDTO = mapper.Map<TipoEmpaqueDTO>(tipoEmpaques);
            return tipoEmpaquesDTO;
        }

        //Método Asíncrono
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TipoEmpaqueCreacionDTO tipoEmpaqueCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var tipoEmpaques = mapper.Map<TipoEmpaque>(tipoEmpaqueCreacion);
            contexto.Add(tipoEmpaques);
            await contexto.SaveChangesAsync();
            var tipoEmpaquesDTO = mapper.Map<TipoEmpaqueDTO>(tipoEmpaques);
            return new CreatedAtRouteResult("GetTipoEmpaque", new { id = tipoEmpaques.CodigoEmpaque }, tipoEmpaquesDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] TipoEmpaqueCreacionDTO tipoEmpaqueActualizacion)
        {
            var tipoEmpaque = mapper.Map<TipoEmpaque>(tipoEmpaqueActualizacion);
            tipoEmpaque.CodigoEmpaque = id;
            this.contexto.Entry(tipoEmpaque).State = EntityState.Modified;
            await this.contexto.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoEmpaqueDTO>> Delete(int id)
        {
            var codigoTipoEmpaque = await this.contexto.TipoEmpaques.Select(x => x.CodigoEmpaque)
                .FirstOrDefaultAsync(x => x == id);
            if(codigoTipoEmpaque == default(int))
            {
                return NotFound();
            }
            this.contexto.Remove(new TipoEmpaque { CodigoEmpaque = id });
            await this.contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}
