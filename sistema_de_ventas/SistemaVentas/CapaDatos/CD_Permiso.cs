using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CapaDatos
{
    public class CD_Permiso
    {
        public List<Permiso> Listar(int id_usuario)
        {
            List<Permiso> lista = new List<Permiso>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder(); // permite saltos de linea
                    query.AppendLine("select P.id_rol, P.nombre_menu from Permiso P");
                    query.AppendLine("INNER JOIN Rol R ON R.id_rol = P.id_rol");
                    query.AppendLine("INNER JOIN Usuario U ON U.id_rol = R.id_rol");
                    query.AppendLine("WHERE U.id_usuario = @id_usuario");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@id_usuario", id_usuario); // reemplaza el valor ingresado por parametro
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Permiso()
                            {
                                oRol = new Rol() { id_rol = Convert.ToInt32(dr["id_rol"])},
                                nombre_menu = dr["nombre_menu"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Permiso>();
                }
            }
            return lista;
        }
    }
}
