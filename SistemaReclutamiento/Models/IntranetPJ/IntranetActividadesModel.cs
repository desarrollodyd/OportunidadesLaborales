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
    public class IntranetActividadesModel
    {
        string _conexion;
        public IntranetActividadesModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<IntranetActividadesEntidad> intranetActividadesLista, claseError error) IntranetActividadesListarJson(int fk_layout)
        {
            List<IntranetActividadesEntidad> lista = new List<IntranetActividadesEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT act_id, act_descripcion, fk_icono, act_fecha, fk_layout, act_estado
	                                FROM intranet.int_actividades
                                where fk_layout=@p0;";
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
                                var actividades = new IntranetActividadesEntidad
                                {

                                    act_id = ManejoNulos.ManageNullInteger(dr["act_id"]),
                                    act_descripcion = ManejoNulos.ManageNullStr(dr["act_descripcion"]),
                                    fk_icono = ManejoNulos.ManageNullInteger(dr["fk_icono"]),
                                    act_fecha = ManejoNulos.ManageNullDate(dr["act_fecha"]),
                                    fk_layout = ManejoNulos.ManageNullInteger(dr["fk_layout"]),
                                    act_estado = ManejoNulos.ManageNullStr(dr["act_estado"]),
                                };

                                lista.Add(actividades);
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
            return (intranetActividadesLista: lista, error: error);
        }
        public (IntranetActividadesEntidad intranetActividades, claseError error) IntranetActividadesIdObtenerJson(int apl_id)
        {
            IntranetActividadesEntidad intranetActividades = new IntranetActividadesEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT act_id, act_descripcion, fk_icono, act_fecha, fk_layout, act_estado
	                                FROM intranet.int_actividades
                                     where act_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", apl_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                intranetActividades.act_id = ManejoNulos.ManageNullInteger(dr["act_id"]);
                                intranetActividades.act_descripcion = ManejoNulos.ManageNullStr(dr["act_descripcion"]);
                                intranetActividades.fk_icono = ManejoNulos.ManageNullInteger(dr["fk_icono"]);
                                intranetActividades.act_fecha = ManejoNulos.ManageNullDate(dr["act_fecha"]);
                                intranetActividades.fk_layout = ManejoNulos.ManageNullInteger(dr["fk_layout"]);
                                intranetActividades.act_estado = ManejoNulos.ManageNullStr(dr["act_estado"]);
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
            return (intranetActividades: intranetActividades, error: error);
        }
        public (int idIntranetActividadesInsertado, claseError error) IntranetActividadesInsertarJson(IntranetActividadesEntidad intranetActividades)
        {
            //bool response = false;
            int idIntranetActividadesInsertado = 0;
            string consulta = @"
            INSERT INTO intranet.int_actividades(
	                             act_descripcion, fk_icono, act_fecha, fk_layout, act_estado)
	                            VALUES (@p0, @p1, @p2, @p3, @p4)
                                returning act_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetActividades.act_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetActividades.fk_icono));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(intranetActividades.act_fecha));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(intranetActividades.fk_layout));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(intranetActividades.act_estado));
                    idIntranetActividadesInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idIntranetActividadesInsertado: idIntranetActividadesInsertado, error: error);
        }

        public (bool intranetActividadesEditado, claseError error) IntranetActividadesEditarJson(IntranetActividadesEntidad intranetActividades)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.int_actividades
	                            SET act_descripcion=@p0, fk_icono=@p1, act_fecha=@p2, fk_layout=@p3, act_estado=@p4
	                            WHERE act_id=@p5;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(intranetActividades.act_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(intranetActividades.fk_icono));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(intranetActividades.act_fecha));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullInteger(intranetActividades.fk_layout));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(intranetActividades.act_estado));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(intranetActividades.act_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (intranetActividadesEditado: response, error: error);
        }
        public (bool intranetActividadesEliminado, claseError error) IntranetActividadesEliminarJson(int act_id)
        {
            bool response = false;
            string consulta = @"DELETE FROM intranet.int_actividades
	                                WHERE act_id=@p0;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(act_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }

            return (intranetActividadesEliminado: response, error: error);
        }
    }
}