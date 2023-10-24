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

using FontAwesome.Sharp;
using System.Runtime.InteropServices;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class Main : Form
    {
        // atributos
        private IconButton botonActivo;
        private Panel bordeIzqBtn;

        //
        private static Usuario usuario_actual;

        // constructor
        public Main(Usuario objusuario = null)
        {
            InitializeComponent();
            customizeDesign();
            bordeIzqBtn = new Panel();
            bordeIzqBtn.Size = new Size(7, 45); //tamaño del borde
            panelMenuLateral.Controls.Add(bordeIzqBtn); // agregar el borde a los controles del menu lateral
            // form
            this.Text = "";
            this.ControlBox = false;
            this.DoubleBuffered = true;  // reducir el parpadeo en los graficos del form
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            usuario_actual = objusuario;
        }

        /************* METODOS ***********/
        // metodo para activar el boton
        private void ActivarBoton(object senderBtn, Color color)
        {
            if(senderBtn != null) 
            {
                DesactivarBoton();
                // para el boton
                botonActivo = (IconButton)senderBtn; // casteamos el objeto al mismo tipo de vboton que usamos
                botonActivo.BackColor = Color.FromArgb(37, 36, 81); // cambiamos el color del boton
                botonActivo.ForeColor = color; // cambiamos el color de texto del boton
                botonActivo.TextAlign = ContentAlignment.MiddleCenter; // centramos
                botonActivo.IconColor = color;
                botonActivo.TextImageRelation = TextImageRelation.TextBeforeImage;
                botonActivo.ImageAlign = ContentAlignment.MiddleRight;

                // para el borde del boton
                bordeIzqBtn.BackColor = color;
                bordeIzqBtn.Location = new Point(0, botonActivo.Location.Y);
                bordeIzqBtn.Visible = true;
                bordeIzqBtn.BringToFront(); // traemos al frente el boton

                // Icono del formulario hijo actual
                iconCurrentChildForm.IconChar = botonActivo.IconChar;
                iconCurrentChildForm.IconColor = color;
                lblTitleChildForm.Text = botonActivo.Text;
            }
        }

        // metodo para activar el resaltado del boton
        private void DesactivarBoton()
        {
            if(botonActivo != null)
            {
                botonActivo.BackColor = Color.FromArgb(188, 152, 243); // cambiamos el color del boton
                botonActivo.ForeColor = Color.Black; // cambiamos el color de texto del boton
                botonActivo.TextAlign = ContentAlignment.MiddleLeft; // centramos
                botonActivo.IconColor = Color.Black;
                botonActivo.TextImageRelation = TextImageRelation.ImageBeforeText;
                botonActivo.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        // struc de colores
        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(243,168,194);
            public static Color color2 = Color.FromArgb(246,209,222);
            public static Color color3 = Color.FromArgb(117,249,242);
            public static Color color4 = Color.FromArgb(176,246,242);
            public static Color color5 = Color.FromArgb(238,246,176);
            public static Color color6 = Color.FromArgb(216,247,154);
            public static Color color7 = Color.FromArgb(209,234,249);
            public static Color color8 = Color.FromArgb(186,224,245);
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
            ActivarBoton(sender, RGBColors.color1);
            showSubMenu(panelSubMenuCompras);

        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            ActivarBoton(sender, RGBColors.color2);
            showSubMenu(panelSubMenuVentas);
        }

        private void btnMantenedor_Click(object sender, EventArgs e)
        {
            ActivarBoton(sender, RGBColors.color3);
            showSubMenu(panelSubMenuMantenedor);
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            ActivarBoton(sender, RGBColors.color4);
            openChildForm(new frmUsuarios());
            hideSubMenu(); // cerrar el submenu
        }

        private void btnRegistrarCompra_Click(object sender, EventArgs e)
        {
            ActivarBoton(sender, RGBColors.color5);
            openChildForm(new frmCompras(usuario_actual));
            hideSubMenu(); // cerrar el submenu
        }

        private void btnVerDetalleCompra_Click(object sender, EventArgs e)
        {
            hideSubMenu(); // cerrar el submenu
        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            hideSubMenu(); // cerrar el submenu
        }

        private void btnVerDetalleVenta_Click(object sender, EventArgs e)
        {
            hideSubMenu(); // cerrar el submenu
        }

        private void btnCategoria_Click(object sender, EventArgs e)
        {
            openChildForm(new frmCategoria());
            hideSubMenu(); // cerrar el submenu
        }

        private void btnProducto_Click(object sender, EventArgs e)
        {
            openChildForm(new frmProducto());
            hideSubMenu(); // cerrar el submenu
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            ActivarBoton(sender, RGBColors.color5);
            openChildForm(new frmClientes());
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            ActivarBoton(sender, RGBColors.color6);
            openChildForm(new frmProveedores());
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            ActivarBoton(sender, RGBColors.color7);
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void btnAcercaDe_Click(object sender, EventArgs e)
        {
            ActivarBoton(sender, RGBColors.color8);
            //...
            //Your code
            //..
            hideSubMenu(); // cerrar el submenu
        }

        private void Main_Load(object sender, EventArgs e)
        {
            List<Permiso> lista_permisos = new CN_Permiso().Listar(usuario_actual.id_usuario);

            foreach (Control button_menu in panelMenuLateral.Controls)
            {
                if (button_menu is IconButton)
                {
                    bool encontrado = lista_permisos.Any(m => m.nombre_menu == button_menu.Name); // m cada elemento de la lista
                    if (encontrado == false)
                    {
                        button_menu.Visible = false;
                    }
                }
            }
            lblUsuario.Text = usuario_actual.nombre_completo.ToString();
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            Reinicio();
        }

        private void Reinicio()
        {
            DesactivarBoton();
            bordeIzqBtn.Visible = false;
            iconCurrentChildForm.IconChar = IconChar.Home;
            iconCurrentChildForm.IconColor = Color.Black;
            lblTitleChildForm.Text = "Home";
        }

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void panelBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnNegocio_Click(object sender, EventArgs e)
        {
            openChildForm(new frmNegocio());
            hideSubMenu(); // cerrar el submenu
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Estas seguro de Cerrar Sesión?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                this.Close(); // solo ciera el formulario y no todo los formularios y se mostrara el login
            }
        }
    }
}
