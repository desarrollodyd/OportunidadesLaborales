using Npgsql;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class CumUsuarioExcelModel
    {
        string _conexion;
        public CumUsuarioExcelModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<CumUsuarioExcelEntidad> lista, claseError error) CumUsuarioExcelListarJson()
        {
            List<CumUsuarioExcelEntidad> lista = new List<CumUsuarioExcelEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT cue_id, cue_numdoc, cue_correo,
                            cue_fecha_reg, cue_fecha_act
	                            FROM cumplimiento.cum_usuario_excel;";
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
                                var usuario = new CumUsuarioExcelEntidad
                                {

                                    cue_id = ManejoNulos.ManageNullInteger(dr["cue_id"]),
                                    cue_numdoc = ManejoNulos.ManageNullStr(dr["cue_numdoc"]),
                                    cue_correo = ManejoNulos.ManageNullStr(dr["cue_correo"]),
                                    cue_fecha_reg = ManejoNulos.ManageNullDate(dr["cue_fecha_reg"]),
                                    cue_fecha_act = ManejoNulos.ManageNullDate(dr["cue_fecha_act"]),
                                };

                                lista.Add(usuario);
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
        public (CumUsuarioExcelEntidad cumUsuario, claseError error) CumUsuarioExcelIdObtenerJson(int cue_id)
        {
            CumUsuarioExcelEntidad usuario = new CumUsuarioExcelEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT cue_id, cue_numdoc, cue_correo,
                            cue_fecha_reg, cue_fecha_act
	                            FROM cumplimiento.cum_usuario_excel
                                where cue_id=@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", cue_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                usuario.cue_id = ManejoNulos.ManageNullInteger(dr["cue_id"]);
                                usuario.cue_numdoc = ManejoNulos.ManageNullStr(dr["cue_numdoc"]);
                                usuario.cue_correo = ManejoNulos.ManageNullStr(dr["cue_correo"]);
                                usuario.cue_fecha_reg = ManejoNulos.ManageNullDate(dr["cue_fecha_reg"]);
                                usuario.cue_fecha_act = ManejoNulos.ManageNullDate(dr["cue_fecha_act"]);
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
            return (cumUsuario: usuario, error: error);
        }
        public (int idInsertado, claseError error) CumUsuarioExcelInsertarJson(CumUsuarioExcelEntidad usuario)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO cumplimiento.cum_usuario_excel(
	                            cue_numdoc, cue_correo, cue_fecha_reg)
	                            VALUES (@p0, @p1, @p2)
                                returning cue_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(usuario.cue_numdoc));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(usuario.cue_correo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(usuario.cue_fecha_reg));
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
        public (bool editado, claseError error) CumUsuarioExcelEditarJson(CumUsuarioExcelEntidad usuario)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE cumplimiento.cum_usuario_excel
	                            SET cue_numdoc=@p0, cue_correo=@p1, cue_fecha_act=@p2
	                            WHERE cue_id=@p3;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(usuario.cue_numdoc));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(usuario.cue_correo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(usuario.cue_fecha_act));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(usuario.cue_id));
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