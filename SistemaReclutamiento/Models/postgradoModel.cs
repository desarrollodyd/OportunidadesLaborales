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
    public class postgradoModel
    {
        string _conexion;
        public postgradoModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<postgradoEntidad> PostgradoListaporPostulanteJson(int fk_postulante)
        {
            List<postgradoEntidad> lista = new List<postgradoEntidad>();
            string consulta = @"SELECT 
                                pos_id, 
                                pos_tipo, 
                                pos_centro_estudio, 
                                pos_carrera, 
                                pos_nombre, 
                                pos_periodo_ini, 
                                pos_periodo_fin, 
                                pos_condicion, 
                                pos_fecha_reg, 
                                pos_fecha_act, 
                                fk_postulante
	                                FROM gestion_talento.gdt_per_postgrado
                                where fk_postulante=@p0
                                order by pos_id desc;";
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
                                var postgrado = new postgradoEntidad
                                {
                                     pos_id= ManejoNulos.ManageNullInteger(dr["pos_id"]),
                                    pos_tipo = ManejoNulos.ManageNullStr(dr["pos_tipo"]),
                                    pos_centro_estudio = ManejoNulos.ManageNullStr(dr["pos_centro_estudio"]),
                                    pos_carrera = ManejoNulos.ManageNullStr(dr["pos_carrera"]),
                                    pos_nombre=ManejoNulos.ManageNullStr(dr["pos_nombre"]),
                                    pos_periodo_ini = ManejoNulos.ManageNullDate(dr["pos_periodo_ini"]),
                                    pos_periodo_fin = ManejoNulos.ManageNullDate(dr["pos_periodo_fin"]),
                                    pos_condicion = ManejoNulos.ManageNullStr(dr["pos_condicion"]),
                                    pos_fecha_reg = ManejoNulos.ManageNullDate(dr["pos_fecha_reg"]),
                                    pos_fecha_act = ManejoNulos.ManageNullDate(dr["pos_fecha_act"]),
                                    fk_postulante = ManejoNulos.ManageNullInteger(dr["fk_postulante"]),

                                };

                                lista.Add(postgrado);
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
        public postgradoEntidad PostgradoIdObtenerJson(int pos_id)
        {
            postgradoEntidad postgrado = new postgradoEntidad();
            string consulta = @"SELECT 
                                pos_id, 
                                pos_tipo, 
                                pos_centro_estudio, 
                                pos_carrera, 
                                pos_nombre, 
                                pos_periodo_ini, 
                                pos_periodo_fin, 
                                pos_condicion, 
                                pos_fecha_reg, 
                                pos_fecha_act, 
                                fk_postulante
	                                FROM gestion_talento.gdt_per_postgrado
                                where pos_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", pos_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                postgrado.pos_id = ManejoNulos.ManageNullInteger(dr["pos_id"]);
                                postgrado.pos_tipo = ManejoNulos.ManageNullStr(dr["pos_tipo"]);
                                postgrado.pos_centro_estudio = ManejoNulos.ManageNullStr(dr["pos_centro_estudio"]);
                                postgrado.pos_carrera = ManejoNulos.ManageNullStr(dr["pos_carrera"]);
                                postgrado.pos_nombre = ManejoNulos.ManageNullStr(dr["pos_nombre"]);
                                postgrado.pos_periodo_ini = ManejoNulos.ManageNullDate(dr["pos_periodo_ini"]);
                                postgrado.pos_periodo_fin = ManejoNulos.ManageNullDate(dr["pos_periodo_fin"]);
                                postgrado.pos_condicion = ManejoNulos.ManageNullStr(dr["pos_condicion"]);
                                postgrado.pos_fecha_reg = ManejoNulos.ManageNullDate(dr["pos_fecha_reg"]);
                                postgrado.pos_fecha_act = ManejoNulos.ManageNullDate(dr["pos_fecha_act"]);
                                postgrado.fk_postulante = ManejoNulos.ManageNullInteger(dr["fk_postulante"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return postgrado;
        }
        public bool PostgradoInsertarJson(postgradoEntidad postgrado)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_per_postgrado(
                                        pos_tipo, 
                                        pos_centro_estudio, 
                                        pos_carrera, 
                                        pos_nombre, 
                                        pos_periodo_ini, 
                                        pos_periodo_fin, 
                                        pos_condicion, 
                                        pos_fecha_reg, 
                                        fk_postulante)
	
	                                VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7,@p8); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(postgrado.pos_tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(postgrado.pos_centro_estudio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(postgrado.pos_carrera));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(postgrado.pos_nombre));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(postgrado.pos_periodo_ini));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(postgrado.pos_periodo_fin));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(postgrado.pos_condicion));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(postgrado.pos_fecha_reg));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(postgrado.fk_postulante));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return response;
        }
        public bool PostgradoEditarJson(postgradoEntidad postgrado)
        {
            bool response = false;
            string consulta = @"
                                UPDATE 
                                gestion_talento.gdt_per_postgrado
	                                SET                                 
                                pos_tipo=@p0, 
                                pos_centro_estudio=@p1, 
                                pos_nombre=@p2,
                                pos_carrera=@p3, 
                                pos_periodo_ini=@p4, 
                                pos_periodo_fin=@p5, 
                                pos_condicion=@p6,                           
                                pos_fecha_act=@p7                    
                                WHERE pos_id=@p8;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(postgrado.pos_tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(postgrado.pos_centro_estudio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(postgrado.pos_carrera));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(postgrado.pos_nombre));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(postgrado.pos_periodo_ini));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(postgrado.pos_periodo_fin));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(postgrado.pos_condicion));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(postgrado.pos_fecha_act));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(postgrado.pos_id));
                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return response;
        }
        public bool PostgradoEliminarJson(int id)
        {
            bool response = false;
            string consulta = @"DELETE FROM gestion_talento.gdt_per_postgrado
                                WHERE  pos_id=@p0;";
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
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }

            return response;
        }
    }
}