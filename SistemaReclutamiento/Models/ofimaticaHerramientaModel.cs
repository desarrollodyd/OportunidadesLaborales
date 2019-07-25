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
    public class ofimaticaHerramientaModel
    {
        string _conexion;
        public ofimaticaHerramientaModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ofimaticaHerramientaEntidad> OfimatiacaHerramientaListarJson()
        {
            List<ofimaticaHerramientaEntidad> lista = new List<ofimaticaHerramientaEntidad>();
            string consulta = @"SELECT 
                                her_id, 
                                her_descripcion, 
                                her_estado, 
                                her_fecha_reg,
                                her_fecha_act
	                                FROM gestion_talento.gdt_per_ofimatica_her;";
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
                                var ofimaticaHerramienta = new ofimaticaHerramientaEntidad
                                {

                                    her_id = ManejoNulos.ManageNullInteger(dr["her_id"]),
                                    her_descripcion = ManejoNulos.ManageNullStr(dr["her_descripcion"]),
                                    her_estado = ManejoNulos.ManageNullStr(dr["her_estado"]),
                                    her_fecha_reg = ManejoNulos.ManageNullDate(dr["her_fecha_reg"]),
                                    her_fecha_act = ManejoNulos.ManageNullDate(dr["her_fecha_act"])                            
                                };

                                lista.Add(ofimaticaHerramienta);
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
        public ofimaticaHerramientaEntidad OfimaticaHerramientaIdObtenerJson(int her_id)
        {
            ofimaticaHerramientaEntidad ofimaticaHerramienta = new ofimaticaHerramientaEntidad();
            string consulta = @"SELECT 
                                her_id, 
                                her_descripcion, 
                                her_estado, 
                                her_fecha_reg,
                                her_fecha_act
	                                FROM gestion_talento.gdt_per_ofimatica_her                                  
                                            where her_id=@p0;";
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
                                ofimaticaHerramienta.her_id = ManejoNulos.ManageNullInteger(dr["her_id"]);
                                ofimaticaHerramienta.her_descripcion = ManejoNulos.ManageNullStr(dr["her_descripcion"]);
                                ofimaticaHerramienta.her_estado = ManejoNulos.ManageNullStr(dr["her_estado"]);
                                ofimaticaHerramienta.her_fecha_reg = ManejoNulos.ManageNullDate(dr["her_fecha_reg"]);
                                ofimaticaHerramienta.her_fecha_act = ManejoNulos.ManageNullDate(dr["her_fecha_act"]);                          

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ofimaticaHerramienta;
        }
    }
}