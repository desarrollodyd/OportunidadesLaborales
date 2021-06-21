using Npgsql;
using SistemaReclutamiento.Entidades.FichaCumplimiento;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class CumUsuRespuestaModel
    {
        string _conexion;
        public CumUsuRespuestaModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<CumUsuRespuestaEntidad> lista, claseError error) CumUsuRespuestaListarxUsuarioJson(int fk_usuario)
        {
            List<CumUsuRespuestaEntidad> lista = new List<CumUsuRespuestaEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT ure_id, ure_dni, ure_respuesta, ure_tipo,
                                ure_orden, ure_calificacion, ure_estado, fk_usu_pregunta, fk_usuario
	                                FROM cumplimiento.cum_usu_respuesta
                                where fk_usuario=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_usuario);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var respuesta = new CumUsuRespuestaEntidad
                                {

                                    ure_id = ManejoNulos.ManageNullInteger(dr["ure_id"]),
                                    ure_dni = ManejoNulos.ManageNullStr(dr["ure_dni"]),
                                    ure_respuesta = ManejoNulos.ManageNullStr(dr["ure_respuesta"]),
                                    ure_tipo = ManejoNulos.ManageNullStr(dr["ure_tipo"]),
                                    ure_orden = ManejoNulos.ManageNullInteger(dr["ure_orden"]),
                                    ure_estado = ManejoNulos.ManageNullStr(dr["ure_estado"]),
                                    fk_usu_pregunta = ManejoNulos.ManageNullInteger(dr["fk_usu_pregunta"]),
                                    fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]),
                                };

                                lista.Add(respuesta);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (lista: lista, error: error);
        }
        public (List<CumUsuRespuestaEntidad> lista, claseError error) CumUsuRespuestaListarxUsuPreguntaJson(int fk_usu_pregunta)
        {
            List<CumUsuRespuestaEntidad> lista = new List<CumUsuRespuestaEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT ure_id, ure_dni, ure_respuesta, ure_tipo,
                                ure_orden, ure_calificacion, ure_estado, fk_usu_pregunta, fk_usuario
	                                FROM cumplimiento.cum_usu_respuesta
                                where fk_usu_pregunta=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_usu_pregunta);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var respuesta = new CumUsuRespuestaEntidad
                                {

                                    ure_id = ManejoNulos.ManageNullInteger(dr["ure_id"]),
                                    ure_dni = ManejoNulos.ManageNullStr(dr["ure_dni"]),
                                    ure_respuesta = ManejoNulos.ManageNullStr(dr["ure_respuesta"]),
                                    ure_tipo = ManejoNulos.ManageNullStr(dr["ure_tipo"]),
                                    ure_orden = ManejoNulos.ManageNullInteger(dr["ure_orden"]),
                                    ure_estado = ManejoNulos.ManageNullStr(dr["ure_estado"]),
                                    fk_usu_pregunta = ManejoNulos.ManageNullInteger(dr["fk_usu_pregunta"]),
                                    fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]),
                                };

                                lista.Add(respuesta);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (lista: lista, error: error);
        }
        public (CumUsuRespuestaEntidad cumUsuRespuesta, claseError error) CumUsuRespuestaIdObtenerJson(int ure_id)
        {
            CumUsuRespuestaEntidad usuRespuesta = new CumUsuRespuestaEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT ure_id, ure_dni, ure_respuesta, ure_tipo,
                                ure_orden, ure_calificacion, ure_estado, fk_usu_pregunta, fk_usuario
	                                FROM cumplimiento.cum_usu_respuesta
                                where ure_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ure_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                usuRespuesta.ure_id = ManejoNulos.ManageNullInteger(dr["ure_id"]);
                                usuRespuesta.ure_dni = ManejoNulos.ManageNullStr(dr["ure_dni"]);
                                usuRespuesta.ure_respuesta = ManejoNulos.ManageNullStr(dr["ure_respuesta"]);
                                usuRespuesta.ure_orden = ManejoNulos.ManageNullInteger(dr["ure_orden"]);
                                usuRespuesta.ure_calificacion = ManejoNulos.ManageNullInteger(dr["ure_calificacion"]);
                                usuRespuesta.ure_estado = ManejoNulos.ManageNullStr(dr["ure_estado"]);
                                usuRespuesta.fk_usu_pregunta = ManejoNulos.ManageNullInteger(dr["fk_usu_pregunta"]);
                                usuRespuesta.fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (cumUsuRespuesta: usuRespuesta, error: error);
        }
        public (int idInsertado, claseError error) CumUsuRespuestaInsertarJson(CumUsuRespuestaEntidad usu_respuesta)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO cumplimiento.cum_usu_respuesta(
	                            ure_dni, ure_respuesta, ure_tipo, ure_orden, 
                                ure_calificacion, ure_estado, fk_usu_pregunta, fk_usuario)
	                            VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7)
                                returning ure_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(usu_respuesta.ure_dni));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(usu_respuesta.ure_respuesta));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(usu_respuesta.ure_tipo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(usu_respuesta.ure_orden));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(usu_respuesta.ure_calificacion));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(usu_respuesta.ure_estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(usu_respuesta.fk_usu_pregunta));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(usu_respuesta.fk_usuario));
                    idInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (idInsertado: idInsertado, error: error);
        }
        public (bool editado, claseError error) CumUsuRespuestaEditarJson(CumUsuRespuestaEntidad usu_respuesta)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE cumplimiento.cum_usu_respuesta
	                            SET  ure_dni=@p0, ure_respuesta=@p1, ure_tipo=@p2, 
                                ure_orden=@p3, ure_calificacion=@p4, 
                                ure_estado=@p5
	                            WHERE ure_id=@p8;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(usu_respuesta.ure_dni));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(usu_respuesta.ure_respuesta));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(usu_respuesta.ure_tipo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(usu_respuesta.ure_orden));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(usu_respuesta.ure_calificacion));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(usu_respuesta.ure_estado));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(usu_respuesta.ure_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (editado: response, error: error);
        }
    }
}