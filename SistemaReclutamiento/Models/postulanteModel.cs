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
        public postulanteEntidad PostulanteIdObtenerporUsuarioJson(int fk_usuario)
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
                                        fk_persona,
                                        fk_usuario
	                                        FROM gestion_talento.gdt_per_postulante
	                                  
                                            where fk_usuario=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_usuario);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var pos_foto = ManejoNulos.ManageNullStr(dr["pos_foto"]) == "" ? "user.png" : dr["pos_foto"];
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
                                postulante.pos_foto = pos_foto.ToString();
                                postulante.pos_situacion = ManejoNulos.ManageNullStr(dr["pos_situacion"]);
                                postulante.pos_fecha_reg = ManejoNulos.ManageNullDate(dr["pos_fecha_reg"]);
                                postulante.pos_fecha_act = ManejoNulos.ManageNullDate(dr["pos_fecha_act"]);
                                postulante.pos_estado = ManejoNulos.ManageNullStr(dr["pos_estado"]);
                                postulante.fk_persona = ManejoNulos.ManageNullInteger(dr["fk_persona"]);
                                postulante.pos_id = ManejoNulos.ManageNullInteger(dr["pos_id"]);
                                postulante.fk_usuario=ManejoNulos.ManageNullInteger(dr["fk_usuario"]);
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
        public postulanteEntidad PostulanteIdObtenerJson(int pos_id)
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
                                var pos_foto = ManejoNulos.ManageNullStr(dr["pos_foto"]) == "" ? "user.png" : dr["pos_foto"];
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
                                postulante.pos_foto = pos_foto.ToString();
                                postulante.pos_situacion = ManejoNulos.ManageNullStr(dr["pos_situacion"]);
                                postulante.pos_fecha_reg = ManejoNulos.ManageNullDate(dr["pos_fecha_reg"]);
                                postulante.pos_fecha_act = ManejoNulos.ManageNullDate(dr["pos_fecha_act"]);
                                postulante.pos_estado = ManejoNulos.ManageNullStr(dr["pos_estado"]);
                                postulante.fk_persona = ManejoNulos.ManageNullInteger(dr["fk_persona"]);
                                postulante.pos_id = ManejoNulos.ManageNullInteger(dr["pos_id"]);
                                postulante.fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]);
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
        public List<ofertaLaboralEntidad> PostulanteListarPostulacionesJson(int pos_id) {
            List<ofertaLaboralEntidad> postulaciones = new List<ofertaLaboralEntidad>();
            string consulta = @"SELECT 
                                ola_id, 
                                ola_nombre, 
                                ola_requisitos, 
                                ola_funciones, 
                                ola_competencias, 
                                ola_condiciones_lab, 
                                ola_vacantes,
                                ola_enviar, 
                                ola_enviado, 
                                ola_publicado, 
                                ola_fecha_pub, 
                                ola_estado_oferta, 
                                ola_duracion, 
                                ola_fecha_fin, 
                                ola_fecha_reg, 
                                ola_fecha_act,
                                ola_estado, 
                                ola_cod_empresa, 
                                ola_cod_cargo, 
                                fk_ubigeo, 
                                fk_usuario
	                            FROM gestion_talento.gdt_ola_oferta_laboral as oferta_laboral
	                            INNER JOIN gestion_talento.gdt_pos_postulacion as postulacion
	                            on oferta_laboral.ola_id=postulacion.fk_oferta_laboral
	                            where postulacion.fk_postulante=@p0
	                            ;";
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
                                var postulacion = new ofertaLaboralEntidad
                                {
                                    ola_id = ManejoNulos.ManageNullInteger(dr["ola_id"]),
                                    ola_nombre = ManejoNulos.ManageNullStr(dr["ola_nombre"]),
                                    ola_requisitos = ManejoNulos.ManageNullStr(dr["ola_requisitos"]),
                                    ola_funciones= ManejoNulos.ManageNullStr(dr["ola_funciones"]),
                                    ola_competencias= ManejoNulos.ManageNullStr(dr["ola_competencias"]),
                                    ola_condiciones_lab= ManejoNulos.ManageNullStr(dr["ola_condiciones_lab"]),
                                    ola_vacantes= ManejoNulos.ManageNullInteger(dr["ola_vacantes"]),
                                    ola_enviar = ManejoNulos.ManegeNullBool(dr["ola_enviar"]),
                                    ola_enviado = ManejoNulos.ManegeNullBool(dr["ola_enviado"]),
                                    ola_publicado = ManejoNulos.ManegeNullBool(dr["ola_publicado"]),
                                    ola_fecha_pub = ManejoNulos.ManageNullDate(dr["ola_fecha_pub"]),
                                    ola_estado_oferta = ManejoNulos.ManageNullStr(dr["ola_estado_oferta"]),
                                    ola_duracion = ManejoNulos.ManageNullInteger(dr["ola_duracion"]),
                                    ola_fecha_fin = ManejoNulos.ManageNullDate(dr["ola_fecha_fin"]),
                                    ola_fecha_reg = ManejoNulos.ManageNullDate(dr["ola_fecha_reg"]),
                                    ola_fecha_act = ManejoNulos.ManageNullDate(dr["ola_fecha_act"]),
                                    ola_estado = ManejoNulos.ManageNullStr(dr["ola_estado"]),
                                    ola_cod_empresa = ManejoNulos.ManageNullStr(dr["ola_cod_empresa"]),
                                    ola_cod_cargo = ManejoNulos.ManageNullStr(dr["ola_cod_cargo"]),
                                    fk_ubigeo = ManejoNulos.ManageNullInteger(dr["fk_ubigeo"]),
                                    fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"])
                                };

                                postulaciones.Add(postulacion);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return postulaciones;
        }
        public bool PostulanteInsertarJson(postulanteEntidad postulante)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_per_postulante(
                                pos_fecha_reg, 
                                pos_estado, 
                                fk_persona,
                                pos_foto,
                                fk_usuario)
	                                VALUES (@p0, @p1, @p2,@p3,@p4); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullDate( postulante.pos_fecha_reg));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(postulante.pos_estado));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(postulante.fk_persona));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(postulante.pos_foto));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullInteger(postulante.fk_usuario));
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
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(postulante.pos_id));
            
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
        public int PostulanteTablaPostulacionInsertarJson(postulanteEntidad postulante, int fk_oferta_laboral)
        {
            bool response = false;
            int idPostulacionInsertada = 0;
            string consulta = @"INSERT INTO gestion_talento.gdt_pos_postulacion(                         
                                ppo_tipo_direccion, 
                                ppo_direccion, 
                                ppo_tipo_calle, 
                                ppo_numero_casa, 
                                ppo_tipo_casa,
                                ppo_celular,
                                ppo_estado_civil,
                                ppo_brevete,
                                ppo_num_brevete,
                                ppo_referido,
                                ppo_nombre_referido, 
                                ppo_cv,
                                ppo_foto, 
                                ppo_situacion, 
                                ppo_fecha_reg, 
                                ppo_fecha_act, 
                                ppo_estado, 
                                fk_postulante, 
                                fk_oferta_laboral)
	                            VALUES 
                                (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18)
                                    returning ppo_id;";
            try {
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
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManegeNullBool(postulante.pos_referido));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullStr(postulante.pos_nombre_referido));
                    query.Parameters.AddWithValue("@p11", ManejoNulos.ManageNullStr(postulante.pos_cv));
                    query.Parameters.AddWithValue("@p12", ManejoNulos.ManageNullStr(postulante.pos_foto));
                    query.Parameters.AddWithValue("@p13", ManejoNulos.ManageNullStr(postulante.pos_situacion));
                    query.Parameters.AddWithValue("@p14", ManejoNulos.ManageNullDate(postulante.pos_fecha_reg));
                    query.Parameters.AddWithValue("@p15", ManejoNulos.ManageNullDate(postulante.pos_fecha_act));
                    query.Parameters.AddWithValue("@p16", ManejoNulos.ManageNullStr(postulante.pos_estado));
                    query.Parameters.AddWithValue("@p17", ManejoNulos.ManageNullInteger(postulante.pos_id));
                    query.Parameters.AddWithValue("@p18", ManejoNulos.ManageNullInteger(fk_oferta_laboral));
                    idPostulacionInsertada = Int32.Parse(query.ExecuteScalar().ToString());
                    //query.ExecuteNonQuery();
                    //response = true;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return idPostulacionInsertada;
        }
        public bool PostulanteTablaPostulacionEducacionBasicaInsertarJson(educacionBasicaEntidad educacionBasica, int fk_oferta_laboral)
        {
            bool response = false;
            string consulta = @"INSERT INTO 
                                gestion_talento.gdt_pos_educacion_bas(
                                eba_tipo,
                                eba_nombre,
                                eba_condicion,
                                eba_fecha_reg, 
                                eba_fecha_act,
                                fk_postulante,
                                fk_oferta_laboral)
	                            VALUES (@p0, @p1, @p2, @p3, @p4,@p5,@p6); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(educacionBasica.eba_tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(educacionBasica.eba_nombre));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(educacionBasica.eba_condicion));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(educacionBasica.eba_fecha_reg));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(educacionBasica.eba_fecha_act));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullInteger(educacionBasica.fk_postulante));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullInteger(fk_oferta_laboral));
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
        public bool PostulanteTablaPostulacionEducacionSuperiorInsertarJson(educacionSuperiorEntidad educacionSuperior, int fk_oferta_laboral)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_pos_educacion_sup(                    
                                esu_tipo, 
                                esu_centro_estudio, 
                                esu_carrera, 
                                esu_periodo_ini, 
                                esu_periodo_fin,
                                esu_condicion, 
                                eso_fecha_reg, 
                                esu_fecha_act, 
                                fk_postulante,
                                fk_oferta_laboral)
                                VALUES 
                                (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7,@p8,@p9); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(educacionSuperior.esu_tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(educacionSuperior.esu_centro_estudio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(educacionSuperior.esu_carrera));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(educacionSuperior.esu_periodo_ini));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(educacionSuperior.esu_periodo_fin));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(educacionSuperior.esu_condicion));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(educacionSuperior.esu_fecha_reg));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(educacionSuperior.esu_fecha_act));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(educacionSuperior.fk_postulante));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(fk_oferta_laboral));
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
        public bool PostulanteTablaPostulacionExperienciaInsertarJson(experienciaEntidad experiencia, int fk_oferta_laboral)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_pos_experiencia(
                                    exp_empresa,
                                    exp_cargo,
                                    exp_fecha_ini,
                                    exp_fecha_fin,
                                    exp_motivo_cese,
                                    exp_fecha_reg, 
                                    exp_fecha_act, 
                                    exp_estado,
                                    fk_postulante,
                                    fk_oferta_laboral)
                                    VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7,@p8,@p9); ";
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
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(experiencia.exp_fecha_reg));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(experiencia.exp_fecha_act));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullStr(experiencia.exp_estado));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(experiencia.fk_postulante));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(fk_oferta_laboral));
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
        public bool PostulanteTablaPostulacionIdiomaInsertarJson(idiomaEntidad idioma, int fk_oferta_laboral)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_pos_idioma(
                                    idi_tipo, 
                                    idi_centro_estudio, 
                                    idi_idioma, 
                                    idi_periodo_ini, 
                                    idi_periodo_fin, 
                                    idi_nivel, 
                                    idi_fecha_reg, 
                                    idi_fecha_act, 
                                    fk_postulante, 
                                    fk_oferta_laboral)
                                    VALUES (@p0, @p1, @p2, @p3, @p4,@p5,@p6,@p7,@p8,@p9); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(idioma.idi_tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(idioma.idi_centro_estudio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(idioma.idi_idioma));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(idioma.idi_periodo_ini));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(idioma.idi_periodo_fin));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(idioma.idi_nivel));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(idioma.idi_fecha_reg));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(idioma.idi_fecha_act));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(idioma.fk_postulante));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(fk_oferta_laboral));
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
        public bool PostulanteTablaPostulacionOfimaticaInsertarJson(ofimaticaEntidad ofimatica, string ofi_herramienta, int fk_oferta_laboral)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_pos_ofimatica(
                                    ofi_tipo,
                                    ofi_centro_estudio, 
                                    ofi_herramienta, 
                                    ofi_nivel,
                                    ofi_periodo_ini,
                                    ofi_periodo_fin,
                                    ofi_fecha_reg, 
                                    ofi_fecha_act,
                                    fk_postulante,
                                    fk_oferta_laboral)
	                                VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7,@p8,@p9); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(ofimatica.ofi_tipo));
                    query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(ofimatica.ofi_centro_estudio));
                    query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullStr(ofi_herramienta));
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullStr(ofimatica.ofi_nivel));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(ofimatica.ofi_periodo_ini));
                    query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullDate(ofimatica.ofi_periodo_fin));
                    query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(ofimatica.ofi_fecha_reg));
                    query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullDate(ofimatica.ofi_fecha_act));
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullInteger(ofimatica.fk_postulante));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(fk_oferta_laboral));
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
        public bool PostulanteTablaPostulacionPostgradoInsertarJson(postgradoEntidad postgrado, int fk_oferta_laboral)
        {
            bool response = false;
            string consulta = @"INSERT INTO gestion_talento.gdt_pos_postgrado(
                                    pos_tipo,
                                    pos_centro_estudio,
                                    pos_carrera, 
                                    pos_nombre,
                                    pos_periodo_ini,
                                    pos_periodo_fin,
                                    pos_condicion, 
                                    pos_fecha_reg,
                                    pos_fecha_act, 
                                    fk_postulante, 
                                    fk_oferta_laboral)
	                                VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7,@p8,@p9,@p10); ";
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
                    query.Parameters.AddWithValue("@p8", ManejoNulos.ManageNullDate(postgrado.pos_fecha_act));
                    query.Parameters.AddWithValue("@p9", ManejoNulos.ManageNullInteger(postgrado.fk_postulante));
                    query.Parameters.AddWithValue("@p10", ManejoNulos.ManageNullInteger(fk_oferta_laboral));
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
    }
}