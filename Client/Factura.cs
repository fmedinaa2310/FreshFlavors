using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDiseño.Client
{
    internal class Factura : Producto
    {
       public DateTime Fecha { get; set; }
       public int numeroFactura { get; set; }
    }
}
