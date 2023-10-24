using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.Modales
{
    public partial class mdProducto : Form
    {
        public Producto _producto { get; set; }
        public mdProducto()
        {
            InitializeComponent();
        }

        private void mdProducto_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true)
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { valor = columna.Name, texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "texto";
            cboBusqueda.ValueMember = "valor";
            cboBusqueda.SelectedIndex = 0;

            List<Producto> lista_productos = new CN_Producto().Listar();
            foreach (Producto item in lista_productos)
            {
                dgvData.Rows.Add(new object[] {
                    item.id_producto,
                    item.codigo,
                    item.nombre,
                    item.oCategoria.descripcion,
                    item.stock,
                    item.precio_compra,
                    item.precio_venta
                });
            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iColum = e.ColumnIndex;

            if (iRow >= 0 && iColum > 0)
            {
                _producto = new Producto()
                {
                    id_producto = Convert.ToInt32(dgvData.Rows[iRow].Cells["id_producto"].Value.ToString()),
                    codigo = dgvData.Rows[iRow].Cells["codigo"].Value.ToString(),
                    nombre = dgvData.Rows[iRow].Cells["nombre"].Value.ToString(),
                    stock = Convert.ToInt32(dgvData.Rows[iRow].Cells["stock"].Value.ToString()),
                    precio_compra = Convert.ToDecimal(dgvData.Rows[iRow].Cells["precio_compra"].Value.ToString()),
                    precio_venta = Convert.ToDecimal(dgvData.Rows[iRow].Cells["precio_venta"].Value.ToString())
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            string columna_filtro = ((OpcionCombo)cboBusqueda.SelectedItem).valor.ToString();

            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Cells[columna_filtro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = string.Empty;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }
    }
}
