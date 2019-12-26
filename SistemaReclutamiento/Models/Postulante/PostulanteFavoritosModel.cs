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

        public (bool idIntranetPostulanteFavoritosEliminado, claseError error) IntranetPostulanteFavoritosEliminarJson(int fk_postulante , int fk_oferta_laboral)
        {
            bool response = false;
            string consulta = @"DELETE FROM gestion_talento.gdt_postulante_favoritos
	                                        WHERE fk_postulante=@p0 and fk_oferta_laboral=@p1;
                                ";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(fk_postulante));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(fk_oferta_laboral));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idIntranetPostulanteFavoritosEliminado: response, error: error);
        }
        public (List<PostulanteFavoritosEntidad> lista, claseError error) IntranetPostulanteFavoritosListarxPostulanteJson(int fk_postulante)
        {
            List<PostulanteFavoritosEntidad> lista = new List<PostulanteFavoritosEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT fk_oferta_laboral
	                            FROM gestion_talento.gdt_postulante_favoritos
	                            where fk_postulante=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_postulante);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var favorito = new PostulanteFavoritosEntidad
                                {
                                    fk_oferta_laboral = ManejoNulos.ManageNullInteger(dr["fk_oferta_laboral"]),
                                };

                                lista.Add(favorito);
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
            return (lista: lista, error: error);
        }

    }
}