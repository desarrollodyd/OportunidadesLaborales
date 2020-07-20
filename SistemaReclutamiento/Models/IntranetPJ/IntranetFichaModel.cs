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
            claseError error = new claseError();
            string consulta = @"SELECT ce.env_id, ce.env_nombre, ce.env_tipo, ce.env_fecha_reg, ce.env_fecha_act, ce.env_estado, ce.fk_cuestionario, ce.fk_usuario
	                            FROM cumplimiento.cum_envio ce
                                join cumplimiento.cum_usuario cu on cu.fk_usuario = ce.fk_usuario
                                 where cu.cus_tipo=@p0 and ce.env_fecha_reg>=@p1 and ce.env_fecha_reg <=@p2 ;
	                                ;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", tipo);
                    query.Parameters.AddWithValue("@p1", inicio);
                    query.Parameters.AddWithValue("@p2", fin);
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

    }
}