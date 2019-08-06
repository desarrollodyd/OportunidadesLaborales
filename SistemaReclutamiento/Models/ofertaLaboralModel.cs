using Npgsql;
using SistemaReclutamiento.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Models
{
    public class ofertaLaboralModel
    {
        string _conexion;
        public ofertaLaboralModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<ofertaLaboralEntidad> OfertaLaboralListarJson(ReporteOfertaLaboral filtros)
        {
            List<ofertaLaboralEntidad> lista = new List<ofertaLaboralEntidad>();
            
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
	                                FROM 
                                    gestion_talento.gdt_ola_oferta_laboral where ";
           
                
            if (filtros.ola_cod_empresa != "" && filtros.ola_cod_empresa != null)
            {
                consulta += "ola_cod_empresa='"+ManejoNulos.ManageNullStr(filtros.ola_cod_empresa) +"' and ";
            }
            if (filtros.ola_cod_cargo != "" && filtros.ola_cod_cargo != null)
            {
                consulta += "ola_cod_cargo='" + ManejoNulos.ManageNullStr(filtros.ola_cod_cargo) + "' and ";
            }
            if (filtros.ola_fecha_ini!=null)
            {
                consulta += "ola_fecha_pub between '" + ManejoNulos.ManageNullDate(filtros.ola_fecha_ini) + "' and '" + DateTime.Now + "' and ";
            }
            if (filtros.ubi_distrito_id != 0)
            {
                consulta += "fk_ubigeo=" + ManejoNulos.ManageNullInteger(filtros.ubi_distrito_id) + " and ";
            }

            if (filtros.ola_nombre != "" && filtros.ola_nombre != null)
            {
                consulta += "lower(ola_nombre) Like '%" + ManejoNulos.ManageNullStr(filtros.ola_nombre.ToLower()) + "%' and ";
            }

            consulta += "ola_estado='A'";         
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var oferta = new ofertaLaboralEntidad
                            {
                                ola_id = ManejoNulos.ManageNullInteger(dr["ola_id"]),
                                ola_nombre = ManejoNulos.ManageNullStr(dr["ola_nombre"]),
                                ola_requisitos = ManejoNulos.ManageNullStr(dr["ola_requisitos"]),
                                ola_funciones = ManejoNulos.ManageNullStr(dr["ola_funciones"]),
                                ola_competencias = ManejoNulos.ManageNullStr(dr["ola_competencias"]),
                                ola_condiciones_lab = ManejoNulos.ManageNullStr(dr["ola_condiciones_lab"]),
                                ola_vacantes=ManejoNulos.ManageNullInteger(dr["ola_vacantes"]),
                                ola_enviar=ManejoNulos.ManegeNullBool(dr["ola_enviar"]),
                                ola_enviado=ManejoNulos.ManegeNullBool(dr["ola_enviado"]),
                                ola_publicado=ManejoNulos.ManegeNullBool(dr["ola_publicado"]),
                                ola_fecha_pub=ManejoNulos.ManageNullDate(dr["ola_fecha_pub"]),
                                ola_estado_oferta=ManejoNulos.ManageNullStr(dr["ola_estado_oferta"]),
                                ola_duracion=ManejoNulos.ManageNullInteger(dr["ola_duracion"]),
                                ola_fecha_fin=ManejoNulos.ManageNullDate(dr["ola_fecha_fin"]),
                                ola_fecha_reg=ManejoNulos.ManageNullDate(dr["ola_fecha_reg"]),
                                ola_fecha_act=ManejoNulos.ManageNullDate(dr["ola_fecha_act"]),
                                ola_estado=ManejoNulos.ManageNullStr(dr["ola_estado"]),
                                ola_cod_empresa=ManejoNulos.ManageNullStr(dr["ola_cod_empresa"]),
                                ola_cod_cargo=ManejoNulos.ManageNullStr(dr["ola_cod_cargo"]),
                                fk_ubigeo=ManejoNulos.ManageNullInteger(dr["fk_ubigeo"]),
                                fk_usuario=ManejoNulos.ManageNullInteger(dr["fk_usuario"])
                            };
                            lista.Add(oferta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return lista;
        }       
        public ofertaLaboralEntidad OfertaLaboralIdObtenerJson(int ola_id)
        {
            ofertaLaboralEntidad ofertalaboral = new ofertaLaboralEntidad();
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
	                                FROM 
                                    gestion_talento.gdt_ola_oferta_laboral where ola_id=@p0";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ola_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ofertalaboral.ola_id = ManejoNulos.ManageNullInteger(dr["ola_id"]);
                                ofertalaboral.ola_nombre = ManejoNulos.ManageNullStr(dr["ola_nombre"]);
                                ofertalaboral.ola_requisitos = ManejoNulos.ManageNullStr(dr["ola_requisitos"]);
                                ofertalaboral.ola_funciones = ManejoNulos.ManageNullStr(dr["ola_funciones"]);
                                ofertalaboral.ola_competencias = ManejoNulos.ManageNullStr(dr["ola_competencias"]);
                                ofertalaboral.ola_condiciones_lab = ManejoNulos.ManageNullStr(dr["ola_condiciones_lab"]);
                                ofertalaboral.ola_vacantes = ManejoNulos.ManageNullInteger(dr["ola_vacantes"]);
                                ofertalaboral.ola_enviar = ManejoNulos.ManegeNullBool(dr["ola_enviar"]);
                                ofertalaboral.ola_enviado = ManejoNulos.ManegeNullBool(dr["ola_enviado"]);
                                ofertalaboral.ola_publicado = ManejoNulos.ManegeNullBool(dr["ola_publicado"]);
                                ofertalaboral.ola_fecha_pub = ManejoNulos.ManageNullDate(dr["ola_fecha_pub"]);
                                ofertalaboral.ola_estado_oferta = ManejoNulos.ManageNullStr(dr["ola_estado_oferta"]);
                                ofertalaboral.ola_duracion = ManejoNulos.ManageNullInteger(dr["ola_duracion"]);
                                ofertalaboral.ola_fecha_fin = ManejoNulos.ManageNullDate(dr["ola_fecha_fin"]);
                                ofertalaboral.ola_fecha_reg = ManejoNulos.ManageNullDate(dr["ola_fecha_reg"]);
                                ofertalaboral.ola_fecha_act = ManejoNulos.ManageNullDate(dr["ola_fecha_act"]);
                                ofertalaboral.ola_estado = ManejoNulos.ManageNullStr(dr["ola_estado"]);
                                ofertalaboral.ola_cod_empresa = ManejoNulos.ManageNullStr(dr["ola_cod_empresa"]);
                                ofertalaboral.ola_cod_cargo = ManejoNulos.ManageNullStr(dr["ola_cod_cargo"]);
                                ofertalaboral.fk_ubigeo = ManejoNulos.ManageNullInteger(dr["fk_ubigeo"]);
                                ofertalaboral.fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ofertalaboral;
        }
    }
}