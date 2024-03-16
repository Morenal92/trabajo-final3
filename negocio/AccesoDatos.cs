using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class AccesoDatos
    {
      
            private SqlConnection conexion;
            private SqlCommand comando;
            private SqlDataReader lector;

            public SqlDataReader Lector
            {
                get { return lector; }
            }

            public AccesoDatos()
            {
                conexion = new SqlConnection("server=MORENA\\SQLEXPRESS; database= CATALOGO_DB; integrated security = true");
                comando = new SqlCommand();
            }
            public void SetearConsulta(string consulta)
            {
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = consulta;
            }
            public void setearProcedimiento(string sp)
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = sp;
            }
            public void EjectutarLectura()
            {
                comando.Connection = conexion;
                try
                {
                    conexion.Open();
                    lector = comando.ExecuteReader();

                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            public void ejecutarAccion()
            {
                comando.Connection = conexion;
                try
                {
                    conexion.Open();
                    // aca inserto datos entonces es:
                    comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }

            public int ejecutarAccionScalar()
            {
                comando.Connection = conexion;
                try
                {
                    conexion.Open();
                    // aca inserto datos entonces es:
                    return int.Parse(comando.ExecuteScalar().ToString());

                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }
            public void setearParamentro(string nombre, object valor)
            {
                comando.Parameters.AddWithValue(nombre, valor);
            }
            public void agregarParametro(string nombre, object valor)
            {
                comando.Parameters.AddWithValue(nombre, valor);
            }
            public void cerrarConexion()
            {
                if (lector != null)
                    lector.Close();
                conexion.Close();

            }
        }
    }
}
