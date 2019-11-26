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
    public class IntranetSaludoCumpleaniosModel
    {
        string _conexion;
        public IntranetSaludoCumpleaniosModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetSaludoCumpleanioEntidad> intranetSaludoCumpleanioLista, claseError error) IntranetSaludoCumpleanioListarJson()
        {
            List<IntranetSaludoCumpleanioEntidad> lista = new List<IntranetSaludoCumpleanioEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT sld_id, sld_cuerpo, sld_estado, sld_fecha_envio, fk_persona
	                            FROM intranet.int_saludos_cumpleanio
                                    order by sld_fecha_envio;";
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
                                var SaludoCumpleanio = new IntranetSaludoCumpleanioEntidad
                                {

                                    sld_id = ManejoNulos.ManageNullInteger(dr["sld_id"]),
                                    sld_cuerpo = ManejoNulos.ManageNullStr(dr["sld_cuerpo"]),
                                    sld_estado = ManejoNulos.ManageNullStr(dr["sld_estado"]),
                                    sld_fecha_envio = ManejoNulos.ManageNullDate(dr["sld_fecha_envio"]),
                                    fk_persona = ManejoNulos.ManageNullInteger(dr["fk_persona"]),
                                };

                                lista.Add(SaludoCumpleanio);
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
            return (intranetSaludoCumpleanioLista: lista, error: error);
        }
        public (IntranetSaludoCumpleanioEntidad intranetSaludoCumpleanio, claseError error) IntranetSaludoCumpleanioIdObtenerJson(int sld_id)
        {
            IntranetSaludoCumpleanioEntidad intranetSaludoCumpleanio = new IntranetSaludoCumpleanioEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT sld_id, sld_cuerpo, sld_estado, sld_fecha_envio, fk_persona
	                            FROM intranet.int_saludos_cumpleanio where sld_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", sld_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                intranetSaludoCumpleanio.sld_id = ManejoNulos.ManageNullInteger(dr["sld_id"]);
                                intranetSaludoCumpleanio.sld_cuerpo = ManejoNulos.ManageNullStr(dr["sld_cuerpo"]);
                                intranetSaludoCumpleanio.sld_estado = ManejoNulos.ManageNullStr(dr["sld_estado"]);
                                intranetSaludoCumpleanio.sld_fecha_envio = ManejoNulos.ManageNullDate(dr["sld_fecha_envio"]);
                                intranetSaludoCumpleanio.fk_persona = ManejoNulos.ManageNullInteger(dr["fk_persona"]);
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
            return (intranetSaludoCumpleanio: intranetSaludoCumpleanio, error: error);
        }
        public (int idIntranetSaludoCumpleanioInsertado, claseError error) IntranetSaludoCumpleanioInsertarJson(IntranetSaludoCumpleanioEntidad intranetSaludoCumpleanio)
        {
            //bool response = false;
            int idIntranetSaludoCumpleanioInsertado = 0;
            string consulta = @"
                            INSERT INTO intranet.int_saludos_cumpleanio(
	                        sld_cuerpo, sld_estado, sld_fecha_envio, fk_persona)
	                        VALUES ( @p0, @p1, @p2, @p3);
                            returning sld_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetSaludoCumpleanio.sld_cuerpo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetSaludoCumpleanio.sld_estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(intranetSaludoCumpleanio.sld_fecha_envio));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(intranetSaludoCumpleanio.fk_persona));
                    idIntranetSaludoCumpleanioInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idIntranetSaludoCumpleanioInsertado: idIntranetSaludoCumpleanioInsertado, error: error);
        }
    }
}