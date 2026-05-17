using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnFerreteria
{
    public class CompraDto
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public decimal total { get; set; }
        public string usuarioRegistro { get; set; }
        public string proveedor { get; set; } // Nombre del proveedor aplanado
    }
}
