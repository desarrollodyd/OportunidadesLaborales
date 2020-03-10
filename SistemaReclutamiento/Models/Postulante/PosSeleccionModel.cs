using Npgsql;
using SistemaReclutamiento.Entidades.Postulante;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.Postulante
{
    public class PosSeleccionModel
    {
        string _conexion;
        public PosSeleccionModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (int idInsertado, claseError error) PosSeleccionInsertarJson(PosSeleccionEntidad seleccion)
        {
            int idInsertado = 0;
            string consulta = @"INSERT INTO gestion_talento.gdt_sel_postulante(
	                            spo_nivel1_calif, spo_nivel1_selec,fk_postulacion)
	                            VALUES (@p0,@p1,@p2)
                                returning spo_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(seleccion.spo_nivel1_calif));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManegeNullBool(seleccion.spo_nivel1_selec));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(seleccion.fk_postulacion));
                    idInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idInsertado: idInsertado, error: error);
        }
    }
}