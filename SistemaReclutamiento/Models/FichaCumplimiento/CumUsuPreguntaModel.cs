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
    public class CumUsuPreguntaModel
    {
        string _conexion;
        public CumUsuPreguntaModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<CumUsuPreguntaEntidad> lista, claseError error) CumUsuPreguntaListarxUsuarioJson(int fk_usuario, int fk_envio)
        {
            List<CumUsuPreguntaEntidad> lista = new List<CumUsuPreguntaEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT upr_id, upr_dni, upr_pregunta, upr_tipo, upr_fecha_reg, 
                                upr_fecha_act, upr_estado, fk_pregunta, fk_usuario
	                            FROM cumplimiento.cum_usu_pregunta
                                where fk_usuario=@p0 and fk_envio=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_usuario);
                    query.Parameters.AddWithValue("@p1", fk_envio);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var pregunta = new CumUsuPreguntaEntidad
                                {

                                    upr_id = ManejoNulos.ManageNullInteger(dr["upr_id"]),
                                    upr_dni = ManejoNulos.ManageNullStr(dr["upr_dni"]),
                                    fk_pregunta = ManejoNulos.ManageNullInteger(dr["fk_pregunta"]),
                                    upr_tipo = ManejoNulos.ManageNullStr(dr["upr_tipo"]),
                                    upr_fecha_reg = ManejoNulos.ManageNullDate(dr["upr_fecha_reg"]),
                                    upr_fecha_act = ManejoNulos.ManageNullDate(dr["upr_fecha_act"]),
                                    upr_estado = ManejoNulos.ManageNullStr(dr["upr_estado"]),
                                    upr_pregunta = ManejoNulos.ManageNullStr(dr["upr_pregunta"]),
                                    fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]),
                                };

                                lista.Add(pregunta);
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
        public (CumUsuPreguntaEntidad cumUsuPregunta, claseError error) CumUsuPreguntaIdObtenerJson(int upr_id)
        {
            CumUsuPreguntaEntidad usuPregunta = new CumUsuPreguntaEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT SELECT upr_id, upr_dni, upr_pregunta, upr_tipo, upr_fecha_reg, 
                                upr_fecha_act, upr_estado, fk_pregunta, fk_usuario
	                            FROM cumplimiento.cum_usu_pregunta
                                where upr_id=@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", upr_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                usuPregunta.upr_id = ManejoNulos.ManageNullInteger(dr["upr_id"]);
                                usuPregunta.upr_dni = ManejoNulos.ManageNullStr(dr["upr_dni"]);
                                usuPregunta.upr_pregunta = ManejoNulos.ManageNullStr(dr["upr_pregunta"]);
                                usuPregunta.upr_tipo = ManejoNulos.ManageNullStr(dr["upr_tipo"]);
                                usuPregunta.upr_fecha_reg = ManejoNulos.ManageNullDate(dr["upr_fecha_reg"]);
                                usuPregunta.upr_fecha_act = ManejoNulos.ManageNullDate(dr["upr_fecha_act"]);
                                usuPregunta.upr_estado = ManejoNulos.ManageNullStr(dr["upr_estado"]);
                                usuPregunta.fk_pregunta = ManejoNulos.ManageNullInteger(dr["fk_pregunta"]);
                                usuPregunta.fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]);


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
            return (cumUsuPregunta: usuPregunta, error: error);
        }
        public (int idInsertado, claseError error) CumUsuPreguntaInsertarJson(CumUsuPreguntaEntidad usu_pregunta)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO cumplimiento.cum_usu_pregunta(
	                            upr_dni, upr_pregunta, upr_tipo, 
                                upr_fecha_reg, upr_estado, 
                                fk_pregunta, fk_usuario,fk_envio)
	                            VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6,@p7)
                                returning upr_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(usu_pregunta.upr_dni));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(usu_pregunta.upr_pregunta));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(usu_pregunta.upr_tipo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(usu_pregunta.upr_fecha_reg));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(usu_pregunta.upr_estado));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(usu_pregunta.fk_pregunta));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(usu_pregunta.fk_usuario));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(usu_pregunta.fk_envio));
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
        public (bool editado, claseError error) CumUsuPreguntaEditarJson(CumUsuPreguntaEntidad usu_pregunta)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE cumplimiento.cum_usu_pregunta
	                            SET upr_dni=@p0, upr_pregunta=@p1, 
                                upr_fecha_act=@p2, 
                                fk_pregunta=@p3
	                            WHERE upr_id=@p5;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(usu_pregunta.upr_dni));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(usu_pregunta.upr_pregunta));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(usu_pregunta.upr_fecha_act));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(usu_pregunta.fk_pregunta));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(usu_pregunta.upr_id));
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