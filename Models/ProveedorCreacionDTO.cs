﻿using InventarioAPI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class ProveedorCreacionDTO
    {
        [Required]
        public string Nit { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string PaginaWeb { get; set; }
        public string ContactoPrincipal { get; set; }   
    }
}
