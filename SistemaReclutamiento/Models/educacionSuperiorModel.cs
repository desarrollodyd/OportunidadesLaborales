using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using SistemaReclutamiento.Entidades;
using Npgsql;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Models
{
    public class educacionSuperiorModel
    {
        string _conexion;
        public educacionSuperiorModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
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
                                educacionSuperior.usu_fecha_act = ManejoNulos.ManageNullDate(dr["usu_fecha_act"]);
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
    }
}