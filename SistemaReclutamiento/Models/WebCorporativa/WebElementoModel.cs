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
            string consulta = @"SELECT elm_id, elm_contenido, fk_menu, fk_tipo, elm_orden
	                            FROM web_corporativa.web_elemento where fk_menu=@p0 order by elm_orden;";
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

                                    elm_id = ManejoNulos.ManageNullInteger(dr["elm_id"]),
                                    elm_contenido = ManejoNulos.ManageNullStr(dr["elm_contenido"]),
                                    elm_orden = ManejoNulos.ManageNullInteger(dr["elm_orden"]),
                                    fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]),
                                    fk_tipo = ManejoNulos.ManageNullInteger(dr["fk_tipo"]),
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
            string consulta = @"SELECT elm_id, elm_contenido, fk_menu, fk_tipo, elm_orden
	                            FROM web_corporativa.web_elemento where elm_id=@p0;";
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

                                WebElemento.elm_id = ManejoNulos.ManageNullInteger(dr["elm_id"]);
                                WebElemento.elm_contenido = ManejoNulos.ManageNullStr(dr["elm_contenido"]);
                                WebElemento.fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]);
                                WebElemento.fk_tipo = ManejoNulos.ManageNullInteger(dr["fk_tipo"]);
                                WebElemento.elm_orden = ManejoNulos.ManageNullInteger(dr["elm_orden"]);
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
	                            elm_contenido, fk_menu, fk_tipo, elm_orden)
	                            VALUES (@p0, @p1, @p2, @p3);
                                            returning elem_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(WebElemento.elm_contenido));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(WebElemento.fk_menu));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(WebElemento.fk_tipo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(WebElemento.elm_orden));
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
	                            SET  elm_contenido=@p0, fk_menu=@p1, fk_tipo=@p2, elm_orden=@p3
	                            WHERE elm_id=@p4;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(WebElemento.elm_contenido));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(WebElemento.fk_menu));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(WebElemento.fk_tipo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(WebElemento.elm_orden));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(WebElemento.elm_id));
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
	                                WHERE elm_id=@p0;";
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
	                            SET elm_orden=@p0
	                            WHERE elm_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(WebElemento.elm_orden));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(WebElemento.elm_id));
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
    }
}