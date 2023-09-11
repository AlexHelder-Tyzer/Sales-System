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
    public class CD_Proveedor
    {
        public List<Proveedor> Listar()
        {
            List<Proveedor> lista = new List<Proveedor>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT id_proveedor, documento, razon_social, correo, telefono, estado FROM Proveedor ");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Proveedor()
                            {
                                id_proveedor = Convert.ToInt32(dr["id_proveedor"]),
                                documento = dr["documento"].ToString(),
                                razon_social = dr["razon_social"].ToString(),
                                correo = dr["correo"].ToString(),
                                telefono = dr["telefono"].ToString(),
                                estado = Convert.ToBoolean(dr["estado"]),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Proveedor>();
                }
            }
            return lista;
        }


        public int Registrar(Proveedor obj, out string Mensaje)
        {
            int id_proveedor_generado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarProveedor", oConexion);
                    cmd.Parameters.AddWithValue("documento", obj.documento);
                    cmd.Parameters.AddWithValue("razon_social", obj.razon_social);
                    cmd.Parameters.AddWithValue("correo ", obj.correo);
                    cmd.Parameters.AddWithValue("telefono", obj.telefono);
                    cmd.Parameters.AddWithValue("estado", obj.estado);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    id_proveedor_generado = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                    Mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                id_proveedor_generado = 0;
                Mensaje = ex.Message;
            }

            return id_proveedor_generado;
        }


        public bool Editar(Proveedor obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_EditarProveedor", oConexion);
                    cmd.Parameters.AddWithValue("id_proveedor", obj.id_proveedor);
                    cmd.Parameters.AddWithValue("documento", obj.documento);
                    cmd.Parameters.AddWithValue("razon_social", obj.razon_social);
                    cmd.Parameters.AddWithValue("correo ", obj.correo);
                    cmd.Parameters.AddWithValue("telefono", obj.telefono);
                    cmd.Parameters.AddWithValue("estado", obj.estado);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    Mensaje = cmd.Parameters["mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }


        public bool Eliminar(Proveedor obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarProveedor", oConexion);
                    cmd.Parameters.AddWithValue("id_proveedor", obj.id_proveedor);

                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                    Mensaje = cmd.Parameters["mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }
    }
}
