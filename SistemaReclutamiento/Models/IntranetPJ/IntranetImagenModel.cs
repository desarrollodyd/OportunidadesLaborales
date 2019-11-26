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
    public class IntranetImagenModel
    {
        string _conexion;
        public IntranetImagenModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetImagenEntidad> intranetImagenLista, claseError error) IntranetImagenListarJson()
        {
            List<IntranetImagenEntidad> lista = new List<IntranetImagenEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT img_id, img_descripcion, img_nombre, 
                                img_extension, img_ubicacion, img_estado, fk_elemento, fk_seccion_elemento
	                                FROM intranet.int_imagen;";
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
                                var Imagen = new IntranetImagenEntidad
                                {

                                    img_id = ManejoNulos.ManageNullInteger(dr["img_id"]),
                                    img_descripcion = ManejoNulos.ManageNullStr(dr["img_descripcion"]),
                                    img_nombre = ManejoNulos.ManageNullStr(dr["img_nombre"]),
                                    img_extension = ManejoNulos.ManageNullStr(dr["img_extension"]),
                                    img_ubicacion = ManejoNulos.ManageNullStr(dr["img_ubicacion"]),
                                    img_estado = ManejoNulos.ManageNullStr(dr["img_estado"]),
                                    fk_elemento = ManejoNulos.ManageNullInteger(dr["fk_elemento"]),
                                    fk_seccion_elemento = ManejoNulos.ManageNullInteger(dr["fk_seccion_elemento"]),

                                };

                                lista.Add(Imagen);
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
            return (intranetImagenLista: lista, error: error);
        }
        public (IntranetImagenEntidad intranetImagen, claseError error) IntranetImagenIdObtenerJson(int img_id)
        {
            IntranetImagenEntidad intranetImagen = new IntranetImagenEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT img_id, img_descripcion, img_nombre, 
                                img_extension, img_ubicacion, img_estado, fk_elemento, fk_seccion_elemento
	                                FROM intranet.int_imagen where img_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", img_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                intranetImagen.img_id = ManejoNulos.ManageNullInteger(dr["img_id"]);
                                intranetImagen.img_descripcion = ManejoNulos.ManageNullStr(dr["img_descripcion"]);
                                intranetImagen.img_nombre = ManejoNulos.ManageNullStr(dr["img_nombre"]);
                                intranetImagen.img_extension = ManejoNulos.ManageNullStr(dr["img_extension"]);
                                intranetImagen.img_ubicacion = ManejoNulos.ManageNullStr(dr["img_ubicacion"]);
                                intranetImagen.img_estado = ManejoNulos.ManageNullStr(dr["img_estado"]);
                                intranetImagen.fk_elemento = ManejoNulos.ManageNullInteger(dr["fk_elemento"]);
                                intranetImagen.fk_seccion_elemento = ManejoNulos.ManageNullInteger(dr["fk_seccion_elemento"]);
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
            return (intranetImagen: intranetImagen, error: error);
        }
        public (int idIntranetImagenInsertado, claseError error) IntranetImagenInsertarJson(IntranetImagenEntidad intranetImagen)
        {
            //bool response = false;
            int idIntranetImagenInsertado = 0;
            string consulta = @"
            INSERT INTO intranet.int_imagen(
	            img_descripcion, img_nombre, img_extension, img_ubicacion, img_estado,fk_elemento,fk_seccion_elemento)
	            VALUES ( @p0, @p1, @p2,@p3, @p5,@p6,@p7);
                returning img_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetImagen.img_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetImagen.img_nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetImagen.img_extension));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetImagen.img_ubicacion));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(intranetImagen.img_estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(intranetImagen.fk_elemento));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(intranetImagen.fk_seccion_elemento));
                    idIntranetImagenInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idIntranetImagenInsertado: idIntranetImagenInsertado, error: error);
        }

        public (bool intranetImagenEditado, claseError error) IntranetImagenEditarJson(IntranetImagenEntidad intranetImagen)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_imagen
	                    SET  img_descripcion=@p0, img_nombre=@p1, img_extension=@p2, img_ubicacion=@p3,  img_estado=@p5,fk_elemento=@p7,@fk_seccion_elemento=@p8
	                    WHERE img_id=@p6;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetImagen.img_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetImagen.img_nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetImagen.img_extension));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetImagen.img_ubicacion));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(intranetImagen.img_estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(intranetImagen.img_id));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(intranetImagen.fk_elemento));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(intranetImagen.fk_seccion_elemento));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (intranetImagenEditado: response, error: error);
        }
        public (bool intranetImagenEliminado, claseError error) IntranetImagenEliminarJson(int img_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_imagen
	                            WHERE img_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(img_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }

            return (intranetImagenEliminado: response, error: error);
        }
    }
}