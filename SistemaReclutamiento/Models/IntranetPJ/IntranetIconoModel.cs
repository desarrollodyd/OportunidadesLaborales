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
    public class IntranetIconoModel
    {
        string _conexion;
        public IntranetIconoModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetIconoEntidad> intranetIconoLista, claseError error) IntranetIconoListarJson(int fk_layout)
        {
            List<IntranetIconoEntidad> lista = new List<IntranetIconoEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT icon_id, icon_descripcion, icon_ubicacion, fk_layout, icon_estado
	FROM intranet.int_icono where fk_layout=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_layout);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var Icono = new IntranetIconoEntidad
                                {

                                    icon_id = ManejoNulos.ManageNullInteger(dr["icon_id"]),
                                    icon_descripcion = ManejoNulos.ManageNullStr(dr["icon_descripcion"]),
                                    icon_ubicacion = ManejoNulos.ManageNullStr(dr["icon_ubicacion"]),
                                    fk_layout = ManejoNulos.ManageNullInteger(dr["fk_layout"]),
                                    icon_estado = ManejoNulos.ManageNullStr(dr["icon_estado"]),
                                };

                                lista.Add(Icono);
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
            return (intranetIconoLista: lista, error: error);
        }
        public (IntranetIconoEntidad intranetIcono, claseError error) IntranetIconoIdObtenerJson(int icon_id)
        {
            IntranetIconoEntidad intranetIcono = new IntranetIconoEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT icon_id, icon_descripcion, icon_ubicacion, fk_layout, icon_estado
	                                FROM intranet.int_icono where icon_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", icon_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                intranetIcono.icon_id = ManejoNulos.ManageNullInteger(dr["icon_id"]);
                                intranetIcono.icon_descripcion = ManejoNulos.ManageNullStr(dr["icon_descripcion"]);
                                intranetIcono.icon_ubicacion = ManejoNulos.ManageNullStr(dr["icon_ubicacion"]);
                                intranetIcono.fk_layout = ManejoNulos.ManageNullInteger(dr["fk_layout"]);
                                intranetIcono.icon_estado = ManejoNulos.ManageNullStr(dr["icon_estado"]);
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
            return (intranetIcono: intranetIcono, error: error);
        }
        public (int idIntranetIconoInsertado, claseError error) IntranetIconoInsertarJson(IntranetIconoEntidad intranetIcono)
        {
            //bool response = false;
            int idIntranetIconoInsertado = 0;
            string consulta = @"
            INSERT INTO intranet.int_icono(
	                icon_descripcion, icon_ubicacion, fk_layout, icon_estado)
	                VALUES ( @p0, @p1, @p2, @p3)
                returning icon_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetIcono.icon_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetIcono.icon_ubicacion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(intranetIcono.fk_layout));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetIcono.icon_estado));
                    idIntranetIconoInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idIntranetIconoInsertado: idIntranetIconoInsertado, error: error);
        }

        public (bool intranetIconoEditado, claseError error) IntranetIconoEditarJson(IntranetIconoEntidad intranetIcono)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_icono
	                    SET  icon_descripcion=@p0, icon_ubicacion=@p1, fk_layout=@p2, icon_estado=@p3
	                    WHERE icon_id=@p4;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetIcono.icon_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(intranetIcono.icon_ubicacion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(intranetIcono.fk_layout));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(intranetIcono.icon_estado));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(intranetIcono.icon_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (intranetIconoEditado: response, error: error);
        }
        public (bool intranetIconoEliminado, claseError error) IntranetIconoEliminarJson(int icon_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_icono
	                            WHERE icon_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(icon_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }

            return (intranetIconoEliminado: response, error: error);
        }
    }
}