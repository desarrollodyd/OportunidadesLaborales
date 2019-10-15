using Npgsql;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class PermisoModel
    {
        string _conexion = string.Empty;
        public PermisoModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<PermisoEntidad> PermisoListarUsuarioJson(int fk_usuario)
        {
            List<PermisoEntidad> lista = new List<PermisoEntidad>();
            string consulta = @"
                    SELECT pem_estado, fk_usuario, fk_boton, fk_submenu, pem_fecha_reg, pem_usu_reg
	                    FROM seguridad.seg_permiso where fk_usuario=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_usuario);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var Permiso = new PermisoEntidad
                            {
                                pem_estado = ManejoNulos.ManageNullStr(dr["pem_estado"]),
                                fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]),
                                fk_boton = ManejoNulos.ManageNullInteger(dr["fk_boton"]),
                                fk_submenu = ManejoNulos.ManageNullInteger(dr["fk_submenu"]),
                                pem_fecha_reg = ManejoNulos.ManageNullDate(dr["pem_fecha_reg"]),
                                pem_usu_reg = ManejoNulos.ManageNullInteger(dr["pem_usu_reg"]),
                            };
                            lista.Add(Permiso);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return lista;
        }
        public bool PermisoQuitar(int fk_submenu, int fk_usuario)
        {
            bool response = false;
            string consulta = @"DELETE FROM seguridad.seg_permiso
	                            WHERE fk_usuario=@p0 and fk_submenu=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_usuario);
                    query.Parameters.AddWithValue("@p1", fk_submenu);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return response;
        }
        public bool PermisoInsertar(int fk_submenu, int fk_usuario)
        {
            bool response = false;
            string consulta = @"INSERT INTO seguridad.seg_permiso(
	                            pem_estado, fk_usuario, fk_boton, fk_submenu)
	                            VALUES (@p0, @p1, @p2, @p3);";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", 'A');
                    query.Parameters.AddWithValue("@p1", fk_usuario);
                    query.Parameters.AddWithValue("@p2", 1);
                    query.Parameters.AddWithValue("@p3", fk_submenu);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return response;
        }
        public bool PermisoTodoEliminar(int fk_usuario)
        {
            bool response = false;
            string consulta = @"DELETE FROM seguridad.seg_permiso
	                            WHERE fk_usuario=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_usuario);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return response;
        }
    }
}