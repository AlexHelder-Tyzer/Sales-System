using CapaEntidad;
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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            customizeDesign();
        }

        // Personalizar diseño
        private void customizeDesign()
        {
            panelSubMenuCompras.Visible = false;
            panelSubMenuVentas.Visible = false;
            panelSubMenuMantenedor.Visible = false;
        }

        // Ocultar el submenu
        private void hideSubMenu()
        {
            if (panelSubMenuCompras.Visible) { panelSubMenuCompras.Visible = false; }
            if (panelSubMenuVentas.Visible) { panelSubMenuVentas.Visible = false; }
            if (panelSubMenuMantenedor.Visible) { panelSubMenuMantenedor.Visible = false; }
        }

        // mostrar submenu
        private void showSubMenu(Panel subMenu)
        {
            if(subMenu.Visible == false)
            {
                hideSubMenu(); // primero oculatamos algun submeno si estuviera abierto
                subMenu.Visible = true; // mostramos el submenu activo
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        //private void btnMedia_Click(object sender, EventArgs e)
        //{
        //    showSubMenu(panelMediaSubMenu);
        //}

        //private void btnEqualizer_Click(object sender, EventArgs e)
        //{
        //    openChildForm(new frmUsuarios());
        //    //...
        //    // Your code
        //    //...
        //    hideSubMenu(); // esto se hara para cada boton del submenu
        //}


        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if(activeForm != null) // primero cerramos otros formularios
            {
                activeForm.Close();
            }
            activeForm = childForm; // luego abrimos el nuevo formulario
            childForm.TopLevel = false; // se conportaara como un control
            childForm.FormBorderStyle = FormBorderStyle.None; // quitamos los bordes
            childForm.Dock = DockStyle.Fill; // rellenar todo el panel contenedor
            panelChildForm.Controls.Add(childForm); // agregamos el fomrmulario hijo al panel contenedor
            panelChildForm.Tag = childForm; // asociamos el formulario para el panel contenedor
            childForm.BringToFront(); // traer al frente el formulario
            childForm.Show(); // mostramos el formulario
        }

        private void btnCompras_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenuCompras);
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenuVentas);
        }

        private void btnMantenedor_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenuMantenedor);
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnRegistrarCompra_Click(object sender, EventArgs e)
        {
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnVerDetalleCompra_Click(object sender, EventArgs e)
        {
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnVerDetalleVenta_Click(object sender, EventArgs e)
        {
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnCategoria_Click(object sender, EventArgs e)
        {
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnProducto_Click(object sender, EventArgs e)
        {
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnAcercaDe_Click(object sender, EventArgs e)
        {
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Estas seguro de Cerrar Sesión?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                this.Close(); // solo ciera el formulario y no todo los formularios y se mostrara el login
            }
        }
    }
}
