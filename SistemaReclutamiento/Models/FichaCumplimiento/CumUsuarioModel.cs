using Npgsql;
using SistemaReclutamiento.Entidades.FichaCumplimiento;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class CumUsuarioModel
    {
        string _conexion;
        public CumUsuarioModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<CumUsuarioEntidad> lista, claseError error) CumUsuarioListarJson()
        {
            List<CumUsuarioEntidad> lista = new List<CumUsuarioEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT cus_id,
                                        cus_dni, 
                                        cus_tipo, 
                                        cus_correo, 
                                        cus_clave, 
                                        cus_firma, 
                                        cus_fecha_reg, 
                                        cus_fecha_act, 
                                        cus_estado, 
                                        fk_usuario
	                                        FROM cumplimiento.cum_usuario;";
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
                                var usuario = new CumUsuarioEntidad
                                {

                                    cus_id = ManejoNulos.ManageNullInteger(dr["cus_id"]),
                                    cus_dni = ManejoNulos.ManageNullStr(dr["cus_dni"]),
                                    cus_tipo = ManejoNulos.ManageNullStr(dr["cus_tipo"]),
                                    cus_correo = ManejoNulos.ManageNullStr(dr["cus_correo"]),
                                    cus_clave = ManejoNulos.ManageNullStr(dr["cus_clave"]),
                                    cus_firma = ManejoNulos.ManageNullStr(dr["cus_firma"]),
                                    cus_fecha_act = ManejoNulos.ManageNullDate(dr["cus_fecha_act"]),
                                    cus_fecha_reg = ManejoNulos.ManageNullDate(dr["cus_fecha_reg"]),
                                    cus_estado = ManejoNulos.ManageNullStr(dr["cus_estado"]),
                                    fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]),

                                };

                                lista.Add(usuario);
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
        public (CumUsuarioEntidad cumUsuario, claseError error) CumUsuarioIdObtenerJson(int cus_id)
        {
            CumUsuarioEntidad usuario = new CumUsuarioEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT cus_id,
                                        cus_dni, 
                                        cus_tipo, 
                                        cus_correo, 
                                        cus_clave, 
                                        cus_firma, 
                                        cus_fecha_reg, 
                                        cus_fecha_act, 
                                        cus_estado, 
                                        fk_usuario
	                                    FROM cumplimiento.cum_usuario
                                        where cus_id=@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", cus_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                usuario.cus_id = ManejoNulos.ManageNullInteger(dr["cus_id"]);
                                usuario.cus_dni = ManejoNulos.ManageNullStr(dr["cus_dni"]);
                                usuario.cus_tipo = ManejoNulos.ManageNullStr(dr["cus_tipo"]);
                                usuario.cus_correo = ManejoNulos.ManageNullStr(dr["cus_correo"]);
                                usuario.cus_clave = ManejoNulos.ManageNullStr(dr["cus_clave"]);
                                usuario.cus_firma = ManejoNulos.ManageNullStr(dr["cus_firma"]);
                                usuario.cus_fecha_reg = ManejoNulos.ManageNullDate(dr["cus_fecha_reg"]);
                                usuario.cus_fecha_act = ManejoNulos.ManageNullDate(dr["cus_fecha_act"]);
                                usuario.cus_estado = ManejoNulos.ManageNullStr(dr["cus_estado"]);
                                usuario.fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]);
                       
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
            return (cumUsuario: usuario, error: error);
        }
        public (CumUsuarioEntidad cumUsuario, claseError error) CumUsuarioFkUsuarioObtenerJson(int fk_usuario)
        {
            CumUsuarioEntidad usuario = new CumUsuarioEntidad();
            claseError error = new claseError();
            string consulta = @"SELECT cus_id,
                                        cus_dni, 
                                        cus_tipo, 
                                        cus_correo, 
                                        cus_clave, 
                                        cus_firma, 
                                        cus_fecha_reg, 
                                        cus_fecha_act, 
                                        cus_estado, 
                                        fk_usuario
	                                    FROM cumplimiento.cum_usuario
                                        where fk_usuario=@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_usuario);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                usuario.cus_id = ManejoNulos.ManageNullInteger(dr["cus_id"]);
                                usuario.cus_dni = ManejoNulos.ManageNullStr(dr["cus_dni"]);
                                usuario.cus_tipo = ManejoNulos.ManageNullStr(dr["cus_tipo"]);
                                usuario.cus_correo = ManejoNulos.ManageNullStr(dr["cus_correo"]);
                                usuario.cus_clave = ManejoNulos.ManageNullStr(dr["cus_clave"]);
                                usuario.cus_firma = ManejoNulos.ManageNullStr(dr["cus_firma"]);
                                usuario.cus_fecha_reg = ManejoNulos.ManageNullDate(dr["cus_fecha_reg"]);
                                usuario.cus_fecha_act = ManejoNulos.ManageNullDate(dr["cus_fecha_act"]);
                                usuario.cus_estado = ManejoNulos.ManageNullStr(dr["cus_estado"]);
                                usuario.fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]);

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
            return (cumUsuario: usuario, error: error);
        }
        public (int idInsertado, claseError error) CumUsuarioInsertarJson(CumUsuarioEntidad usuario)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO cumplimiento.cum_usuario(
	                            cus_dni, cus_tipo, cus_correo, cus_firma, cus_fecha_reg, cus_estado, fk_usuario, cus_fecha_act)
	                            VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6,@p7)
                                returning cus_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(usuario.cus_dni));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(usuario.cus_tipo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(usuario.cus_correo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(usuario.cus_firma));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(usuario.cus_fecha_reg));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(usuario.cus_estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(usuario.fk_usuario));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(usuario.cus_fecha_act));
                    idInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idInsertado: idInsertado, error: error);
        }

        public (int idInsertado, claseError error) CumUsuarioInsertarsinfkuserJson(CumUsuarioEntidad usuario)
        {
            //bool response = false;
            int idInsertado = 0;
            string consulta = @"INSERT INTO cumplimiento.cum_usuario(
	                            cus_dni, cus_tipo, cus_correo, cus_firma, cus_fecha_reg, cus_estado,cus_clave, cus_fecha_act)
	                            VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6,@p7)
                                returning cus_id;";
            claseError error = new claseError();
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(usuario.cus_dni));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(usuario.cus_tipo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(usuario.cus_correo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(usuario.cus_firma));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(usuario.cus_fecha_reg));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(usuario.cus_estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(usuario.cus_clave));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(usuario.cus_fecha_act));
                    idInsertado = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (idInsertado: idInsertado, error: error);
        }
        public (bool editado, claseError error) CumUsuarioEditarJson(CumUsuarioEntidad usuario)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE cumplimiento.cum_usuario
	                            SET cus_dni=@p0, cus_tipo=@p1, cus_correo=@p2, 
                                cus_clave=@p3,cus_fecha_act=@p4, cus_estado=@p5, fk_usuario=@p6
	                            WHERE cus_id=@p7;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(usuario.cus_dni));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(usuario.cus_tipo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(usuario.cus_correo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(usuario.cus_clave));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(usuario.cus_fecha_act));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(usuario.cus_estado));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(usuario.fk_usuario));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(usuario.cus_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (editado: response, error: error);
        }

        public (bool editado, claseError error) CumUsuarioEditarcorreoJson(CumUsuarioEntidad usuario)
        {
            claseError error = new claseError();
            bool response = false;
            string consulta = @"UPDATE cumplimiento.cum_usuario
	                            SET cus_correo=@p2, 
                                cus_clave=@p3,cus_fecha_act=@p4, cus_estado=@p5
	                            WHERE cus_id=@p7;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                   
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(usuario.cus_correo));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(usuario.cus_clave));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(usuario.cus_fecha_act));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(usuario.cus_estado));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(usuario.cus_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (editado: response, error: error);
        }
    }
}