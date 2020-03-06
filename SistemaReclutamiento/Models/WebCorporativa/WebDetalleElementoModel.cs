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
    public class WebDetalleElementoModel
    {
        string _conexion;
        public WebDetalleElementoModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<WebDetalleElementoEntidad> listadetalles, claseError error) WebDetalleElementoListarJson()
        {
            List<WebDetalleElementoEntidad> lista = new List<WebDetalleElementoEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT detel_id, fk_elemento, 
                                detel_titulo, detel_subtitulo, detel_parrafo, 
                                detel_imagen, detel_imagen_detalle, detel_estado, detel_orden
	                            FROM web_corporativa.web_detalle_elemento order by detel_orden;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var detalle = new WebDetalleElementoEntidad
                                {

                                    detel_id = ManejoNulos.ManageNullInteger(dr["detel_id"]),
                                    fk_elemento = ManejoNulos.ManageNullInteger(dr["fk_elemento"]),
                                    detel_titulo = ManejoNulos.ManageNullStr(dr["detel_titulo"]),
                                    detel_subtitulo = ManejoNulos.ManageNullStr(dr["detel_subtitulo"]),
                                    detel_parrafo = ManejoNulos.ManageNullStr(dr["detel_parrafo"]),
                                    detel_imagen = ManejoNulos.ManageNullStr(dr["detel_imagen"]),
                                    detel_imagen_detalle = ManejoNulos.ManageNullStr(dr["detel_imagen_detalle"]),
                                    detel_estado = ManejoNulos.ManageNullStr(dr["detel_estado"]),
                                    detel_orden = ManejoNulos.ManageNullInteger(dr["detel_orden"]),
                                };

                                lista.Add(detalle);
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
            return (listadetalles: lista, error: error);
        }

        public (List<WebDetalleElementoEntidad> listadetalle, claseError error) WebDetalleElementoListarxElementoIDJson(int elemento_id)
        {
            List<WebDetalleElementoEntidad> lista = new List<WebDetalleElementoEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT detel_id, fk_elemento, 
                                detel_titulo, detel_subtitulo, detel_parrafo, 
                                detel_imagen, detel_imagen_detalle, detel_estado, detel_orden,fk_tipo_elemento
                                FROM web_corporativa.web_detalle_elemento
                                join web_corporativa.web_elemento
                                on web_corporativa.web_detalle_elemento.fk_elemento=web_corporativa.web_elemento.elem_id
                                where fk_elemento=@p0 order by detel_orden";
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
                                var detalle = new WebDetalleElementoEntidad
                                {
                                    detel_id = ManejoNulos.ManageNullInteger(dr["detel_id"]),
                                    fk_elemento = ManejoNulos.ManageNullInteger(dr["fk_elemento"]),
                                    detel_titulo = ManejoNulos.ManageNullStr(dr["detel_titulo"]),
                                    detel_subtitulo = ManejoNulos.ManageNullStr(dr["detel_subtitulo"]),
                                    detel_parrafo = ManejoNulos.ManageNullStr(dr["detel_parrafo"]),
                                    detel_imagen = ManejoNulos.ManageNullStr(dr["detel_imagen"]),
                                    detel_imagen_detalle = ManejoNulos.ManageNullStr(dr["detel_imagen_detalle"]),
                                    detel_estado = ManejoNulos.ManageNullStr(dr["detel_estado"]),
                                    detel_orden = ManejoNulos.ManageNullInteger(dr["detel_orden"]),
                                    fk_tipo = ManejoNulos.ManageNullInteger(dr["fk_tipo_elemento"]),
                                };

                                lista.Add(detalle);
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
            return (listadetalle: lista, error: error);
        }

        public (WebDetalleElementoEntidad detalle, claseError error) WebDetalleElementoIdObtenerJson(int detel_id)
        {
            WebDetalleElementoEntidad WebImagen = new WebDetalleElementoEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT det.detel_id, det.fk_elemento, 
			                    det.detel_titulo, det.detel_subtitulo, det.detel_parrafo, 
			                    det.detel_imagen, det.detel_imagen_detalle, det.detel_estado, det.detel_orden, elm.fk_tipo_elemento
			                    FROM web_corporativa.web_detalle_elemento as det join web_corporativa.web_elemento as elm
			                    on det.fk_elemento=elm.elem_id  where det.detel_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", detel_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                WebImagen.detel_id = ManejoNulos.ManageNullInteger(dr["detel_id"]);
                                WebImagen.fk_elemento = ManejoNulos.ManageNullInteger(dr["fk_elemento"]);
                                WebImagen.detel_titulo = ManejoNulos.ManageNullStr(dr["detel_titulo"]);
                                WebImagen.detel_subtitulo = ManejoNulos.ManageNullStr(dr["detel_subtitulo"]);
                                WebImagen.detel_parrafo = ManejoNulos.ManageNullStr(dr["detel_parrafo"]);
                                WebImagen.detel_imagen = ManejoNulos.ManageNullStr(dr["detel_imagen"]);
                                WebImagen.detel_imagen_detalle = ManejoNulos.ManageNullStr(dr["detel_imagen_detalle"]);
                                WebImagen.detel_estado = ManejoNulos.ManageNullStr(dr["detel_estado"]);
                                WebImagen.detel_orden = ManejoNulos.ManageNullInteger(dr["detel_orden"]);
                                WebImagen.fk_tipo = ManejoNulos.ManageNullInteger(dr["fk_tipo_elemento"]);
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
            return (detalle: WebImagen, error: error);
        }
        public (int idInsertado, claseError error) WebDetalleElementoInsertarJson(WebDetalleElementoEntidad WebDetalleElemento)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"
            INSERT INTO web_corporativa.web_detalle_elemento(
	                    fk_elemento, detel_titulo, detel_subtitulo, detel_parrafo, detel_imagen, detel_imagen_detalle, detel_estado, detel_orden)
	                    VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7)
                returning detel_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(WebDetalleElemento.fk_elemento));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_titulo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_subtitulo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_parrafo));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_imagen));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_imagen_detalle));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_estado));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(WebDetalleElemento.detel_orden));
                    idInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idInsertado: idInsertado, error: error);
        }

        public (bool WebDetalleElementoEditado, claseError error) WebDetalleElementoEditarJson(WebDetalleElementoEntidad WebDetalleElemento)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE web_corporativa.web_detalle_elemento
	                            SET  fk_elemento=@p0, detel_titulo=@p1, 
                            detel_subtitulo=@p2, detel_parrafo=@p3, detel_imagen=@p4, 
                            detel_imagen_detalle=@p5, detel_estado=@p6, detel_orden=@p7
	                            WHERE detel_id=@p8;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(WebDetalleElemento.fk_elemento));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_titulo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_subtitulo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_parrafo));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_imagen));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_imagen_detalle));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(WebDetalleElemento.detel_estado));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(WebDetalleElemento.detel_orden));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(WebDetalleElemento.detel_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (WebDetalleElementoEditado: response, error: error);
        }
        public (bool eliminado, claseError error) WebDetalleElementoEliminarJson(int detel_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM web_corporativa.web_detalle_elemento
	                            WHERE detel_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(detel_id));
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
        public (bool WebDetElementoReordenado, claseError error) WebDetalleElementoEditarOrdenJson(WebDetalleElementoEntidad WebDetalleElemento)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE web_corporativa.web_detalle_elemento
	                            SET detel_orden=@p0
	                            WHERE detel_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(WebDetalleElemento.detel_orden));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(WebDetalleElemento.detel_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (WebDetElementoReordenado: response, error: error);
        }
    }
}