using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmNegocio : Form
    {
        public frmNegocio()
        {
            InitializeComponent();
        }

        public Image ByteToImage(byte[] image_bytes)
        {
            MemoryStream ms = new MemoryStream(); // permite guardar iamgenes en memoria
            ms.Write(image_bytes, 0, image_bytes.Length);

            Image image = new Bitmap(ms); // convierte en imagen
            return image;
        }

        private void frmNegocio_Load(object sender, EventArgs e)
        {
            bool obtenido = true;

            byte[] byte_image = new CN_Negocio().ObtenerLogo(out obtenido);

            // pintar en nuestrop picture box
            if(obtenido)
            {
                picLogo.Image = ByteToImage(byte_image);
            }

            Negocio datos = new CN_Negocio().ObtenerDatos();

            txtNombreNegocio.Text = datos.nombre;
            txtDireccion.Text = datos.direccion;
            txtRUC.Text = datos.ruc;
        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Files|*.jpg;*.jpeg;*.png";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] byte_image = File.ReadAllBytes(ofd.FileName);
                bool respuesta = new CN_Negocio().ActualizarLogo(byte_image, out mensaje);

                if(respuesta)
                {
                    picLogo.Image = ByteToImage(byte_image);
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Negocio obj = new Negocio()
            {
                nombre = txtNombreNegocio.Text,
                ruc = txtRUC.Text,
                direccion = txtDireccion.Text
            };

            bool respuesta = new CN_Negocio().GuardarDatos(obj, out mensaje);
            
            if( respuesta )
            {
                MessageBox.Show("Los cambios fueron guardados correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se puedo guardar los cambios", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
