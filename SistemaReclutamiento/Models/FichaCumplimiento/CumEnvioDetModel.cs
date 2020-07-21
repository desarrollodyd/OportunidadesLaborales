using Npgsql;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.FichaCumplimiento
{
    public class CumEnvioDetModel
    {
        string _conexion;
        public CumEnvioDetModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (CumEnvioDetalleEntidad cumEnvioDet, claseError error) CumEnvioDetalleObtenerxEnvioJson(int fk_envio)
        {
            CumEnvioDetalleEntidad envioDetalle = new CumEnvioDetalleEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT end_id, end_dni, 
                                end_correo_corp, end_correo_pers, end_fecha_reg,
                                end_fecha_act, end_estado, fk_envio
	                                FROM cumplimiento.cum_envio_det
                                where fk_envio=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_envio);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                envioDetalle.end_id = ManejoNulos.ManageNullInteger(dr["end_id"]);
                                envioDetalle.end_dni = ManejoNulos.ManageNullStr(dr["end_dni"]);
                                envioDetalle.end_correo_corp = ManejoNulos.ManageNullStr(dr["end_correo_corp"]);
                                envioDetalle.end_correo_pers = ManejoNulos.ManageNullStr(dr["end_correo_pers"]);
                                envioDetalle.end_fecha_reg = ManejoNulos.ManageNullDate(dr["end_fecha_reg"]);
                                envioDetalle.end_fecha_act = ManejoNulos.ManageNullDate(dr["end_fecha_act"]);
                                envioDetalle.end_estado = ManejoNulos.ManageNullStr(dr["end_estado"]);
                                envioDetalle.fk_envio = ManejoNulos.ManageNullInteger(dr["fk_envio"]);
                  
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
            return (cumEnvioDet: envioDetalle, error: error);
        }
        public (int idInsertado, claseError error) CumEnvioDetalleInsertarJson(CumEnvioDetalleEntidad envioDet)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO cumplimiento.cum_envio_det(
	end_dni, end_correo_corp, end_correo_pers, end_fecha_reg, end_fecha_act, end_estado, fk_envio)
	VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)
                                returning end_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(envioDet.end_dni));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(envioDet.end_correo_corp));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(envioDet.end_correo_pers));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(envioDet.end_fecha_reg));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(envioDet.end_fecha_act));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(envioDet.end_estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(envioDet.fk_envio));

                    idInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idInsertado: idInsertado, error: error);
        }
        public (bool editado, claseError error) CumEnvioDetalleEditarJson(CumEnvioDetalleEntidad envioDet)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE cumplimiento.cum_envio_det
	                            SET end_correo_pers=@p0, 
                                end_fecha_act=@p1, end_estado=@p2
	                            WHERE end_id=@p3;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(envioDet.end_correo_pers));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullDate(envioDet.end_fecha_act));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(envioDet.end_estado));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(envioDet.end_id));


                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (editado: response, error: error);
        }
    }
}