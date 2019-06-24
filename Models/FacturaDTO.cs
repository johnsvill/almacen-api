using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class FacturaDTO
    {
        public int NumeroFactura { get; set; }
        [Required]
        public string Nit { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
