using InventarioAPI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class ClienteCreacionDTO
    {
        [Required]
        public string Dpi { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public List<Factura> Facturas { get; set; }
        public List<TelefonoCliente> TelefonoClientes { get; set; }
        public List<EmailCliente> EmailClientes { get; set; }
    }
}
