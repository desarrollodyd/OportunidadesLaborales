using Npgsql;
using SistemaReclutamiento.Entidades.WebCorporativa;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.WebCorporativa
{
    public class WebElementoModel
    {
        string _conexion;
        public WebElementoModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<WebElementoEntidad> lista, claseError error) WebElementoListarxMenuIDJson(int menu_id)
        {
            List<WebElementoEntidad> lista = new List<WebElementoEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT elem_id, elem_contenido, fk_menu, fk_tipo_elemento, elem_orden,tipo_nombre,elem_estado
                                FROM web_corporativa.web_elemento join web_corporativa.web_tipo_elemento
                                on web_corporativa.web_elemento.fk_tipo_elemento=web_corporativa.web_tipo_elemento.tipo_id
                                where fk_menu=@p0 order by elem_orden;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", menu_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var Elemento = new WebElementoEntidad
                                {

                                    elem_id = ManejoNulos.ManageNullInteger(dr["elem_id"]),
                                    elem_contenido = ManejoNulos.ManageNullStr(dr["elem_contenido"]),
                                    elem_orden = ManejoNulos.ManageNullInteger(dr["elem_orden"]),
                                    fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]),
                                    fk_tipo_elemento = ManejoNulos.ManageNullInteger(dr["fk_tipo_elemento"]),
                                    tipo_nombre = ManejoNulos.ManageNullStr(dr["tipo_nombre"]),
                                    elem_estado = ManejoNulos.ManageNullStr(dr["elem_estado"]),
                                };
                                lista.Add(Elemento);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (lista, error: error);
        }


        public (WebElementoEntidad elemento, claseError error) WebElementoIdObtenerJson(int elemento_id)
        {
            WebElementoEntidad WebElemento = new WebElementoEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT elem_id, elem_contenido, fk_menu, fk_tipo_elemento, elem_orden,elem_estado
	                            FROM web_corporativa.web_elemento where elem_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", elemento_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                WebElemento.elem_id = ManejoNulos.ManageNullInteger(dr["elem_id"]);
                                WebElemento.elem_contenido = ManejoNulos.ManageNullStr(dr["elem_contenido"]);
                                WebElemento.fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]);
                                WebElemento.fk_tipo_elemento = ManejoNulos.ManageNullInteger(dr["fk_tipo_elemento"]);
                                WebElemento.elem_orden = ManejoNulos.ManageNullInteger(dr["elem_orden"]);
                                WebElemento.elem_estado = ManejoNulos.ManageNullStr(dr["elem_estado"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (elemento: WebElemento, error: error);
        }
        public (int idInsertado, claseError error) WebElementoInsertarJson(WebElementoEntidad WebElemento)
        {
            //bool response = false;
            int idWebElementoInsertado = 0;
            string consulta = @"INSERT INTO web_corporativa.web_elemento(
	                            elem_contenido, fk_menu, fk_tipo_elemento, elem_orden,elem_estado)
	                            VALUES (@p0, @p1, @p2, @p3,@p4)
                                            returning elem_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(WebElemento.elem_contenido));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(WebElemento.fk_menu));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(WebElemento.fk_tipo_elemento));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(WebElemento.elem_orden));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(WebElemento.elem_estado));
                    idWebElementoInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idInsertado: idWebElementoInsertado, error: error);
        }
        public (bool WebElementoEditado, claseError error) WebElementoEditarJson(WebElementoEntidad WebElemento)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE web_corporativa.web_elemento
	                            SET  elem_contenido=@p0, fk_menu=@p1, fk_tipo_elemento=@p2, elem_orden=@p3,elem_estado=@p4
	                            WHERE elem_id=@p5;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(WebElemento.elem_contenido));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(WebElemento.fk_menu));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(WebElemento.fk_tipo_elemento));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(WebElemento.elem_orden));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(WebElemento.elem_estado));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(WebElemento.elem_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (WebElementoEditado: response, error: error);
        }
        public (bool eliminado, claseError error) WebElementoEliminarJson(int elemento_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM web_corporativa.web_elemento
	                                WHERE elem_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(elemento_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }

            return (eliminado: response, error: error);
        }
         public (bool reordenado, claseError error) WebElementoEditarOrdenJson(WebElementoEntidad WebElemento)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE web_corporativa.web_elemento
	                            SET elem_orden=@p0
	                            WHERE elem_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(WebElemento.elem_orden));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(WebElemento.elem_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (reordenado: response, error: error);
        }

        //////////////////////////////////////////////////////////////
        ///

        public (List<WebElementoEntidad> lista, claseError error) WebElementoListarxMenuIDxtipoJson(int menu_id,int tipo)
        {
            List<WebElementoEntidad> lista = new List<WebElementoEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT elem_id, elem_contenido, fk_menu, fk_tipo_elemento, elem_orden,tipo_nombre,elem_estado
                                FROM web_corporativa.web_elemento join web_corporativa.web_tipo_elemento
                                on web_corporativa.web_elemento.fk_tipo_elemento=web_corporativa.web_tipo_elemento.tipo_id
                                where fk_menu=@p0 and fk_tipo_elemento=@p1 order by elem_orden;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", menu_id);
                    query.Parameters.AddWithValue("@p1", menu_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var Elemento = new WebElementoEntidad
                                {

                                    elem_id = ManejoNulos.ManageNullInteger(dr["elem_id"]),
                                    elem_contenido = ManejoNulos.ManageNullStr(dr["elem_contenido"]),
                                    elem_orden = ManejoNulos.ManageNullInteger(dr["elem_orden"]),
                                    fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]),
                                    fk_tipo_elemento = ManejoNulos.ManageNullInteger(dr["fk_tipo_elemento"]),
                                    tipo_nombre = ManejoNulos.ManageNullStr(dr["tipo_nombre"]),
                                    elem_estado = ManejoNulos.ManageNullStr(dr["elem_estado"]),
                                };
                                lista.Add(Elemento);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (lista, error: error);
        }
    }
}