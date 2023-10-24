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
    public partial class mdProveedor : Form
    {
        public Proveedor _proveedor { get; set; }
        public mdProveedor()
        {
            InitializeComponent();
        }

        private void mdProveedor_Load(object sender, EventArgs e)
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

            List<Proveedor> lista_proveedores = new CN_Proveedor().Listar();
            foreach (Proveedor item in lista_proveedores)
            {
                dgvData.Rows.Add(new object[] {
                    item.id_proveedor,
                    item.documento,
                    item.razon_social,
                    item.correo,
                    item.telefono,
                });
            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iColum = e.ColumnIndex;

            if(iRow >= 0 && iColum > 0)
            {
                _proveedor = new Proveedor()
                {
                    id_proveedor = Convert.ToInt32(dgvData.Rows[iRow].Cells["id_proveedor"].Value.ToString()),
                    documento = dgvData.Rows[iRow].Cells["documento"].Value.ToString(),
                    razon_social = dgvData.Rows[iRow].Cells["razon_social"].Value.ToString(),
                    correo = dgvData.Rows[iRow].Cells["correo"].Value.ToString(),
                    telefono = dgvData.Rows[iRow].Cells["telefono"].Value.ToString()
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
