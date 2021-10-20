using Npgsql;
using SistemaReclutamiento.Entidades.BoletasGDT;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.BoletasGDT
{
    public class BolConfiguracionModel
    {
        string _conexion;
        public BolConfiguracionModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (BolConfiguracionEntidad configuracion, claseError error) BoolConfiguracionIdObtenerJson(int config_id)
        {
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT config_id, config_descripcion, config_estado, 
                                config_valor, config_tipo
	                            FROM intranet.bol_configuracion
                                where config_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", config_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                configuracion.config_id = ManejoNulos.ManageNullInteger(dr["config_id"]);
                                configuracion.config_descripcion = ManejoNulos.ManageNullStr(dr["config_descripcion"]);
                                configuracion.config_estado= ManejoNulos.ManageNullInteger(dr["config_estado"]);
                                configuracion.config_valor = ManejoNulos.ManageNullStr(dr["config_valor"]);
                                configuracion.config_tipo = ManejoNulos.ManageNullStr(dr["config_tipo"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (configuracion, error);
        }
        public (BolConfiguracionEntidad configuracion, claseError error) BoolConfiguracionObtenerxTipoJson(string config_tipo)
        {
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT config_id, config_descripcion, config_estado, 
                                config_valor, config_tipo
	                            FROM intranet.bol_configuracion
                                where config_tipo=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", config_tipo);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                configuracion.config_id = ManejoNulos.ManageNullInteger(dr["config_id"]);
                                configuracion.config_descripcion = ManejoNulos.ManageNullStr(dr["config_descripcion"]);
                                configuracion.config_estado = ManejoNulos.ManageNullInteger(dr["config_estado"]);
                                configuracion.config_valor = ManejoNulos.ManageNullStr(dr["config_valor"]);
                                configuracion.config_tipo = ManejoNulos.ManageNullStr(dr["config_tipo"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (configuracion, error);
        }
        public (int idInsertado, claseError error) BoolConfiguracionInsertarJson(BolConfiguracionEntidad configuracion)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO intranet.bol_configuracion(
	                                config_descripcion, config_estado, config_valor, config_tipo)
	                                VALUES (@p0,@p1,@p2,@p3)
                                returning config_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(configuracion.config_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(configuracion.config_estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(configuracion.config_valor));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(configuracion.config_tipo));

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
            return (idInsertado: idInsertado, error: error);
        }
        public (bool editado, claseError error) BoolConfiguracionEditarJson(BolConfiguracionEntidad configuracion)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE intranet.bol_configuracion
	                                SET  config_descripcion=@p0, config_estado=@p1, config_valor=@p2, config_tipo=@p3
	                                WHERE config_id=@p4;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(configuracion.config_descripcion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullInteger(configuracion.config_estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(configuracion.config_valor));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(configuracion.config_tipo));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(configuracion.config_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (editado: response, error: error);
        }
    }
}