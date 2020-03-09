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
    public class PosRespuestaOLAModel
    {
        string _conexion;
        public PosRespuestaOLAModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (bool response, claseError error) PosRespuestaOLAInsertarJson(PosRespuestaOLAEntidad respuesta)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_pos_respuesta_ol(
	                            rol_respuesta, fk_pos_pregunta_ol,rol_calificacion,rol_tipo,rol_orden,rol_estado)
	                            VALUES (@p0, @p1,@p2,@p3,@p4,@p5);";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(respuesta.rol_respuesta));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(respuesta.fk_pos_pregunta_ol));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(respuesta.rol_calificacion));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(respuesta.rol_tipo));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(respuesta.rol_orden));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(respuesta.rol_estado));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (response, error);
        }
    }
}