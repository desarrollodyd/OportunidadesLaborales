using Npgsql;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.IntranetPJ
{
    public class IntranetFichaModel
    {
        string _conexion;
        public IntranetFichaModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<cum_envio> intranetFichaLista, claseError error) IntranetFichaListarJson(string tipo,DateTime inicio,DateTime fin)
        {
            List<cum_envio> lista = new List<cum_envio>();
            var fecha = ManejoNulos.ManageNullDate(inicio).ToString("yyyy-MM-dd HH':'mm':'ss");
            claseError error = new claseError();
            string consulta = @"SELECT ce.env_id,cu.cus_dni,cu.cus_correo, ce.env_nombre, ce.env_tipo, ce.env_fecha_reg, ce.env_fecha_act, ce.env_estado,ced.end_correo_corp,ced.end_correo_pers, ce.fk_cuestionario, ce.fk_usuario
	                            FROM cumplimiento.cum_envio ce
                                join cumplimiento.cum_usuario cu on cu.cus_id = ce.fk_usuario
                                join cumplimiento.cum_envio_det ced on ced.fk_envio = ce.env_id
                                 where cu.cus_tipo=@p0 and ce.env_fecha_reg between '" + ManejoNulos.ManageNullDate(inicio).ToString("yyyy-MM-dd HH':'mm':'ss")+ "' and '" + ManejoNulos.ManageNullDate(fin).ToString("yyyy-MM-dd 23':'59':'59") + "' ;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", tipo);
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullDate(inicio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(fin));
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var ficha = new cum_envio
                                {

                                    env_id = ManejoNulos.ManageNullInteger(dr["env_id"]),
                                    env_nombre = ManejoNulos.ManageNullStr(dr["env_nombre"]),
                                    env_tipo = ManejoNulos.ManageNullStr(dr["env_tipo"]),
                                    env_fecha_reg = ManejoNulos.ManageNullDate(dr["env_fecha_reg"]),
                                    env_fecha_act = ManejoNulos.ManageNullDate(dr["env_fecha_act"]),
                                    env_estado = ManejoNulos.ManageNullStr(dr["env_estado"]),
                                    cus_dni = ManejoNulos.ManageNullStr(dr["cus_dni"]),
                                    cus_correo = ManejoNulos.ManageNullStr(dr["cus_correo"]),
                                    end_correo_corp = ManejoNulos.ManageNullStr(dr["end_correo_corp"]),
                                    end_correo_pers = ManejoNulos.ManageNullStr(dr["end_correo_pers"]),
                                    fk_cuestionario = ManejoNulos.ManageNullInteger(dr["fk_cuestionario"]),
                                    fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]),
                                };

                                lista.Add(ficha);
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
            return (intranetFichaLista: lista, error: error);
        }

        public (List<cum_envio> intranetFichaLista, claseError error) IntranetFichaPostListarJson(DateTime inicio, DateTime fin)
        {
            List<cum_envio> lista = new List<cum_envio>();
            var fecha = ManejoNulos.ManageNullDate(inicio).ToString("yyyy-MM-dd HH':'mm':'ss");
            claseError error = new claseError();
            string consulta = @"SELECT ce.env_id,per.per_nombre,per.per_apellido_pat,per.per_apellido_mat, ce.env_nombre, ce.env_tipo, ce.env_fecha_reg, ce.env_fecha_act, ce.env_estado,ced.end_correo_corp,ced.end_correo_pers, ce.fk_cuestionario, ce.fk_usuario
	                            FROM cumplimiento.cum_envio ce
                                join cumplimiento.cum_usuario cu on cu.cus_id = ce.fk_usuario
                                join cumplimiento.cum_envio_det ced on ced.fk_envio = ce.env_id
                                join seguridad.seg_usuario usu on usu.usu_id = cu.fk_usuario
                                 join marketing.cpj_persona per on per.per_id=usu.fk_persona
                                 where cu.cus_tipo=@p0 and ce.env_fecha_reg between '" + ManejoNulos.ManageNullDate(inicio).ToString("yyyy-MM-dd HH':'mm':'ss") + "' and '" + ManejoNulos.ManageNullDate(fin).ToString("yyyy-MM-dd 23':'59':'59") + "' ;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", "POSTULANTE");
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullDate(inicio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(fin));
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var ficha = new cum_envio
                                {

                                    env_id = ManejoNulos.ManageNullInteger(dr["env_id"]),
                                    env_nombre = ManejoNulos.ManageNullStr(dr["env_nombre"]),
                                    per_nombre = ManejoNulos.ManageNullStr(dr["per_nombre"]),
                                    per_apellido_pat = ManejoNulos.ManageNullStr(dr["per_apellido_pat"]),
                                    per_apellido_mat = ManejoNulos.ManageNullStr(dr["per_apellido_mat"]),
                                    env_tipo = ManejoNulos.ManageNullStr(dr["env_tipo"]),
                                    env_fecha_reg = ManejoNulos.ManageNullDate(dr["env_fecha_reg"]),
                                    env_fecha_act = ManejoNulos.ManageNullDate(dr["env_fecha_act"]),
                                    env_estado = ManejoNulos.ManageNullStr(dr["env_estado"]),
                                    end_correo_corp = ManejoNulos.ManageNullStr(dr["end_correo_corp"]),
                                    end_correo_pers = ManejoNulos.ManageNullStr(dr["end_correo_pers"]),
                                    fk_cuestionario = ManejoNulos.ManageNullInteger(dr["fk_cuestionario"]),
                                    fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]),
                                };

                                lista.Add(ficha);
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
            return (intranetFichaLista: lista, error: error);
        }
        public (List<cum_usuario> intranetCumusuarioLista, claseError error) IntranetUsuarioListarJson(string id,string tipo)
        {
            List<cum_usuario> lista = new List<cum_usuario>();
            claseError error = new claseError();
            string consulta = @"SELECT cus_id, cus_dni, cus_tipo, cus_correo, cus_clave, cus_firma, cus_fecha_reg, cus_fecha_act, cus_estado, fk_usuario
	                            FROM cumplimiento.cum_usuario
	                             where cus_dni=@p0 and cus_tipo=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id);
                    query.Parameters.AddWithValue("@p1", tipo);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var ficha = new cum_usuario
                                {

                                    cus_id = ManejoNulos.ManageNullInteger(dr["cus_id"]),
                                    cus_dni = ManejoNulos.ManageNullStr(dr["cus_dni"]),
                                    cus_tipo = ManejoNulos.ManageNullStr(dr["cus_tipo"]),
                                    cus_correo = ManejoNulos.ManageNullStr(dr["cus_correo"]),
                                    cus_clave = ManejoNulos.ManageNullStr(dr["cus_clave"]),
                                    cus_fecha_reg = ManejoNulos.ManageNullDate(dr["cus_fecha_reg"]),
                                    cus_fecha_act = ManejoNulos.ManageNullDate(dr["cus_fecha_act"]),
                                    cus_estado = ManejoNulos.ManageNullStr(dr["cus_estado"]),
                                    fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]),
                                };

                                lista.Add(ficha);
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
            return (intranetCumusuarioLista: lista, error: error);
        }
        public (List<cum_envio> intranetFichaLista, claseError error) IntranetFichaPostListarxUsuarioJson(int fk_usuario,string tipo)
        {
            List<cum_envio> lista = new List<cum_envio>();
            claseError error = new claseError();
            string consulta = @"SELECT per.per_nombre,per_apellido_pat,per_apellido_mat, ce.env_id, 
                                ce.env_nombre, ce.env_tipo, ce.env_fecha_reg, 
                                ce.env_fecha_act, ce.env_estado,ced.end_correo_corp,ced.end_correo_pers, 
                                ce.fk_cuestionario, ce.fk_usuario,cu.cus_tipo
	                            FROM cumplimiento.cum_envio ce
                                join cumplimiento.cum_usuario cu on cu.cus_id = ce.fk_usuario
                                join cumplimiento.cum_envio_det ced on ced.fk_envio = ce.env_id
								join seguridad.seg_usuario seg on seg.usu_id=cu.fk_usuario
								join marketing.cpj_persona per on per.per_id=seg.fk_persona
								where cu.fk_usuario=@p0 and  cu.cus_tipo=@p1";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_usuario);
                    query.Parameters.AddWithValue("@p1", tipo);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var ficha = new cum_envio
                                {

                                    env_id = ManejoNulos.ManageNullInteger(dr["env_id"]),
                                    env_nombre = ManejoNulos.ManageNullStr(dr["env_nombre"]),
                                    per_nombre = ManejoNulos.ManageNullStr(dr["per_nombre"]),
                                    per_apellido_pat = ManejoNulos.ManageNullStr(dr["per_apellido_pat"]),
                                    per_apellido_mat = ManejoNulos.ManageNullStr(dr["per_apellido_mat"]),
                                    env_tipo = ManejoNulos.ManageNullStr(dr["env_tipo"]),
                                    env_fecha_reg = ManejoNulos.ManageNullDate(dr["env_fecha_reg"]),
                                    env_fecha_act = ManejoNulos.ManageNullDate(dr["env_fecha_act"]),
                                    env_estado = ManejoNulos.ManageNullStr(dr["env_estado"]),
                                    end_correo_corp = ManejoNulos.ManageNullStr(dr["end_correo_corp"]),
                                    end_correo_pers = ManejoNulos.ManageNullStr(dr["end_correo_pers"]),
                                    fk_cuestionario = ManejoNulos.ManageNullInteger(dr["fk_cuestionario"]),
                                    fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]),
                                    cus_tipo = ManejoNulos.ManageNullStr(dr["cus_tipo"]),
                                };

                                lista.Add(ficha);
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
            return (intranetFichaLista: lista, error: error);
        }
    }
}