using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaPresentacion.Utilidades;
using CapaEntidad;
using CapaNegocio;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace CapaPresentacion
{
    public partial class frmUsuarios : Form
    {
        public frmUsuarios()
        {
            InitializeComponent();
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombo() { valor = 1, texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { valor = 0, texto = "No Activo" });
            cboEstado.DisplayMember = "texto";
            cboEstado.ValueMember = "valor";
            cboEstado.SelectedIndex = 0;

            List<Rol> lista_rol = new CN_Rol().Listar();
            foreach (Rol item in lista_rol)
            {
                cboRol.Items.Add(new OpcionCombo() { valor = item.id_rol, texto = item.descripcion });
            }

            cboRol.DisplayMember = "texto";
            cboRol.ValueMember = "valor";
            cboRol.SelectedIndex = 0;


            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if(columna.Visible == true && columna.HeaderText != "")
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { valor = columna.Name, texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "texto";
            cboBusqueda.ValueMember = "valor";
            cboBusqueda.SelectedIndex = 0;

            // mostrar todos los usuarios
            List<Usuario> lista_usuario = new CN_Usuario().Listar();
            foreach (Usuario item in lista_usuario)
            {
                dgvData.Rows.Add(new object[] {"", item.id_usuario, item.documento, item.nombre_completo, item.correo, item.clave, item.telefono,
                    item.oRol.id_rol,
                    item.oRol.descripcion,
                    item.estado == true ? 1 : 0,
                    item.estado == true ? "Activo" : "No Activo",
                }) ;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;
            Usuario obj_usuario = new Usuario()
            {
                id_usuario = Convert.ToInt32(txtId.Text),
                documento = txtDocumento.Text,
                nombre_completo = txtNombreCompleto.Text,
                correo = txtCorreo.Text,
                clave = txtClave.Text,
                telefono = txtTelefono.Text,
                oRol = new Rol() { id_rol = Convert.ToInt32(((OpcionCombo)cboRol.SelectedItem).valor) },
                estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).valor) == 1 ? true : false
            };

            if(obj_usuario.id_usuario == 0)
            {
                int id_usuario_generado = new CN_Usuario().Registrar(obj_usuario, out Mensaje); // se genera desde la base de datos

                if(id_usuario_generado != 0)
                {
                    // solo hace un registro de nuestro data grid view
                    dgvData.Rows.Add(new object[] {"", id_usuario_generado, txtDocumento.Text, txtNombreCompleto.Text, txtCorreo.Text, txtClave.Text, txtTelefono.Text,
                    ((OpcionCombo)cboRol.SelectedItem).valor.ToString(),
                    ((OpcionCombo)cboRol.SelectedItem).texto.ToString(),
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
                bool resultado = new CN_Usuario().Editar(obj_usuario, out Mensaje);
                if(resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)]; // obtener la fila de edicion del dgv
                    row.Cells["id_usuario"].Value = txtId.Text;
                    row.Cells["documento"].Value = txtDocumento.Text ;
                    row.Cells["nombre_completo"].Value = txtNombreCompleto.Text;
                    row.Cells["correo"].Value = txtCorreo.Text;
                    row.Cells["clave"].Value = txtClave.Text;
                    row.Cells["telefono"].Value = txtTelefono.Text;
                    row.Cells["id_rol"].Value = ((OpcionCombo)cboRol.SelectedItem).valor.ToString();
                    row.Cells["rol"].Value = ((OpcionCombo)cboRol.SelectedItem).texto.ToString();
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
            txtNombreCompleto.Text = "";
            txtCorreo.Text = "";
            txtClave.Text = "";
            txtConfirmarClave.Text = "";
            txtTelefono.Text = "";
            cboRol.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;

            txtDocumento.Select();
        }

        // Pintar los  botones de la columna 0 del data grid view (los checks)
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

        // 
        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btn_seleccionar")
            {
                int indice = e.RowIndex;
                if(indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvData.Rows[indice].Cells["id_usuario"].Value.ToString();
                    txtDocumento.Text = dgvData.Rows[indice].Cells["documento"].Value.ToString();
                    txtNombreCompleto.Text = dgvData.Rows[indice].Cells["nombre_completo"].Value.ToString();
                    txtCorreo.Text = dgvData.Rows[indice].Cells["correo"].Value.ToString();
                    txtClave.Text = dgvData.Rows[indice].Cells["clave"].Value.ToString();
                    txtConfirmarClave.Text = dgvData.Rows[indice].Cells["clave"].Value.ToString();
                    txtTelefono.Text = dgvData.Rows[indice].Cells["telefono"].Value.ToString();
                    // cargar contrnido de combobox rol
                    foreach(OpcionCombo oc in cboRol.Items)
                    {
                        if( Convert.ToInt32(oc.valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["id_rol"].Value.ToString()))
                        {
                            int indice_combo = cboRol.Items.IndexOf(oc);
                            cboRol.SelectedIndex = indice_combo;
                            break;
                        }
                    }
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
            if(Convert.ToInt32(txtId.Text) != 0)
            {
                if(MessageBox.Show("Desea eliminar el usuario?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Usuario obj_usuario = new Usuario()
                    {
                        id_usuario = Convert.ToInt32(txtId.Text)
                    };

                    bool respuesta = new CN_Usuario().Eliminar(obj_usuario, out mensaje);
                    
                    if(respuesta)
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

            if(dgvData.Rows.Count > 0 )
            {
                foreach(DataGridViewRow row in dgvData.Rows)
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
