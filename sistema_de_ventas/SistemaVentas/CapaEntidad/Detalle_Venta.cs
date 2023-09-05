using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Detalle_Venta
    {
        public int id_detalle_venta {  get; set; }
        public Producto oProducto { get; set; }
        public decimal precio_venta { get; set; }
        public int cantidad { get; set; }
        public decimal subtotal { get; set; }
        public string fecha_registro { get; set; }
    }
}
