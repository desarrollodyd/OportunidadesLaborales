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
    public class postulanteModel
    {
        string _conexion;
        public postulanteModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public postulanteEntidad PostulanteIdObtenerporPersonaJson(int fk_persona)
        {
            postulanteEntidad postulante = new postulanteEntidad();
            string consulta = @"SELECT pos_id, 
                                        pos_tipo_direccion, 
                                        pos_direccion, 
                                        pos_tipo_calle, 
                                        pos_numero_casa, 
                                        pos_tipo_casa, 
                                        pos_celular, 
                                        pos_estado_civil, 
                                        pos_brevete, 
                                        pos_num_brevete, 
                                        pos_referido, 
                                        pos_nombre_referido, 
                                        pos_cv, 
                                        pos_foto, 
                                        pos_situacion, 
                                        pos_fecha_reg, 
                                        pos_fecha_act, 
                                        pos_estado, 
                                        fk_persona
	                                        FROM gestion_talento.gdt_per_postulante
	                                  
                                            where fk_persona=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_persona);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                postulante.pos_tipo_direccion = ManejoNulos.ManageNullStr(dr["pos_tipo_direccion"]);
                                postulante.pos_direccion = ManejoNulos.ManageNullStr(dr["pos_direccion"]);
                                postulante.pos_tipo_calle = ManejoNulos.ManageNullStr(dr["pos_tipo_calle"]);
                                postulante.pos_numero_casa = ManejoNulos.ManageNullStr(dr["pos_numero_casa"]);
                                postulante.pos_tipo_casa = ManejoNulos.ManageNullStr(dr["pos_tipo_casa"]);
                                postulante.pos_celular = ManejoNulos.ManageNullStr(dr["pos_celular"]);
                                postulante.pos_estado_civil = ManejoNulos.ManageNullStr(dr["pos_estado_civil"]);
                                postulante.pos_brevete = ManejoNulos.ManegeNullBool(dr["pos_brevete"]);
                                postulante.pos_num_brevete = ManejoNulos.ManageNullStr(dr["pos_num_brevete"]);
                                postulante.pos_referido = ManejoNulos.ManegeNullBool(dr["pos_referido"]);
                                postulante.pos_nombre_referido = ManejoNulos.ManageNullStr(dr["pos_nombre_referido"]);
                                postulante.pos_cv = ManejoNulos.ManageNullStr(dr["pos_cv"]);
                                postulante.pos_foto = ManejoNulos.ManageNullStr(dr["pos_foto"]);
                                postulante.pos_situacion = ManejoNulos.ManageNullStr(dr["pos_situacion"]);
                                postulante.pos_fecha_reg = ManejoNulos.ManageNullDate(dr["pos_fecha_reg"]);
                                postulante.pos_fecha_act = ManejoNulos.ManageNullDate(dr["pos_fecha_act"]);
                                postulante.pos_estado = ManejoNulos.ManageNullStr(dr["pos_estado"]);
                                postulante.fk_persona = ManejoNulos.ManageNullInteger(dr["fk_persona"]);
                                postulante.pos_id = ManejoNulos.ManageNullInteger(dr["pos_id"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return postulante;
        }
        public bool PostulanteInsertarJson(postulanteEntidad postulante)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_per_postulante(
                                pos_fecha_reg, 
                                pos_estado, 
                                fk_persona)
	                                VALUES (@p0, @p1, @p2); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullDate( postulante.pos_fecha_reg));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(postulante.pos_estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(postulante.fk_persona));
                

                    query.ExecuteNonQuery();
                    response = true;
                }
            }
            catch (Exception ex)
            {
            }
            return response;
        }
        public bool PostulanteEditarJson(postulanteEntidad postulante)
        {
            bool response = false;
            string consulta = @"
                UPDATE gestion_talento.gdt_per_postulante
	                    SET 
                    pos_tipo_direccion=@p0, 
                    pos_direccion=@p1, 
                    pos_tipo_calle=@p2, 
                    pos_numero_casa=@p3, 
                    pos_tipo_casa=@p4, 
                    pos_celular=@p5, 
                    pos_estado_civil=@p6, 
                    pos_brevete=@p7, 
                    pos_num_brevete=@p8,              
                    pos_fecha_act=@p9
 	                    WHERE pos_id=@p10;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(postulante.pos_tipo_direccion));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(postulante.pos_direccion));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(postulante.pos_tipo_calle));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(postulante.pos_numero_casa));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullStr(postulante.pos_tipo_casa));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(postulante.pos_celular));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullStr(postulante.pos_estado_civil));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManegeNullBool(postulante.pos_brevete));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullStr(postulante.pos_num_brevete));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullDate(postulante.pos_fecha_act));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(postulante.pos_id));               
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
        public bool PostulanteSubirFotoJson(postulanteEntidad postulante)
        {
            bool response = false;
            string consulta = @"
                UPDATE gestion_talento.gdt_per_postulante
	                    SET 
                    pos_foto=@p0,                                
                    pos_fecha_act=@p1 
 	                    WHERE pos_id=@p2;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(postulante.pos_foto));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullDate(postulante.pos_fecha_act));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(postulante.pos_id));
            
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
        public bool PostulanteInsertarInformacionAdicionalJson(postulanteEntidad postulante)
        {
            bool response = false;
            string consulta = @"
                UPDATE gestion_talento.gdt_per_postulante
	                    SET 
                        pos_referido=@p0,                                
                        pos_nombre_referido=@p1,
                        pos_cv=@p2,
                        pos_fecha_act=@p3   
 	                    WHERE pos_id=@p4;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManegeNullBool(postulante.pos_referido));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(postulante.pos_nombre_referido));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(postulante.pos_cv));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(postulante.pos_fecha_act));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(postulante.pos_id));
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