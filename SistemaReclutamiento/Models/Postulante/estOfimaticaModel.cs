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
    public class EstOfimaticaModel
    {
        string _conexion;
        public EstOfimaticaModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<EstOfimaticaEntidad> EstOfimaticaListarJson()
        {
            List<EstOfimaticaEntidad> lista = new List<EstOfimaticaEntidad>();
            string consulta = @"SELECT 
                                eof_id, 
                                eof_nombre, 
                                eof_fecha_reg, 
                                eof_fecha_act, 
                                eof_estado
	                            FROM gestion_talento.gdt_est_ofimatica where eof_estado='A';
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
                                var estOfimatica = new EstOfimaticaEntidad
                                {

                                    eof_id = ManejoNulos.ManageNullInteger(dr["eof_id"]),
                                    eof_nombre = ManejoNulos.ManageNullStr(dr["eof_nombre"]),
                                    eof_estado = ManejoNulos.ManageNullStr(dr["eof_estado"]),
                                    eof_fecha_reg = ManejoNulos.ManageNullDate(dr["eof_fecha_reg"]),
                                    eof_fecha_act = ManejoNulos.ManageNullDate(dr["eof_fecha_act"])                            
                                };

                                lista.Add(estOfimatica);
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
        public EstOfimaticaEntidad EstOfimaticaIdObtenerJson(int her_id)
        {
            EstOfimaticaEntidad estOfimatica = new EstOfimaticaEntidad();
            string consulta = @"SELECT 
                                eof_id, 
                                eof_nombre, 
                                eof_fecha_reg, 
                                eof_fecha_act, 
                                eof_estado
	                            FROM gestion_talento.gdt_est_ofimatica                                 
                                where eof_id=@p0;";
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
                                estOfimatica.eof_id = ManejoNulos.ManageNullInteger(dr["eof_id"]);
                                estOfimatica.eof_nombre = ManejoNulos.ManageNullStr(dr["eof_nombre"]);
                                estOfimatica.eof_estado = ManejoNulos.ManageNullStr(dr["eof_estado"]);
                                estOfimatica.eof_fecha_reg = ManejoNulos.ManageNullDate(dr["eof_fecha_reg"]);
                                estOfimatica.eof_fecha_act = ManejoNulos.ManageNullDate(dr["eof_fecha_act"]);                          

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return estOfimatica;
        }
    }
}