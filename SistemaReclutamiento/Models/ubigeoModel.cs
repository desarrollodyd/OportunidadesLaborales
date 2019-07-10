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
        public List<ubigeoEntidad> UbigeoListarPaises() {
            List<ubigeoEntidad> lista = new List<ubigeoEntidad>();
            string consulta = @"SELECT 
                                ubi_id, 
                                ubi_pais_id, 
                                ubi_departamento_id, 
                                ubi_provincia_id, 
                                ubi_distrito_id, 
                                ubi_nombre, 
                                ubi_estado
	                                FROM marketing.cpj_ubigeo;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
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
    }
}