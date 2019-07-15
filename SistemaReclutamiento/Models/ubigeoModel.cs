using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Npgsql;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Models
{
    public class ubigeoModel
    {
        string _conexion;
        public ubigeoModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ubigeoEntidad> UbigeoListarPaisesJson() {
            List<ubigeoEntidad> lista = new List<ubigeoEntidad>();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                                FROM marketing.cpj_ubigeo
                                where ubi_departamento_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0" , "0");
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ubigeo = new ubigeoEntidad
                            {
                                ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]),
                                ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]),
                                ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]),
                                ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]),
                                ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]),
                                ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]),
                                ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"])
                            };
                            lista.Add(ubigeo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return lista;
        }
        public List<ubigeoEntidad> UbigeoListarDepartamentosporPaisJson(string id_pais) {
            List<ubigeoEntidad> lista = new List<ubigeoEntidad>();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                            FROM marketing.cpj_ubigeo 
                                where ubi_pais_id=@p0
                                and ubi_provincia_id='0'
                                and ubi_distrito_id='0'
                                and ubi_departamento_id<>'0';";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id_pais);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ubigeo = new ubigeoEntidad
                            {
                                ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]),
                                ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]),
                                ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]),
                                ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]),
                                ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]),
                                ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]),
                                ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"])
                            };
                            lista.Add(ubigeo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return lista;
        }
        public List<ubigeoEntidad> UbigeoListarProvinciasporDepartamentoJson(string id_pais, string id_departamento)
        {
            List<ubigeoEntidad> lista = new List<ubigeoEntidad>();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                            FROM marketing.cpj_ubigeo 
                                where ubi_pais_id=@p0
                                and ubi_provincia_id<>'0'
                                and ubi_distrito_id='0'
                                and ubi_departamento_id=@p1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", id_pais);
                    query.Parameters.AddWithValue("@p1", id_departamento);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ubigeo = new ubigeoEntidad
                            {
                                ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]),
                                ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]),
                                ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]),
                                ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]),
                                ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]),
                                ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]),
                                ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"])
                            };
                            lista.Add(ubigeo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return lista;
        }
        public List<ubigeoEntidad> UbigeoListarDistritosporProvinciaJson(string ubi_pais_id, string ubi_departamento_id,string ubi_provincia_id)
        {
            List<ubigeoEntidad> lista = new List<ubigeoEntidad>();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                            FROM marketing.cpj_ubigeo 
                                where ubi_pais_id=@p0
                                and ubi_provincia_id=@p1
                                and ubi_distrito_id<>'0'
                                and ubi_departamento_id=@p2;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ubi_pais_id);
                    query.Parameters.AddWithValue("@p1", ubi_provincia_id);
                    query.Parameters.AddWithValue("@p2", ubi_departamento_id);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var ubigeo = new ubigeoEntidad
                            {
                                ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]),
                                ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]),
                                ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]),
                                ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]),
                                ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]),
                                ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]),
                                ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"])
                            };
                            lista.Add(ubigeo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return lista;
        }
        public ubigeoEntidad UbigeoIdObtenerJson(string ubi_pais_id, string ubi_departamento_id, string ubi_provincia_id, string ubi_distrito_id)
        {
            ubigeoEntidad ubigeo = new ubigeoEntidad();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                            FROM marketing.cpj_ubigeo 
                                where ubi_pais_id=@p0
                                and ubi_provincia_id=@p1
                                and ubi_distrito_id=@p2
                                and ubi_departamento_id=@p3;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ubi_pais_id);
                    query.Parameters.AddWithValue("@p1", ubi_provincia_id);
                    query.Parameters.AddWithValue("@p2", ubi_distrito_id);
                    query.Parameters.AddWithValue("@p3", ubi_departamento_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                                
                            {
                                ubigeo.ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]);
                                ubigeo.ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]);
                                ubigeo.ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]);
                                ubigeo.ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]);
                                ubigeo.ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]);
                                ubigeo.ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]);
                                ubigeo.ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ubigeo;
        }

        public ubigeoEntidad UbigeoObtenerDatosporIdJson(int ubi_id)
        {
            ubigeoEntidad ubigeo = new ubigeoEntidad();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                            FROM marketing.cpj_ubigeo 
                                where ubi_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ubi_id);                 
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())

                            {
                                ubigeo.ubi_id = ManejoNulos.ManageNullInteger(dr["ubi_id"]);
                                ubigeo.ubi_departamento_id = ManejoNulos.ManageNullStr(dr["ubi_departamento_id"]);
                                ubigeo.ubi_distrito_id = ManejoNulos.ManageNullStr(dr["ubi_distrito_id"]);
                                ubigeo.ubi_estado = ManejoNulos.ManageNullStr(dr["ubi_estado"]);
                                ubigeo.ubi_pais_id = ManejoNulos.ManageNullStr(dr["ubi_pais_id"]);
                                ubigeo.ubi_provincia_id = ManejoNulos.ManageNullStr(dr["ubi_provincia_id"]);
                                ubigeo.ubi_nombre = ManejoNulos.ManageNullStr(dr["ubi_nombre"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ubigeo;
        }
    }
}