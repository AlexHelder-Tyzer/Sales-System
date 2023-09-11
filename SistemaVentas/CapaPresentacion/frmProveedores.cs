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

namespace CapaPresentacion
{
    public partial class frmProveedores : Form
    {
        public frmProveedores()
        {
            InitializeComponent();
        }

        private void frmProveedores_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombo() { valor = 1, texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { valor = 0, texto = "No Activo" });
            cboEstado.DisplayMember = "texto";
            cboEstado.ValueMember = "valor";
            cboEstado.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true && columna.HeaderText != "")
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { valor = columna.Name, texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "texto";
            cboBusqueda.ValueMember = "valor";
            cboBusqueda.SelectedIndex = 0;

            // mostrar todos los usuarios
            List<Proveedor> lista_proveedores = new CN_Proveedor().Listar();
            foreach (Proveedor item in lista_proveedores)
            {
                dgvData.Rows.Add(new object[] {
                    "",
                    item.id_proveedor,
                    item.documento,
                    item.razon_social,
                    item.correo,
                    item.telefono,
                    item.estado == true ? 1 : 0,
                    item.estado == true ? "Activo" : "No Activo",
                });
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;
            Proveedor obj_proveedor = new Proveedor()
            {
                id_proveedor = Convert.ToInt32(txtId.Text),
                documento = txtDocumento.Text,
                razon_social = txtRazonSocial.Text,
                correo = txtCorreo.Text,
                telefono = txtTelefono.Text,
                estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).valor) == 1 ? true : false
            };

            if (obj_proveedor.id_proveedor == 0)
            {
                int id_proveedor_generado = new CN_Proveedor().Registrar(obj_proveedor, out Mensaje); // se genera desde la base de datos

                if (id_proveedor_generado != 0)
                {
                    // solo hace un registro de nuestro data grid view // pintar en el dgv
                    dgvData.Rows.Add(new object[] {
                        "",
                        id_proveedor_generado,
                        txtDocumento.Text,
                        txtRazonSocial.Text,
                        txtCorreo.Text,
                        txtTelefono.Text,
                        ((OpcionCombo)cboEstado.SelectedItem).valor.ToString(),
                        ((OpcionCombo)cboEstado.SelectedItem).texto.ToString(),
                        });

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(Mensaje);
                }
            }
            else // COdigo para editar un registro
            {
                bool resultado = new CN_Proveedor().Editar(obj_proveedor, out Mensaje);
                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)]; // obtener la fila de edicion del dgv
                    row.Cells["id_proveedor"].Value = txtId.Text;
                    row.Cells["documento"].Value = txtDocumento.Text;
                    row.Cells["razon_social"].Value = txtRazonSocial.Text;
                    row.Cells["correo"].Value = txtCorreo.Text;
                    row.Cells["telefono"].Value = txtTelefono.Text;
                    row.Cells["estado_valor"].Value = ((OpcionCombo)cboEstado.SelectedItem).valor.ToString();
                    row.Cells["estado"].Value = ((OpcionCombo)cboEstado.SelectedItem).texto.ToString();

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(Mensaje);
                }
            }
        }

        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = "0";
            txtDocumento.Text = "";
            txtRazonSocial.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            cboEstado.SelectedIndex = 0;

            txtDocumento.Select();
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All); // obtiene los limites de la celda, que considere todos los limites de la celda
                var w = Properties.Resources.check.Width;// ancho de la imagen de los recursos check
                var h = Properties.Resources.check.Height; // recuperar el alto de la imagen
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check, new Rectangle(x, y, w, h));
                e.Handled = true; // para continuar con el evento del click
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btn_seleccionar")
            {
                int indice = e.RowIndex;
                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvData.Rows[indice].Cells["id_proveedor"].Value.ToString();
                    txtDocumento.Text = dgvData.Rows[indice].Cells["documento"].Value.ToString();
                    txtRazonSocial.Text = dgvData.Rows[indice].Cells["razon_social"].Value.ToString();
                    txtCorreo.Text = dgvData.Rows[indice].Cells["correo"].Value.ToString();
                    txtTelefono.Text = dgvData.Rows[indice].Cells["telefono"].Value.ToString();
                    // mostrar contenido del combo box estado
                    foreach (OpcionCombo oc in cboEstado.Items)
                    {
                        if (Convert.ToInt32(oc.valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["estado_valor"].Value.ToString()))
                        {
                            int indice_combo = cboEstado.Items.IndexOf(oc);
                            cboEstado.SelectedIndex = indice_combo;
                            break;
                        }
                    }

                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtId.Text) != 0)
            {
                if (MessageBox.Show("Desea eliminar el Proveedor?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Proveedor obj_proveedor = new Proveedor()
                    {
                        id_proveedor = Convert.ToInt32(txtId.Text)
                    };

                    bool respuesta = new CN_Proveedor().Eliminar(obj_proveedor, out mensaje);

                    if (respuesta)
                    {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
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
