using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Compra
    {
        public int id_compra { get; set; }
        public Usuario oUsuario { get; set; }
        public Proveedor oProveedor { get; set; }
        public string tipo_documento { get; set; }
        public string numero_documento { get; set; }
        public decimal monto_total { get; set; }
        public List<Detalle_Compra> oDetalleCompra { get; set; }
        public string fecha_registro { get; set; }
    }
}
