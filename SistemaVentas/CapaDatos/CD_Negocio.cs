using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Negocio
    {
        public Negocio ObtenerDatos()
        {
            Negocio obj = new Negocio();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();

                    string query = "SELECT id_negocio, nombre, ruc, direccion FROM Negocio WHERE id_negocio = 1";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            obj = new Negocio()
                            {
                                id_negocio = int.Parse(dr["id_negocio"].ToString()),
                                nombre = dr["nombre"].ToString(),
                                ruc = dr["ruc"].ToString(),
                                direccion = dr["direccion"].ToString()
                            };

                        }
                    }
                }
            } catch (Exception ex)
            {
                obj = new Negocio();
            }
            return obj;
        }


        public bool GuardarDatos(Negocio obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool respuesta = true;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("UPDATE Negocio SET nombre = @nombre,");
                    query.AppendLine("ruc = @ruc,");
                    query.AppendLine("direccion = @direccion");
                    query.AppendLine("WHERE id_negocio = 1");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("@ruc", obj.ruc);
                    cmd.Parameters.AddWithValue("@direccion", obj.direccion);
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        mensaje = "No se puede guardar los datos";
                        respuesta = false;
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }
            return respuesta;
        }


        public byte[] ObtenerLogo(out bool obtenido)
        {
            obtenido = true;
            byte[] logo_bytes = new byte[0];
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();

                    string query = "SELECT logo FROM Negocio WHERE id_negocio = 1";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            logo_bytes = (byte[])dr["logo"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obtenido = false;
                logo_bytes = new byte[0];
            }

            return logo_bytes;
        }


        public bool actualizarLogo(byte[] image, out string mensaje)
        {
            mensaje = string.Empty;
            bool respuesta = true;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("UPDATE Negocio SET logo = @imagen");
                    query.AppendLine("WHERE id_negocio = 1");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@imagen", image);
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        mensaje = "No se pudo actualizar el Logo";
                        respuesta = false;
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }

            return respuesta;
        }
    }

    
}
