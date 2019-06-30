﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InventarioAPI.Entities;
using InventarioAPI.Models;
using System.Threading.Tasks;

namespace InventarioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        //Inyección de dependencia
        private readonly InventarioDBContext contexto;
        private readonly IMapper mapper;
                    
        public ProductosController(InventarioDBContext contexto, IMapper mapper)
        {
            this.contexto = contexto;
            this.mapper = mapper;   
        }
        //Método Asíncrono
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> Get()
        {
            var productos = await this.contexto.Productos.Include("Categoria").Include("TipoEmpaques").ToListAsync();
            var productosDTO = this.mapper.Map<List<ProductoDTO>>(productos);
            return productosDTO;
        }

        //Método Asíncrono
        [HttpGet("{id}", Name = "GetProducto")]
        public async Task<ActionResult<ProductoDTO>> Get(int id)//Se devuelve un objeto ProductoDTO
        {
            var producto = await this.contexto.Productos
                .Include("Categoria").Include("TipoEmpaques")
                .FirstOrDefaultAsync(x => x.CodigoProducto == id);
            if (producto == null)
            {
                return NotFound();
            }
            var productoDTO = this.mapper.Map<ProductoDTO>(producto);//El objeto producto se convierte en un ProductoDTO
            return productoDTO;
        }

        //Método Asíncrono
        [HttpPost]//Crear
        public async Task<ActionResult> Post([FromBody] ProductoCreacionDTO productoCreacion)//Se espera recibir un JSON o un DOC.XML
        {
            var producto = this.mapper.Map<Producto>(productoCreacion);//Se convierte el objeto a tipo producto mapeado de la DB
            this.contexto.Add(producto);    
            await this.contexto.SaveChangesAsync();
            var productoDTO = this.mapper.Map<ProductoDTO>(producto);//Producto se convierte a un objeto ProductoDTO
            return new CreatedAtRouteResult("GetProducto", new { id = producto.CodigoProducto }, productoDTO);
        }

        //Método Asíncrono
        [HttpPut("{id}")]//Modificar
        public async Task<ActionResult> Put(int id, [FromBody] ProductoCreacionDTO productoActualizacion)//Task hace referencia a los hilos
        {
            var producto = this.mapper.Map<Producto>(productoActualizacion);
            producto.CodigoProducto = id;
            this.contexto.Entry(producto).State = EntityState.Modified;
            await this.contexto.SaveChangesAsync();
            return NoContent();
        }

        //Método Asíncrono
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductoDTO>> Delete(int id)
        {
            var codigoProducto = await this.contexto.Productos.Select(x => x.CodigoProducto)
                .FirstOrDefaultAsync(x => x == id);//La propiedad CodigoProducto sea igual al ID
            if (codigoProducto == default(int))
            {
                return NotFound();
            }
            this.contexto.Remove(new Producto { CodigoProducto = id });
            await this.contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}
