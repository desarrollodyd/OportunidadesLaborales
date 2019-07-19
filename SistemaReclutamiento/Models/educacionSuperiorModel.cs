using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using SistemaReclutamiento.Entidades;
using Npgsql;
using SistemaReclutamiento.Utilitarios;
using System.Diagnostics;

namespace SistemaReclutamiento.Models
{
    public class educacionSuperiorModel
    {
        string _conexion;
        public educacionSuperiorModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<educacionSuperiorEntidad> EducacionSuperiorListaporPostulanteJson(int fk_postulante)
        {
            List<educacionSuperiorEntidad> lista = new List<educacionSuperiorEntidad>();
            string consulta = @"SELECT 
                                esu_id,     
                                esu_tipo, 
                                esu_centro_estudio,
                                esu_carrera, 
                                esu_periodo_ini, 
                                esu_periodo_fin,
                                esu_condicion, 
                                esu_fecha_reg, 
                                esu_fecha_act, 
                                fk_postulante
	                            FROM gestion_talento.gdt_per_educacion_sup;
                                where fk_postulante=@p0
                                order by esu_id desc;";
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
                                var educacionbasica = new educacionSuperiorEntidad
                                {
                                    esu_id = ManejoNulos.ManageNullInteger(dr["esu_id"]),
                                    esu_tipo = ManejoNulos.ManageNullStr(dr["esu_tipo"]),
                                    esu_centro_estudio = ManejoNulos.ManageNullStr(dr["esu_centro_estudio"]),
                                    esu_carrera = ManejoNulos.ManageNullStr(dr["esu_carrera"]),
                                    esu_periodo_ini = ManejoNulos.ManageNullDate(dr["esu_periodo_ini"]),
                                    esu_periodo_fin = ManejoNulos.ManageNullDate(dr["esu_periodo_fin"]),
                                    esu_condicion = ManejoNulos.ManageNullStr(dr["esu_condicion"]),
                                    esu_fecha_reg = ManejoNulos.ManageNullDate(dr["esu_fecha_reg"]),
                                    esu_fecha_act = ManejoNulos.ManageNullDate(dr["esu_fecha_act"]),
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
        public educacionSuperiorEntidad EducacionSuperiorIdObtenerJson(int esu_id)
        {
            educacionSuperiorEntidad educacionSuperior = new educacionSuperiorEntidad();
            string consulta = @"SELECT 
                                    esu_id, 
                                    esu_tipo, 
                                    esu_centro_estudio, 
                                    esu_carrera, 
                                    esu_periodo_ini, 
                                    esu_periodo_fin, 
                                    esu_condicion, 
                                    esu_fecha_reg, 
                                    esu_fecha_act, 
                                    fk_postulante
	                                    FROM gestion_talento.gdt_per_educacion_sup                                
                                            where esu_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", esu_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                educacionSuperior.esu_id = ManejoNulos.ManageNullInteger(dr["esu_id"]);
                                educacionSuperior.esu_tipo = ManejoNulos.ManageNullStr(dr["esu_tipo"]);
                                educacionSuperior.esu_centro_estudio = ManejoNulos.ManageNullStr(dr["esu_centro_estudio"]);
                                educacionSuperior.esu_carrera = ManejoNulos.ManageNullStr(dr["esu_carrera"]);
                                educacionSuperior.esu_periodo_ini = ManejoNulos.ManageNullDate(dr["esu_periodo_ini"]);
                                educacionSuperior.esu_periodo_fin = ManejoNulos.ManageNullDate(dr["esu_periodo_fin"]);
                                educacionSuperior.esu_condicion = ManejoNulos.ManageNullStr(dr["esu_condicion"]);
                                educacionSuperior.esu_fecha_reg = ManejoNulos.ManageNullDate(dr["esu_fecha_reg"]);
                                educacionSuperior.esu_fecha_act = ManejoNulos.ManageNullDate(dr["usu_fecha_act"]);
                                educacionSuperior.fk_postulante = ManejoNulos.ManageNullInteger(dr["fk_postulante"]);                             

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return educacionSuperior;
        }
        public bool EducacionSuperiorInsertarJson(educacionSuperiorEntidad educacionSuperior)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_per_educacion_sup(
	                  
                                esu_tipo, 
                                esu_centro_estudio, 
                                esu_carrera, 
                                esu_periodo_ini, 
                                esu_periodo_fin, 
                                esu_condicion, 
                                esu_fecha_reg,                            
                                fk_postulante)
	                                VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(educacionSuperior.esu_tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(educacionSuperior.esu_centro_estudio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(educacionSuperior.esu_carrera));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(educacionSuperior.esu_periodo_ini));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(educacionSuperior.esu_periodo_fin));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(educacionSuperior.esu_condicion));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(educacionSuperior.esu_fecha_reg));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(educacionSuperior.fk_postulante));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
            }
            return response;
        }
        public bool EducacionSuperiorEditarJson(educacionSuperiorEntidad educacionSuperior)
        {
            bool response = false;
            string consulta = @"
                                UPDATE 
                                gestion_talento.gdt_per_educacion_sup
	                                SET                                 
                                esu_tipo=@p0, 
                                esu_centro_estudio=@p1, 
                                esu_carrera=@p2, 
                                esu_periodo_ini=@p3, 
                                esu_periodo_fin=@p4, 
                                esu_condicion=@p5,                           
                                esu_fecha_act=@p6                    
                                WHERE esu_id=@p7;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(educacionSuperior.esu_tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(educacionSuperior.esu_centro_estudio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(educacionSuperior.esu_carrera));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(educacionSuperior.esu_periodo_ini));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(educacionSuperior.esu_periodo_fin));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(educacionSuperior.esu_condicion));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(educacionSuperior.esu_fecha_act));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(educacionSuperior.esu_id));
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