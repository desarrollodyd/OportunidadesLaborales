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
    public class CumEnvioModel
    {
        string _conexion;
        public CumEnvioModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<CumEnvioEntidad> lista, claseError error) CumEnvioListarxUsuarioJson(int fk_usuario)
        {
            List<CumEnvioEntidad> lista = new List<CumEnvioEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT env_id, env_nombre, env_tipo, 
                                env_fecha_reg, env_fecha_act, 
                                env_estado, fk_cuestionario, fk_usuario, env_observacion
	                            FROM cumplimiento.cum_envio
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
                                var envio = new CumEnvioEntidad
                                {
                                    env_id = ManejoNulos.ManageNullInteger(dr["env_id"]),
                                    env_nombre = ManejoNulos.ManageNullStr(dr["env_nombre"]),
                                    env_tipo = ManejoNulos.ManageNullStr(dr["env_tipo"]),
                                    env_fecha_reg = ManejoNulos.ManageNullDate(dr["env_fecha_reg"]),
                                    env_fecha_act = ManejoNulos.ManageNullDate(dr["env_fecha_act"]),
                                    env_estado = ManejoNulos.ManageNullStr(dr["env_estado"]),
                                    fk_cuestionario = ManejoNulos.ManageNullInteger(dr["fk_cuestionario"]),
                                    fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]),
                                    env_observacion = ManejoNulos.ManageNullStr(dr["env_observacion"]),
                                };

                                lista.Add(envio);
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

        public (CumEnvioEntidad cumEnvio, claseError error) CumEnvioIdObtenerJson(int env_id)
        {
            CumEnvioEntidad envio = new CumEnvioEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT env_id, env_nombre, env_tipo, 
                                env_fecha_reg, env_fecha_act, 
                                env_estado, fk_cuestionario, fk_usuario, env_observacion
	                            FROM cumplimiento.cum_envio
                                where env_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", env_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                envio.env_id = ManejoNulos.ManageNullInteger(dr["env_id"]);
                                envio.env_nombre = ManejoNulos.ManageNullStr(dr["env_nombre"]);
                                envio.env_tipo = ManejoNulos.ManageNullStr(dr["env_tipo"]);
                                envio.env_fecha_reg = ManejoNulos.ManageNullDate(dr["env_fecha_reg"]);
                                envio.env_fecha_act = ManejoNulos.ManageNullDate(dr["env_fecha_act"]);
                                envio.env_estado = ManejoNulos.ManageNullStr(dr["env_estado"]);
                                envio.fk_cuestionario = ManejoNulos.ManageNullInteger(dr["fk_cuestionario"]);
                                envio.fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]);
                                envio.env_observacion = ManejoNulos.ManageNullStr(dr["env_observacion"]);
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
            return (cumEnvio: envio, error: error);
        }
        public (int idInsertado, claseError error) CumEnvioInsertarJson(CumEnvioEntidad envio)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO cumplimiento.cum_envio(
	                            env_nombre, env_fecha_reg, 
                                env_fecha_act, env_estado, fk_cuestionario, fk_usuario)
	                            VALUES (@p0, @p1, @p2, @p3, @p4, @p5)
                                returning env_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(envio.env_nombre));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullDate(envio.env_fecha_reg));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(envio.env_fecha_act));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(envio.env_estado));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(envio.fk_cuestionario));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(envio.fk_usuario));

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
        public (bool editado, claseError error) CumEnvioEditarJson(CumEnvioEntidad envio)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE cumplimiento.cum_envio
	                            SET env_fecha_act=@p0, env_estado=@p1
	                            WHERE env_id=@p2;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullDate(envio.env_fecha_act));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(envio.env_estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(envio.env_id));

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
        public (bool editado, claseError error) CumEnvioEditarObservacionJson(CumEnvioEntidad envio)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE cumplimiento.cum_envio
	                            SET env_observacion=@p1
	                            WHERE env_id=@p2;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(envio.env_observacion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(envio.env_id));

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