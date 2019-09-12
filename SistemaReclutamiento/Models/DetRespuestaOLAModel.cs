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
    public class DetRespuestaOLAModel
    {
        string _conexion;
        public DetRespuestaOLAModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<DetRespuestaOLAEntidad> DetRespuestaListarporPreguntaJson(int fk_det_pregunta_of)
        {
            List<DetRespuestaOLAEntidad> lista = new List<DetRespuestaOLAEntidad>();
            string consulta = @"SELECT dro_id, dro_respuesta, dro_tipo, dro_orden, dro_estado, fk_det_pregunta_of
	FROM gestion_talento.gdt_ola_det_respuesta_of where fk_det_pregunta_of=@p0 and dro_estado='A'
                                order by dro_orden asc;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_det_pregunta_of);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var detalle = new DetRespuestaOLAEntidad
                                {
                                    dro_id = ManejoNulos.ManageNullInteger(dr["dro_id"]),
                                    dro_respuesta = ManejoNulos.ManageNullStr(dr["dro_respuesta"]),
                                    dro_tipo = ManejoNulos.ManageNullStr(dr["dro_tipo"]),
                                    dro_orden = ManejoNulos.ManageNullInteger(dr["dro_orden"]),
                                    dro_estado = ManejoNulos.ManageNullStr(dr["dro_estado"]),
                                    fk_det_pregunta_of = ManejoNulos.ManageNullInteger(dr["fk_det_pregunta_of"])
                                };

                                lista.Add(detalle);
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
        //public IdiomaEntidad IdiomaIdObtenerJson(int idi_id)
        //{
        //    IdiomaEntidad idioma = new IdiomaEntidad();
        //    string consulta = @"SELECT 
        //                        idi_id, 
        //                        idi_tipo, 
        //                        idi_centro_estudio, 
        //                        fk_idioma, 
        //                        idi_periodo_ini, 
        //                        idi_periodo_fin, 
        //                        idi_nivel, 
        //                        idi_fecha_reg, 
        //                        idi_fecha_act, 
        //                        fk_postulante
	       //                         FROM gestion_talento.gdt_per_idioma
        //                        where idi_id=@p0;";
        //    try
        //    {
        //        using (var con = new NpgsqlConnection(_conexion))
        //        {
        //            con.Open();
        //            var query = new NpgsqlCommand(consulta, con);
        //            query.Parameters.AddWithValue("@p0", idi_id);
        //            using (var dr = query.ExecuteReader())
        //            {
        //                if (dr.HasRows)
        //                {
        //                    while (dr.Read())
        //                    {
        //                        idioma.idi_id = ManejoNulos.ManageNullInteger(dr["idi_id"]);
        //                        idioma.idi_tipo = ManejoNulos.ManageNullStr(dr["idi_tipo"]);
        //                        idioma.idi_centro_estudio = ManejoNulos.ManageNullStr(dr["idi_centro_estudio"]);
        //                        idioma.fk_idioma = ManejoNulos.ManageNullInteger(dr["fk_idioma"]);
        //                        idioma.idi_periodo_ini = ManejoNulos.ManageNullDate(dr["idi_periodo_ini"]);
        //                        idioma.idi_periodo_fin = ManejoNulos.ManageNullDate(dr["idi_periodo_fin"]);
        //                        idioma.idi_nivel = ManejoNulos.ManageNullStr(dr["idi_nivel"]);
        //                        idioma.idi_fecha_reg = ManejoNulos.ManageNullDate(dr["idi_fecha_reg"]);
        //                        idioma.idi_fecha_act = ManejoNulos.ManageNullDate(dr["idi_fecha_act"]);
        //                        idioma.fk_postulante = ManejoNulos.ManageNullInteger(dr["fk_postulante"]);

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
        //    }
        //    return idioma;
        //}
        //public bool IdiomaInsertarJson(IdiomaEntidad idioma)
        //{
        //    bool response = false;
        //    string consulta = @"INSERT INTO gestion_talento.gdt_per_idioma(
	                         
        //                        idi_tipo, 
        //                        idi_centro_estudio,
        //                        fk_idioma, 
        //                        idi_periodo_ini, 
        //                        idi_periodo_fin, 
        //                        idi_nivel, 
        //                        idi_fecha_reg,                             
        //                        fk_postulante)	
	       //                                     VALUES (@p0, @p1, @p2, @p3, @p4,@p5,@p6,@p7); ";
        //    try
        //    {
        //        using (var con = new NpgsqlConnection(_conexion))
        //        {
        //            con.Open();
        //            var query = new NpgsqlCommand(consulta, con);
        //            query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(idioma.idi_tipo));
        //            query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(idioma.idi_centro_estudio));
        //            query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(idioma.fk_idioma));
        //            query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(idioma.idi_periodo_ini));
        //            query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(idioma.idi_periodo_fin));
        //            query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(idioma.idi_nivel));
        //            query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(idioma.idi_fecha_reg));
        //            query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(idioma.fk_postulante));
        //            query.ExecuteNonQuery();
        //            response = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
        //    }
        //    return response;
        //}
        //public bool IdiomaEditarJson(IdiomaEntidad idioma)
        //{
        //    bool response = false;
        //    string consulta = @"
        //        UPDATE gestion_talento.gdt_per_idioma
	       //                 SET 
        //                    idi_tipo=@p0, 
        //                    idi_centro_estudio=@p1, 
        //                    fk_idioma=@p2, 
        //                    idi_periodo_ini=@p3, 
        //                    idi_periodo_fin=@p4, 
        //                    idi_nivel=@p5,                     
        //                    idi_fecha_act=@p6
	       //                 WHERE idi_id=@p7;";
        //    try
        //    {
        //        using (var con = new NpgsqlConnection(_conexion))
        //        {
        //            con.Open();
        //            var query = new NpgsqlCommand(consulta, con);
        //            query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullStr(idioma.idi_tipo));
        //            query.Parameters.AddWithValue("@p1", ManejoNulos.ManageNullStr(idioma.idi_centro_estudio));
        //            query.Parameters.AddWithValue("@p2", ManejoNulos.ManageNullInteger(idioma.fk_idioma));
        //            query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(idioma.idi_periodo_ini));
        //            query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(idioma.idi_periodo_fin));
        //            query.Parameters.AddWithValue("@p5", ManejoNulos.ManageNullStr(idioma.idi_nivel));
        //            query.Parameters.AddWithValue("@p6", ManejoNulos.ManageNullDate(idioma.idi_fecha_act));
        //            query.Parameters.AddWithValue("@p7", ManejoNulos.ManageNullInteger(idioma.idi_id));
        //            query.ExecuteNonQuery();
        //            response = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return response;
        //}
        //public bool IdiomaEliminarJson(int id)
        //{
        //    bool response = false;
        //    string consulta = @"DELETE FROM gestion_talento.gdt_per_idioma
        //                        WHERE  idi_id=@p0;";
        //    try
        //    {
        //        using (var con = new NpgsqlConnection(_conexion))
        //        {
        //            con.Open();

        //            var query = new NpgsqlCommand(consulta, con);
        //            query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(id));
        //            query.ExecuteNonQuery();
        //            response = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    return response;
        //}
    }
}
