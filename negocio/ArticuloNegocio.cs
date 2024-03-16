using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=MORENA\\SQLEXPRESS; database= CATALOGO_DB; integrated security = true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select A.Id, Codigo , Nombre, M.Descripcion Marca, C.Descripcion Categoria, A.Descripcion, ImagenUrl ,Precio,A.IdMarca, A.IdCategoria\r\nfrom ARTICULOS A, MARCAS M , CATEGORIAS C  \r\nWHERE(A.IdMarca = M.Id) and A.IdCategoria = C.Id";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)lector["Id"];
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    if (!(lector.IsDBNull(lector.GetOrdinal("ImagenUrl"))))
                        aux.ImagenUrl = (string)lector["ImagenUrl"];

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)lector["IdMarca"];


                    aux.Marca.Descripcion = (string)lector["Marca"];

                    aux.Precio = (decimal)lector["Precio"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)lector["IdCategoria"];

                    aux.Categoria.Descripcion = (string)lector["Categoria"];
                    // aux.Categoria.Id = (int)lector["IdCategoria"];


                    lista.Add(aux);
                }
                conexion.Close();
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }



        }


        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("Insert into Articulos (Codigo, Nombre, Descripcion, Precio , IdMarca , IdCategoria, ImagenUrl) values ('" + nuevo.Codigo + "',' " + nuevo.Nombre + " ', ' " + nuevo.Descripcion + " ',' " + nuevo.Precio + " ',@IdMarca , @IdCategoria, @ImagenUrl)");
                datos.setearParamentro("@IdMarca", nuevo.Marca.Id);
                datos.setearParamentro("@IdCategoria", nuevo.Categoria.Id);
                datos.setearParamentro("@ImagenUrl", nuevo.ImagenUrl);
                // datos.setearParamentro("@IdCategoria", nuevo.Categoria.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void modificar(Articulo arti)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("update ARTICULOS set Codigo = @codigo , Nombre = @nombre, Descripcion = @descripcion ,IdMarca= @IdMarca, IdCategoria = @IdCategoria,  ImagenUrl = @img , Precio = @precio where Id = @Id");
                datos.setearParamentro("@codigo", arti.Codigo);
                datos.setearParamentro("@nombre", arti.Nombre);
                datos.setearParamentro("@descripcion", arti.Descripcion);
                datos.setearParamentro("@IdMarca", arti.Marca.Id);
                datos.setearParamentro("@IdCategoria", arti.Categoria.Id);
                datos.setearParamentro("@img", arti.ImagenUrl);
                datos.setearParamentro("@precio", arti.Precio);
                datos.setearParamentro("@Id", arti.Id);

                datos.ejecutarAccion();
            }

            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminar(int Id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearConsulta("delete from ARTICULOS where Id = @Id");
                datos.setearParamentro("@Id", Id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select A.Id, Codigo , Nombre, M.Descripcion Marca, C.Descripcion Categoria, A.Descripcion, ImagenUrl ,Precio,A.IdMarca, A.IdCategoria\r\nfrom ARTICULOS A, MARCAS M , CATEGORIAS C  \r\nWHERE(A.IdMarca = M.Id) and A.IdCategoria = C.Id And ";

                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Menor a ":
                            consulta += "Precio < " + filtro;
                            break;

                        case "Mayor a ":
                            consulta += "Precio > " + filtro;
                            break;

                        default:
                            consulta += "Precio = " + filtro;
                            break;
                    }

                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;

                        case "Termina con":
                            consulta += "Nombre like  '%" + filtro + "'";
                            break;

                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "M.Descripcion like '" + filtro + "%'";
                            break;

                        case "Termina con":
                            consulta += "M.Descripcion like  '%" + filtro + "'";
                            break;

                        default:
                            consulta += "M.Descripcion like '%" + filtro + "%'";
                            break;
                    }

                }
                datos.SetearConsulta(consulta);
                datos.EjectutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector.IsDBNull(datos.Lector.GetOrdinal("ImagenUrl"))))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];


                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.Precio = (decimal)datos.Lector["Precio"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];

                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    // aux.Categoria.Id = (int)lector["IdCategoria"];


                    lista.Add(aux);
                }


                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

}
}
