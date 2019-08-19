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
    public class estIdiomaModel
    {
        string _conexion;
        public estIdiomaModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<estIdiomaEntidad> EstOfimaticaListarJson()
        {
            List<estIdiomaEntidad> lista = new List<estIdiomaEntidad>();
            string consulta = @"SELECT 
                                eid_id, 
                                eid_nombre,
                                eid_fecha_reg, 
                                eid_fecha_act,
                                eid_estado
	                            FROM gestion_talento.gdt_est_idioma
                                where eid_estado='A';
                                ";
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
                                var estIdioma = new estIdiomaEntidad
                                {

                                    eid_id = ManejoNulos.ManageNullInteger(dr["eid_id"]),
                                    eid_nombre = ManejoNulos.ManageNullStr(dr["eid_nombre"]),
                                    eid_estado = ManejoNulos.ManageNullStr(dr["eid_estado"]),
                                    eid_fecha_reg = ManejoNulos.ManageNullDate(dr["eid_fecha_reg"]),
                                    eid_fecha_act = ManejoNulos.ManageNullDate(dr["eid_fecha_act"])
                                };

                                lista.Add(estIdioma);
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
        public estIdiomaEntidad EstOfimaticaIdObtenerJson(int her_id)
        {
            estIdiomaEntidad estIdioma = new estIdiomaEntidad();
            string consulta = @"SELECT 
                                eid_id, 
                                eid_nombre,
                                eid_fecha_reg, 
                                eid_fecha_act,
                                eid_estado
	                            FROM gestion_talento.gdt_est_idioma                                
                                where eid_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", her_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                estIdioma.eid_id = ManejoNulos.ManageNullInteger(dr["eid_id"]);
                                estIdioma.eid_nombre = ManejoNulos.ManageNullStr(dr["eid_nombre"]);
                                estIdioma.eid_estado = ManejoNulos.ManageNullStr(dr["eid_estado"]);
                                estIdioma.eid_fecha_reg = ManejoNulos.ManageNullDate(dr["eid_fecha_reg"]);
                                estIdioma.eid_fecha_act = ManejoNulos.ManageNullDate(dr["eid_fecha_act"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return estIdioma;
        }
    }
}