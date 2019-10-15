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
    public class ModuloModel
    {
        string _conexion;
        public ModuloModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        public List<ModuloEntidad> ModuloListarJson()
        {
            List<ModuloEntidad> lista = new List<ModuloEntidad>();
            string consulta = @"SELECT 
                                mod_id,
                                mod_descripcion, 
                                mod_descripcion_eng,
                                mod_tipo, 
                                mod_orden,
                                mod_icono,
                                mod_estado
	                                FROM seguridad.seg_modulo
                                WHERE mod_tipo='Extranet'
                                and mod_estado='A';";
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
                                var modulo = new ModuloEntidad
                                {

                                    mod_id = ManejoNulos.ManageNullInteger(dr["mod_id"]),
                                    mod_descripcion = ManejoNulos.ManageNullStr(dr["mod_descripcion"]),
                                    mod_descripcion_eng = ManejoNulos.ManageNullStr(dr["mod_descripcion_eng"]),
                                    mod_tipo = ManejoNulos.ManageNullStr(dr["mod_tipo"]),
                                    mod_orden = ManejoNulos.ManageNullInteger(dr["mod_orden"]),
                                    mod_icono = ManejoNulos.ManageNullStr(dr["mod_icono"]),
                                    mod_estado = ManejoNulos.ManageNullStr(dr["mod_estado"]),

                            };

                                lista.Add(modulo);
                            }
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
        public ModuloEntidad ModuloIdObtenerJson(int mod_id)
        {
            ModuloEntidad modulo = new ModuloEntidad();
            string consulta = @"SELECT 
                                mod_id,
                                mod_descripcion, 
                                mod_descripcion_eng,
                                mod_tipo, 
                                mod_orden,
                                mod_icono,
                                mod_estado
	                                FROM seguridad.seg_modulo
                                where mod_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", mod_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                modulo.mod_id = ManejoNulos.ManageNullInteger(dr["mod_id"]);
                                modulo.mod_descripcion = ManejoNulos.ManageNullStr(dr["mod_descripcion"]);
                                modulo.mod_descripcion_eng = ManejoNulos.ManageNullStr(dr["mod_descripcion_eng"]);
                                modulo.mod_tipo = ManejoNulos.ManageNullStr(dr["mod_tipo"]);
                                modulo.mod_orden = ManejoNulos.ManageNullInteger(dr["mod_orden"]);
                                modulo.mod_icono = ManejoNulos.ManageNullStr(dr["mod_icono"]);
                                modulo.mod_estado = ManejoNulos.ManageNullStr(dr["mod_estado"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return modulo;
        }

        public bool ModuloInsertarJson(ModuloEntidad modulo)
        {
            bool response = false;
            string consulta = @"INSERT INTO seguridad.seg_modulo(
	                             mod_descripcion, mod_descripcion_eng, mod_tipo, mod_orden, mod_icono, mod_estado)
	                            VALUES ( @p0, @p1, @p2, @p3, @p4, @p5); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", modulo.mod_descripcion);
                    query.Parameters.AddWithValue("@p1", modulo.mod_descripcion_eng);
                    query.Parameters.AddWithValue("@p2", modulo.mod_tipo);
                    query.Parameters.AddWithValue("@p3", modulo.mod_orden);
                    query.Parameters.AddWithValue("@p4", modulo.mod_icono);
                    query.Parameters.AddWithValue("@p5", modulo.mod_estado);
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

    }
}