using Npgsql;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class DetPreguntaOLAModel
    {
        string _conexion;
        public DetPreguntaOLAModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<DetPreguntaOLAEntidad> DetPreguntaListarporPreguntaJson(int fk_oferta_laboral)
        {
            List<DetPreguntaOLAEntidad> lista = new List<DetPreguntaOLAEntidad>();
            string consulta = @"SELECT dop_id, dop_pregunta, dop_tipo, dop_resp1, dop_resp2, dop_porcentaje, fk_oferta_laboral
	FROM gestion_talento.gdt_ola_det_pregunta_of where fk_oferta_laboral=@p0
                                order by dop_id asc;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_oferta_laboral);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var detalle = new DetPreguntaOLAEntidad
                                {
                                    dop_id = ManejoNulos.ManageNullInteger(dr["dop_id"]),
                                    dop_pregunta = ManejoNulos.ManageNullStr(dr["dop_pregunta"]),
                                    dop_tipo = ManejoNulos.ManageNullStr(dr["dop_tipo"]),
                                    dop_resp1 = ManejoNulos.ManageNullStr(dr["dop_resp1"]),
                                    dop_resp2 = ManejoNulos.ManageNullStr(dr["dop_resp2"]),
                                    dop_porcentaje = ManejoNulos.ManageNullStr(dr["dop_porcentaje"]),
                                    fk_oferta_laboral = ManejoNulos.ManageNullInteger(dr["fk_oferta_laboral"])
                                };

                                lista.Add(detalle);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return lista;
        }
    }
}