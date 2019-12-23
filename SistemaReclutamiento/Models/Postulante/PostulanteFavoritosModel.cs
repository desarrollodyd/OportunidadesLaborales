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
    public class PostulanteFavoritosModel
    {
        string _conexion;
        public PostulanteFavoritosModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (bool idIntranetPostulanteFavoritosInsertado, claseError error) IntranetPostulanteFavoritosInsertarJson(PostulanteFavoritosEntidad postulanteFavoritos)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_postulante_favoritos(
	                            fk_postulante, fk_oferta_laboral, posfav_estado, posfav_notificar)
	                            VALUES ( @p0, @p1, @p2, @p3)
                                ";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(postulanteFavoritos.fk_postulante));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(postulanteFavoritos.fk_oferta_laboral));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(postulanteFavoritos.posfav_estado));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManegeNullBool(postulanteFavoritos.posfav_notificar));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idIntranetPostulanteFavoritosInsertado: response, error: error);
        }
    }
}