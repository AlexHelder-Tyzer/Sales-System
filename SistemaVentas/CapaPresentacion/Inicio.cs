using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaEntidad;
using FontAwesome.Sharp;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class Inicio : Form
    {
        private static Usuario usuario_actual;
        private static IconMenuItem menu_activo = null;
        private static Form formulario_activo = null;
        public Inicio(Usuario objusuario = null)
        {
            if(objusuario == null)
            {
                usuario_actual = new Usuario() { nombre_completo = "ADMIN PREDEFINIDO", id_usuario = 1};
            }
            else
            {
                usuario_actual = objusuario;
            }

            InitializeComponent();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            List<Permiso> lista_permisos = new CN_Permiso().Listar(usuario_actual.id_usuario);

            foreach (IconMenuItem icon_menu in menu.Items)
            {
                bool encontrado = lista_permisos.Any(m => m.nombre_menu == icon_menu.Name); // m cada elemento de la lista
                if(encontrado == false)
                {
                    icon_menu.Visible = false;
                }
            }
            lblUsuario.Text = usuario_actual.nombre_completo.ToString();
            
        }

        private void Abrir_Formulario(IconMenuItem menu, Form formulario)
        {
            if (menu_activo != null)
            {
                menu_activo.BackColor = Color.White;
            }

            menu.BackColor = Color.Silver;
            menu_activo = menu;

            if (formulario_activo != null)
            {
                formulario_activo.Close();
            }

            formulario_activo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.BackColor = Color.MediumAquamarine;
            contenedor.Controls.Add(formulario);
            formulario.Show();
        }
        private void menuUsuarios_Click(object sender, EventArgs e)
        {
            Abrir_Formulario((IconMenuItem)sender, new frmUsuarios()); // arb9ir formulario dentro del panel
        }

        private void subMenuCategoria_Click(object sender, EventArgs e)
        {
            Abrir_Formulario(menuMantenedor, new frmCategoria());
        }

        private void subMenuProducto_Click(object sender, EventArgs e)
        {
            Abrir_Formulario(menuMantenedor, new frmProducto());
        }

        private void subMenuRegistrarVenta_Click(object sender, EventArgs e)
        {
            Abrir_Formulario(menuVentas, new frmVentas());
        }

        private void subMenuVerDetalleVenta_Click(object sender, EventArgs e)
        {
            Abrir_Formulario(menuVentas, new frmDetalleVenta());
        }

        private void subMenuRegistrarCompra_Click(object sender, EventArgs e)
        {
            Abrir_Formulario(menuCompras, new frmCompras());
        }

        private void subMenuVerDetalleCompra_Click(object sender, EventArgs e)
        {
            Abrir_Formulario(menuCompras, new frmDetalleCompra());
        }

        private void menuProveedores_Click(object sender, EventArgs e)
        {
            Abrir_Formulario((IconMenuItem)sender, new frmProveedores());
        }

        private void menuReportes_Click(object sender, EventArgs e)
        {
            Abrir_Formulario((IconMenuItem)sender, new frmReportes());
        }

        private void menuClientes_Click(object sender, EventArgs e)
        {
            Abrir_Formulario((IconMenuItem)sender, new frmClientes());
        }
    }
}
