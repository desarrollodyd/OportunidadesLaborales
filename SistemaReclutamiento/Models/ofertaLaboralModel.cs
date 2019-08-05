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
            //if (ola_fecha_pub != "")
            //{
            //    switch (ola_fecha_pub) {
            //        case "hoy": consulta += "ola_fecha_pub between" +ManejoNulos.ManageNullDate(DateTime.Now ManejoNulos.ManageNullDate(DateTime.Now) + "and";
            //            break;
            //    }

            //}
            if (filtros.ubi_distrito_id != 0)
            {
                consulta += "fk_ubigeo=" + ManejoNulos.ManageNullInteger(filtros.ubi_distrito_id) + "and";
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
    }
}