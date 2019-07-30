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
    public class ofimaticaModel
    {
        string _conexion;
        public ofimaticaModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ofimaticaEntidad> OfimaticaListaporPostulanteJson(int fk_postulante)
        {
            List<ofimaticaEntidad> lista = new List<ofimaticaEntidad>();
            string consulta = @"SELECT 
                                ofi_id, 
                                ofi_tipo, 
                                ofi_centro_estudio, 
                                fk_herramienta, 
                                ofi_nivel, 
                                ofi_periodo_ini, 
                                ofi_periodo_fin, 
                                ofi_fecha_reg, 
                                ofi_fecha_act, 
                                fk_postulante
	                            FROM gestion_talento.gdt_per_ofimatica
                                where fk_postulante=@p0
                                order by ofi_id desc;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_postulante);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var educacionbasica = new ofimaticaEntidad
                                {
                                    ofi_id = ManejoNulos.ManageNullInteger(dr["ofi_id"]),
                                    ofi_tipo = ManejoNulos.ManageNullStr(dr["ofi_tipo"]),
                                    ofi_centro_estudio = ManejoNulos.ManageNullStr(dr["ofi_centro_estudio"]),
                                    fk_herramienta = ManejoNulos.ManageNullInteger(dr["fk_herramienta"]),
                                    ofi_nivel= ManejoNulos.ManageNullStr(dr["ofi_nivel"]),
                                    ofi_periodo_ini = ManejoNulos.ManageNullDate(dr["ofi_periodo_ini"]),
                                    ofi_periodo_fin = ManejoNulos.ManageNullDate(dr["ofi_periodo_fin"]),                            
                                    ofi_fecha_reg = ManejoNulos.ManageNullDate(dr["ofi_fecha_reg"]),
                                    ofi_fecha_act = ManejoNulos.ManageNullDate(dr["ofi_fecha_act"]),
                                    fk_postulante = ManejoNulos.ManageNullInteger(dr["fk_postulante"]),

                                };

                                lista.Add(educacionbasica);
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
        public ofimaticaEntidad OfimaticaIdObtenerJson(int ofi_id)
        {
            ofimaticaEntidad ofimatica = new ofimaticaEntidad();
            string consulta = @"SELECT 
                                ofi_id, 
                                ofi_tipo, 
                                ofi_centro_estudio, 
                                fk_herramienta, 
                                ofi_nivel, 
                                ofi_periodo_ini, 
                                ofi_periodo_fin, 
                                ofi_fecha_reg, 
                                ofi_fecha_act, 
                                fk_postulante
	                            FROM gestion_talento.gdt_per_ofimatica
                                where ofi_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ofi_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ofimatica.ofi_id = ManejoNulos.ManageNullInteger(dr["ofi_id"]);
                                ofimatica.ofi_tipo = ManejoNulos.ManageNullStr(dr["ofi_tipo"]);
                                ofimatica.ofi_centro_estudio = ManejoNulos.ManageNullStr(dr["ofi_centro_estudio"]);
                                ofimatica.fk_herramienta = ManejoNulos.ManageNullInteger(dr["fk_herramienta"]);
                                ofimatica.ofi_nivel = ManejoNulos.ManageNullStr(dr["ofi_nivel"]);
                                ofimatica.ofi_periodo_ini = ManejoNulos.ManageNullDate(dr["ofi_periodo_ini"]);
                                ofimatica.ofi_periodo_fin = ManejoNulos.ManageNullDate(dr["ofi_periodo_fin"]);                           
                                ofimatica.ofi_fecha_reg = ManejoNulos.ManageNullDate(dr["ofi_fecha_reg"]);
                                ofimatica.ofi_fecha_act = ManejoNulos.ManageNullDate(dr["ofi_fecha_act"]);
                                ofimatica.fk_postulante = ManejoNulos.ManageNullInteger(dr["fk_postulante"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ofimatica;
        }
        public bool OfimaticaInsertarJson(ofimaticaEntidad ofimatica)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_per_ofimatica(                                     
                                            ofi_tipo, 
                                            ofi_centro_estudio, 
                                            fk_herramienta, 
                                            ofi_nivel, 
                                            ofi_periodo_ini, 
                                            ofi_periodo_fin, 
                                            ofi_fecha_reg,                                           
                                            fk_postulante)	
	                                VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(ofimatica.ofi_tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(ofimatica.ofi_centro_estudio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(ofimatica.fk_herramienta));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(ofimatica.ofi_nivel));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(ofimatica.ofi_periodo_ini));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(ofimatica.ofi_periodo_fin));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(ofimatica.ofi_fecha_reg));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(ofimatica.fk_postulante));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return response;
        }
        public bool OfimaticaEditarJson(ofimaticaEntidad ofimatica)
        {
            bool response = false;
            string consulta = @"
                                UPDATE gestion_talento.gdt_per_ofimatica
	                                    SET                                   
                                        ofi_tipo=@p0, 
                                        ofi_centro_estudio=@p1, 
                                        fk_herramienta=@p2, 
                                        ofi_nivel=@p3, 
                                        ofi_periodo_ini=@p4, 
                                        ofi_periodo_fin=@p5,                                         
                                        ofi_fecha_act=@p7                                      
	                                        WHERE ofi_id=@p7;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(ofimatica.ofi_tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(ofimatica.ofi_centro_estudio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(ofimatica.fk_herramienta));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(ofimatica.ofi_nivel));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(ofimatica.ofi_periodo_ini));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(ofimatica.ofi_periodo_fin));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(ofimatica.ofi_fecha_act));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(ofimatica.ofi_id));
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
        public bool OfimaticaEliminarJson(int id)
        {
            bool response = false;
            string consulta = @"DELETE FROM gestion_talento.gdt_per_ofimatica
                                WHERE  ofi_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();

                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(id));
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