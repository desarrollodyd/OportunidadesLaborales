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
    public class detalleRespuestaPostulanteModel
    {
        string _conexion;
        public detalleRespuestaPostulanteModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public bool DetalleRespuestaPostulanteInsertarJson(detalleRespuestaPostulanteEntidad detalle)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_pos_det_respuestas(
                                    dre_pregunta,
                                    dre_tipo, 
                                    dre_resp1, 
                                    dre_resp2, 
                                    dre_porcentaje, 
                                    dre_respuesta, 
                                    fk_postulante, 
                                    fk_oferta_laboral)
	                                VALUES (@p0, @p1, @p2, @p3, @p4,@p5,@p6,@p7); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(detalle.dre_pregunta));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(detalle.dre_tipo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(detalle.dre_resp1));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(detalle.dre_resp2));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(detalle.dre_porcentaje));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(detalle.dre_respuesta));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(detalle.fk_postulante));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(detalle.fk_oferta_laboral));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return response;
        }
    }
}