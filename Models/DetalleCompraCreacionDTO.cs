using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventarioAPI.Models
{
    public class DetalleCompraCreacionDTO
    {        
        [Required]
        public int NumeroDocumento { get; set; }
        public int CodigoProveedor { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
