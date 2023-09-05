using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Detalle_Compra
    {
        public int id_detalle_compra { get; set; }
        public Producto oProducto { get; set; }
        public decimal precio_compra { get; set; }
        public decimal precio_venta { get; set; }
        public int cantidad { get; set; }
        public decimal monto_total { get; set; }
        public string fecha_registro { get; set; }
    }
}
