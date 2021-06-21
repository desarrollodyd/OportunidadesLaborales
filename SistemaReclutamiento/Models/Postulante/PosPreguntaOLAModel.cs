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
    public class PosPreguntaOLAModel
    {
        string _conexion;
        public PosPreguntaOLAModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (int idPreguntaInsertada,claseError error) PosPreguntaOLAInsertarJson(PosPreguntaOLAEntidad pregunta)
        {
            claseError error = new claseError();
           // bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO gestion_talento.gdt_pos_pregunta_ol(
	                            pol_pregunta, fk_postulacion)
	                            VALUES (@p0, @p1) returning pol_id; ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(pregunta.pol_pregunta));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(pregunta.fk_postulacion));
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
            return (idPreguntaInsertada:idInsertado,error);
        }
    }
}