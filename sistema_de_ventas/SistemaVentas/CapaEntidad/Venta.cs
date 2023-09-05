using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int id_venta { get; set; }
        public Usuario oUsuario { get; set; }
        public string tipo_documento { get; set; }
        public string numero_documento { get; set; }
        public string documento_cliente { get; set; }
        public string nombre_cliente { get; set; }
        public decimal monto_pago { get; set; }
        public decimal monto_cambio { get; set; }
        public decimal monto_total { get; set; }
        public List<Detalle_Venta> oDetalleVenta { get; set; }
        public string fecha_registro { get; set; }

    }
}
