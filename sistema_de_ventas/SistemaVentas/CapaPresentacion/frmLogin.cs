using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        // Funcionalidad de mover el formulario con el mouse
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void Login2_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtUser.Text != "USUARIO")
            {
                if(txtPassword.Text != "CONTRASEÑA")
                {
                    Usuario oUsuario = new CN_Usuario().Listar().Where(u => u.documento == txtUser.Text && u.clave == txtPassword.Text).FirstOrDefault();
                    if (oUsuario != null)
                    {
                        Main form = new Main();
                        form.Show();
                        form.FormClosed += Logout; // sobrecargamos el FormClosed con el metodo de cerrar sesion
                        this.Hide();
                    }
                    else
                    {
                        //MessageBox.Show("Usuario Incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        msgError("Usuario o contraseña incorrecta \nPor favor, ingrese nuevamente");
                        txtPassword.Text = "CONTRASEÑA";
                        txtPassword.Focus();
                    }
                }
                else
                {
                    msgError("Por favor, ingrese su contraseña");
                }
            }
            else
            {
                msgError("Por favor, ingrese su usuario");
            }
        }

        // metodo para mostrar el mensaje de error
        private void msgError(string msg)
        {
            lblMensajeError.Text = msg;
            lblMensajeError.Visible = true;
        }

        private void txtUser_Enter(object sender, EventArgs e) //
        {
            if (txtUser.Text == "USUARIO")
            {
                txtUser.Text = "";
                txtUser.ForeColor = Color.LightGray;
            }
        }

        private void txtUser_Leave(object sender, EventArgs e)
        {
            if (txtUser.Text == "")
            {
                txtUser.Text = "USUARIO";
                txtUser.ForeColor = Color.DimGray;
            }
        }

        //
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if(txtPassword.Text == "CONTRASEÑA")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.LightGray;
                txtPassword.UseSystemPasswordChar = true; 
            }
        }

        // cambia el color de la pregutna para contraseña 
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "CONTRASEÑA";
                txtPassword.ForeColor = Color.DimGray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        // mover el form login
        private void Login2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        // mover el form login desde la parte del logo
        private void panelLogoLogin_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        // metodo para cerrar sesion
        private void Logout(object sender, FormClosedEventArgs e)
        {
            txtPassword.Text = "CONTRASEÑA";
            txtPassword.UseSystemPasswordChar=false;
            txtUser.Text = "USUARIO";
            lblMensajeError.Visible = false;
            this.Show();
            //txtUser.Focus();
        }
    }
}
