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
    public class ExperienciaModel
    {
        string _conexion;
        public ExperienciaModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ExperienciaEntidad> ExperienciaListaporPostulanteJson(int fk_postulante)
        {
            List<ExperienciaEntidad> lista = new List<ExperienciaEntidad>();
            string consulta = @"SELECT 
                                exp_id,
                                exp_empresa, 
                                exp_cargo, 
                                exp_fecha_ini, 
                                exp_fecha_fin, 
                                exp_motivo_cese, 
                                exp_fecha_reg, 
                                exp_fecha_act, 
                                exp_estado, 
                                fk_postulante
	                                FROM gestion_talento.gdt_per_experiencia
                                where fk_postulante=@p0
                                order by exp_id desc;";
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
                                var experiencia = new ExperienciaEntidad
                                {
                                    exp_id = ManejoNulos.ManageNullInteger(dr["exp_id"]),
                                    exp_empresa = ManejoNulos.ManageNullStr(dr["exp_empresa"]),
                                    exp_cargo = ManejoNulos.ManageNullStr(dr["exp_cargo"]),
                                    exp_fecha_ini = ManejoNulos.ManageNullDate(dr["exp_fecha_ini"]),
                                    exp_fecha_fin = ManejoNulos.ManageNullDate(dr["exp_fecha_fin"]),
                                    exp_motivo_cese = ManejoNulos.ManageNullStr(dr["exp_motivo_cese"]),
                                    exp_fecha_reg = ManejoNulos.ManageNullDate(dr["exp_fecha_reg"]),
                                    exp_fecha_act = ManejoNulos.ManageNullDate(dr["exp_fecha_act"]),
                                    exp_estado = ManejoNulos.ManageNullStr(dr["exp_estado"]),                                
                                    fk_postulante = ManejoNulos.ManageNullInteger(dr["fk_postulante"]),

                                };

                                lista.Add(experiencia);
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
        public ExperienciaEntidad ExperienciaOnbtenerIdJson(int exp_id) {
            ExperienciaEntidad experiencia = new ExperienciaEntidad();
            string consulta = @"SELECT 
                            exp_id, 
                            exp_empresa, 
                            exp_cargo, 
                            exp_fecha_ini, 
                            exp_fecha_fin, 
                            exp_motivo_cese, 
                            exp_fecha_reg, 
                            exp_fecha_act, 
                            exp_estado, 
                            fk_postulante
                            FROM gestion_talento.gdt_per_experiencia
                            where exp_id=@p0; ";
            try {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", exp_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if(dr.HasRows){
                            while (dr.Read()) {
                                experiencia.exp_id = ManejoNulos.ManageNullInteger(dr["exp_id"]);
                                experiencia.exp_empresa = ManejoNulos.ManageNullStr(dr["exp_empresa"]);
                                experiencia.exp_cargo = ManejoNulos.ManageNullStr(dr["exp_cargo"]);
                                experiencia.exp_fecha_ini = ManejoNulos.ManageNullDate(dr["exp_fecha_ini"]);
                                experiencia.exp_fecha_fin = ManejoNulos.ManageNullDate(dr["exp_fecha_fin"]);
                                experiencia.exp_motivo_cese = ManejoNulos.ManageNullStr(dr["exp_motivo_cese"]);
                                experiencia.exp_fecha_reg = ManejoNulos.ManageNullDate(dr["exp_fecha_reg"]);
                                experiencia.exp_fecha_act = ManejoNulos.ManageNullDate(dr["exp_fecha_act"]);
                                experiencia.exp_estado = ManejoNulos.ManageNullStr(dr["exp_estado"]);
                                experiencia.fk_postulante = ManejoNulos.ManageNullInteger(dr["fk_postulante"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return experiencia;
        }
        public bool ExperienciaInsertarJson(ExperienciaEntidad experiencia)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_per_experiencia(                                   
                                            exp_empresa, 
                                            exp_cargo, 
                                            exp_fecha_ini,
                                            exp_fecha_fin, 
                                            exp_motivo_cese, 
                                            exp_fecha_reg,                                         
                                            exp_estado, 
                                            fk_postulante)
	                                            VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0",ManejoNulos.ManageNullStr( experiencia.exp_empresa));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(experiencia.exp_cargo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(experiencia.exp_fecha_ini));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(experiencia.exp_fecha_fin));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(experiencia.exp_motivo_cese));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate (experiencia.exp_fecha_reg));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr (experiencia.exp_estado));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(experiencia.fk_postulante));
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
        public bool ExperienciaEditarJson(ExperienciaEntidad experiencia)
        {
            bool response = false;
            string consulta = @"
                UPDATE gestion_talento.gdt_per_experiencia
                            SET 
                            exp_empresa=@p0, 
                            exp_cargo=@p1, 
                            exp_fecha_ini=@p2, 
                            exp_fecha_fin=@p3, 
                            exp_motivo_cese=@p4, 
                            exp_fecha_act=@p5, 
                            exp_estado=@p6
	                            WHERE exp_id=@p7;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(experiencia.exp_empresa));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(experiencia.exp_cargo));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullDate(experiencia.exp_fecha_ini));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(experiencia.exp_fecha_fin));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(experiencia.exp_motivo_cese));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(experiencia.exp_fecha_act));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(experiencia.exp_estado));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(experiencia.exp_id));
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
        public bool ExperienciaEliminarJson(int id)
        {
            bool response = false;
            string consulta = @"DELETE FROM gestion_talento.gdt_per_experiencia
                                WHERE  exp_id=@p0;";
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