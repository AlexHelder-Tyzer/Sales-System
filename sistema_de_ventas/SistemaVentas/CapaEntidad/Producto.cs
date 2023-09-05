using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Producto
    {
        public int id_producto { get; set; }
        public Categoria oCategoria { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set;}
        public string descripcion { get; set;}
        public int stock { get; set; }
        public decimal precio_compra { get; set; }
        public decimal precio_venta { get; set; }
        public bool estado { get; set; }
        public string fecha_registro { get; set; }
    }
}
