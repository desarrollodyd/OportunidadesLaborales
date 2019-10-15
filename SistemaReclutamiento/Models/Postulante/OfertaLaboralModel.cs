using Npgsql;
using SistemaReclutamiento.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SistemaReclutamiento.Utilitarios;
using System.Diagnostics;

namespace SistemaReclutamiento.Models
{
    public class OfertaLaboralModel
    {
        string _conexion;
        public OfertaLaboralModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public int ObtenerCantidadOfertas(int pos_id) {
            int cantidad = 0;
            string consulta = @"select Count(*) as total from gestion_talento.gdt_ola_oferta_laboral where ola_estado='A' and ola_publicado='true' and ola_estado_oferta='ACTIVO' ola_id not in
                                (select fk_oferta_laboral
                                    FROM gestion_talento.gdt_ola_oferta_laboral as oferta_laboral
                                    INNER JOIN gestion_talento.gdt_pos_postulacion as postulacion
                                    on oferta_laboral.ola_id = postulacion.fk_oferta_laboral
                                    where postulacion.fk_postulante = " + pos_id + ")";
            try
            {
                using (var con = new NpgsqlConnection())
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cantidad = ManejoNulos.ManageNullInteger(dr["total"]);
                        }
                    }
                }
            }
            catch (Exception  ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cantidad;
        }
        public (List<OfertaLaboralEntidad> lista,claseError error) OfertaLaboralListarJson(ReporteOfertaLaboral filtros)
        {
            claseError error = new claseError();
            List<OfertaLaboralEntidad> lista = new List<OfertaLaboralEntidad>();
            
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
                                ola_cod_unidad,
                                ola_cod_sede, 
                                ola_cod_gerencia,
                                ola_cod_area,
                                ola_cod_puesto, 
                                fk_ubigeo, 
                                fk_usuario
	                            FROM gestion_talento.gdt_ola_oferta_laboral join marketing.cpj_ubigeo
                                on (fk_ubigeo=marketing.cpj_ubigeo.ubi_id)
                                where ";

            if (filtros.busqueda.Equals("PAIS")) {
                consulta+= "(ubi_pais_id in (select ubi_pais_id from marketing.cpj_ubigeo where ubi_pais_id = '"+filtros.ubi_pais_id+"' and ubi_departamento_id = '0'and ubi_provincia_id = '0' and ubi_distrito_id = '0')) and ";
            }
            if (filtros.busqueda.Equals("DEPARTAMENTO"))
            {
                consulta += "(ubi_departamento_id in (select ubi_departamento_id from marketing.cpj_ubigeo where ubi_pais_id = '" + filtros.ubi_pais_id + "' and ubi_departamento_id = '"+filtros.ubi_departamento_id+"'and ubi_provincia_id = '0' and ubi_distrito_id = '0')) and ";
            }
            if (filtros.busqueda.Equals("PROVINCIA"))
            {
                consulta += "(ubi_provincia_id in (select ubi_provincia_id from marketing.cpj_ubigeo where ubi_pais_id = '" + filtros.ubi_pais_id + "' and ubi_departamento_id = '" + filtros.ubi_departamento_id + "'and ubi_provincia_id = '"+filtros.ubi_provincia_id+"' and ubi_distrito_id = '0')) and ";
            }
            if (filtros.busqueda.Equals("DISTRITO"))
            {
                consulta += "(ubi_distrito_id in (select ubi_distrito_id from marketing.cpj_ubigeo where ubi_pais_id = '" + filtros.ubi_pais_id + "' and ubi_departamento_id = '" + filtros.ubi_departamento_id + "'and ubi_provincia_id = '" + filtros.ubi_provincia_id + "' and ubi_distrito_id = '"+filtros.ubi_distrito_id+"')) and ";
            }
            if (filtros.ola_cod_empresa != "" && filtros.ola_cod_empresa != null)
            {
                consulta += "ola_cod_empresa='"+ManejoNulos.ManageNullStr(filtros.ola_cod_empresa) +"' and ";
            }
            if (filtros.ola_cod_cargo != "" && filtros.ola_cod_cargo != null)
            {
                consulta += "ola_cod_puesto='" + ManejoNulos.ManageNullStr(filtros.ola_cod_cargo) + "' and ";
            }
            if (filtros.ola_fecha_ini!=null)
            {
                consulta += "ola_fecha_pub between '" + ManejoNulos.ManageNullDate(filtros.ola_fecha_ini).ToString("yyyy-MM-dd HH':'mm':'ss") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss") + "' and ";
            }
            if (filtros.ola_nombre != "" && filtros.ola_nombre != null)
            {
                consulta += "lower(ola_nombre) Like '%" + ManejoNulos.ManageNullStr(filtros.ola_nombre.ToLower()) + "%' and ";
            }

            consulta += "ola_estado='A' and ola_publicado='true' and ola_estado_oferta='ACTIVO' and  ";
            consulta += @" ola_id not in
                                (select fk_oferta_laboral
                                    FROM gestion_talento.gdt_ola_oferta_laboral as oferta_laboral
                                    INNER JOIN gestion_talento.gdt_pos_postulacion as postulacion
                                    on oferta_laboral.ola_id = postulacion.fk_oferta_laboral
                                    where postulacion.fk_postulante = "+filtros.pos_id+")";
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
                            var oferta = new OfertaLaboralEntidad
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
                                ola_cod_unidad = ManejoNulos.ManageNullStr(dr["ola_cod_unidad"]),
                                ola_cod_sede=ManejoNulos.ManageNullStr(dr["ola_cod_sede"]),
                                ola_cod_gerencia=ManejoNulos.ManageNullStr(dr["ola_cod_gerencia"]),
                                ola_cod_area=ManejoNulos.ManageNullStr(dr["ola_cod_area"]),
                                ola_cod_puesto=ManejoNulos.ManageNullStr(dr["ola_cod_puesto"]),
                                fk_ubigeo =ManejoNulos.ManageNullInteger(dr["fk_ubigeo"]),
                                fk_usuario=ManejoNulos.ManageNullInteger(dr["fk_usuario"])
                            };
                            lista.Add(oferta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
                //Console.Write(ex.Message);
            }
            return (lista:lista,error:error);
        }
        public (List<OfertaLaboralEntidad> lista,claseError error) PostulanteListarMisOfertasPostuladasJson(ReporteOfertaLaboral filtros)
        {
            claseError error = new claseError();
            List<OfertaLaboralEntidad> lista = new List<OfertaLaboralEntidad>();
            string consulta = @"SELECT 
                                distinct
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
                                ola_cod_unidad,
                                ola_cod_sede, 
                                ola_cod_gerencia,
                                ola_cod_area,
                                ola_cod_puesto, 
                                fk_ubigeo, 
                                fk_usuario
	                            FROM gestion_talento.gdt_ola_oferta_laboral as oferta_laboral
	                            INNER JOIN gestion_talento.gdt_pos_postulacion as postulacion
	                            on oferta_laboral.ola_id=postulacion.fk_oferta_laboral
                                join marketing.cpj_ubigeo
                                on (fk_ubigeo=marketing.cpj_ubigeo.ubi_id)
	                            where  ";
            if (filtros.busqueda.Equals("PAIS"))
            {
                consulta += "(ubi_pais_id in (select ubi_pais_id from marketing.cpj_ubigeo where ubi_pais_id = '" + filtros.ubi_pais_id + "' and ubi_departamento_id = '0'and ubi_provincia_id = '0' and ubi_distrito_id = '0')) and ";
            }
            if (filtros.busqueda.Equals("DEPARTAMENTO"))
            {
                consulta += "(ubi_departamento_id in (select ubi_departamento_id from marketing.cpj_ubigeo where ubi_pais_id = '" + filtros.ubi_pais_id + "' and ubi_departamento_id = '" + filtros.ubi_departamento_id + "'and ubi_provincia_id = '0' and ubi_distrito_id = '0')) and ";
            }
            if (filtros.busqueda.Equals("PROVINCIA"))
            {
                consulta += "(ubi_provincia_id in (select ubi_provincia_id from marketing.cpj_ubigeo where ubi_pais_id = '" + filtros.ubi_pais_id + "' and ubi_departamento_id = '" + filtros.ubi_departamento_id + "'and ubi_provincia_id = '" + filtros.ubi_provincia_id + "' and ubi_distrito_id = '0')) and ";
            }
            if (filtros.busqueda.Equals("DISTRITO"))
            {
                consulta += "(ubi_distrito_id in (select ubi_distrito_id from marketing.cpj_ubigeo where ubi_pais_id = '" + filtros.ubi_pais_id + "' and ubi_departamento_id = '" + filtros.ubi_departamento_id + "'and ubi_provincia_id = '" + filtros.ubi_provincia_id + "' and ubi_distrito_id = '" + filtros.ubi_distrito_id + "')) and ";
            }
            if (filtros.ola_cod_empresa != "" && filtros.ola_cod_empresa != null)
            {
                consulta += "ola_cod_empresa='" + ManejoNulos.ManageNullStr(filtros.ola_cod_empresa) + "' and ";
            }
            if (filtros.ola_cod_cargo != "" && filtros.ola_cod_cargo != null)
            {
                consulta += "ola_cod_puesto='" + ManejoNulos.ManageNullStr(filtros.ola_cod_cargo) + "' and ";
            }
            if (filtros.ola_fecha_ini != null)
            {
                consulta += "ola_fecha_pub between '" + ManejoNulos.ManageNullDate(filtros.ola_fecha_ini).ToString("yyyy-MM-dd HH':'mm':'ss") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss") + "' and ";
            }
            if (filtros.ola_nombre != "" && filtros.ola_nombre != null)
            {
                consulta += "lower(ola_nombre) Like '%" + ManejoNulos.ManageNullStr(filtros.ola_nombre.ToLower()) + "%' and ";
            }
            consulta += " postulacion.fk_postulante=@p0 and ola_estado='A';";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", filtros.pos_id);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var oferta = new OfertaLaboralEntidad
                            {
                                ola_id = ManejoNulos.ManageNullInteger(dr["ola_id"]),
                                ola_nombre = ManejoNulos.ManageNullStr(dr["ola_nombre"]),
                                ola_requisitos = ManejoNulos.ManageNullStr(dr["ola_requisitos"]),
                                ola_funciones = ManejoNulos.ManageNullStr(dr["ola_funciones"]),
                                ola_competencias = ManejoNulos.ManageNullStr(dr["ola_competencias"]),
                                ola_condiciones_lab = ManejoNulos.ManageNullStr(dr["ola_condiciones_lab"]),
                                ola_vacantes = ManejoNulos.ManageNullInteger(dr["ola_vacantes"]),
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
                                ola_cod_unidad = ManejoNulos.ManageNullStr(dr["ola_cod_unidad"]),
                                ola_cod_sede = ManejoNulos.ManageNullStr(dr["ola_cod_sede"]),
                                ola_cod_gerencia = ManejoNulos.ManageNullStr(dr["ola_cod_gerencia"]),
                                ola_cod_area = ManejoNulos.ManageNullStr(dr["ola_cod_area"]),
                                ola_cod_puesto = ManejoNulos.ManageNullStr(dr["ola_cod_puesto"]),
                                fk_ubigeo = ManejoNulos.ManageNullInteger(dr["fk_ubigeo"]),
                                fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"])
                            };
                            lista.Add(oferta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
               // Console.Write(ex.Message);
            }
            return (lista:lista,error:error);

        }
        public List<OfertaLaboralEntidad> PostulanteListarPostulacionesJson(int pos_id)
        {
            List<OfertaLaboralEntidad> postulaciones = new List<OfertaLaboralEntidad>();
            string consulta = @"SELECT 
                                distinct
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
                                ola_cod_unidad,
                                ola_cod_sede, 
                                ola_cod_gerencia,
                                ola_cod_area,
                                ola_cod_puesto, 
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
                                var postulacion = new OfertaLaboralEntidad
                                {
                                    ola_id = ManejoNulos.ManageNullInteger(dr["ola_id"]),
                                    ola_nombre = ManejoNulos.ManageNullStr(dr["ola_nombre"]),
                                    ola_requisitos = ManejoNulos.ManageNullStr(dr["ola_requisitos"]),
                                    ola_funciones = ManejoNulos.ManageNullStr(dr["ola_funciones"]),
                                    ola_competencias = ManejoNulos.ManageNullStr(dr["ola_competencias"]),
                                    ola_condiciones_lab = ManejoNulos.ManageNullStr(dr["ola_condiciones_lab"]),
                                    ola_vacantes = ManejoNulos.ManageNullInteger(dr["ola_vacantes"]),
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
                                    ola_cod_unidad = ManejoNulos.ManageNullStr(dr["ola_cod_unidad"]),
                                    ola_cod_sede = ManejoNulos.ManageNullStr(dr["ola_cod_sede"]),
                                    ola_cod_gerencia = ManejoNulos.ManageNullStr(dr["ola_cod_gerencia"]),
                                    ola_cod_area = ManejoNulos.ManageNullStr(dr["ola_cod_area"]),
                                    ola_cod_puesto = ManejoNulos.ManageNullStr(dr["ola_cod_puesto"]),
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
        public (OfertaLaboralEntidad ofertalaboral, claseError error) OfertaLaboralIdObtenerJson(int ola_id)
        {
            claseError error = new claseError();
            OfertaLaboralEntidad ofertalaboral = new OfertaLaboralEntidad();
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
                                    ola_cod_unidad,
                                    ola_cod_sede, 
                                    ola_cod_gerencia,
                                    ola_cod_area,
                                    ola_cod_puesto, 
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
                                ofertalaboral.ola_cod_unidad = ManejoNulos.ManageNullStr(dr["ola_cod_unidad"]);
                                ofertalaboral.ola_cod_sede = ManejoNulos.ManageNullStr(dr["ola_cod_sede"]);
                                ofertalaboral.ola_cod_gerencia = ManejoNulos.ManageNullStr(dr["ola_cod_gerencia"]);
                                ofertalaboral.ola_cod_area = ManejoNulos.ManageNullStr(dr["ola_cod_area"]);
                                ofertalaboral.ola_cod_puesto = ManejoNulos.ManageNullStr(dr["ola_cod_puesto"]);
                                ofertalaboral.fk_ubigeo = ManejoNulos.ManageNullInteger(dr["fk_ubigeo"]);
                                ofertalaboral.fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
                //Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return (ofertalaboral:ofertalaboral,error:error);
        }
        public (List<OfertaLaboralEntidad> lista, claseError error ) OfertaLaboralListarVistaIndexJson(int pos_id)
        {
            claseError error = new claseError();
            List<OfertaLaboralEntidad> lista = new List<OfertaLaboralEntidad>();
            var fechaHoy = DateTime.Now;
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
                                ola_cod_unidad,
                                ola_cod_sede, 
                                ola_cod_gerencia,
                                ola_cod_area,
                                ola_cod_puesto, 
                                fk_ubigeo, 
                                fk_usuario
	                            FROM gestion_talento.gdt_ola_oferta_laboral 
                                join marketing.cpj_ubigeo
                                on fk_ubigeo=marketing.cpj_ubigeo.ubi_id where ";

            consulta += "ola_estado='A' and ola_publicado='true' and ola_estado_oferta='ACTIVO' and ola_fecha_pub<='" + fechaHoy.ToString("yyyy-MM-dd HH':'mm':'ss") + "' and ola_fecha_fin>='" + fechaHoy.ToString("yyyy-MM-dd HH':'mm':'ss") + "' and ";
            consulta += @" ola_id not in
                                (select fk_oferta_laboral
                                    FROM gestion_talento.gdt_ola_oferta_laboral as oferta_laboral
                                    INNER JOIN gestion_talento.gdt_pos_postulacion as postulacion
                                    on oferta_laboral.ola_id = postulacion.fk_oferta_laboral
                                    where postulacion.fk_postulante = " + pos_id + ")";
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
                            var oferta = new OfertaLaboralEntidad
                            {
                                ola_id = ManejoNulos.ManageNullInteger(dr["ola_id"]),
                                ola_nombre = ManejoNulos.ManageNullStr(dr["ola_nombre"]),
                                ola_requisitos = ManejoNulos.ManageNullStr(dr["ola_requisitos"]),
                                ola_funciones = ManejoNulos.ManageNullStr(dr["ola_funciones"]),
                                ola_competencias = ManejoNulos.ManageNullStr(dr["ola_competencias"]),
                                ola_condiciones_lab = ManejoNulos.ManageNullStr(dr["ola_condiciones_lab"]),
                                ola_vacantes = ManejoNulos.ManageNullInteger(dr["ola_vacantes"]),
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
                                ola_cod_unidad = ManejoNulos.ManageNullStr(dr["ola_cod_unidad"]),
                                ola_cod_sede = ManejoNulos.ManageNullStr(dr["ola_cod_sede"]),
                                ola_cod_gerencia = ManejoNulos.ManageNullStr(dr["ola_cod_gerencia"]),
                                ola_cod_area = ManejoNulos.ManageNullStr(dr["ola_cod_area"]),
                                ola_cod_puesto = ManejoNulos.ManageNullStr(dr["ola_cod_puesto"]),
                                fk_ubigeo = ManejoNulos.ManageNullInteger(dr["fk_ubigeo"]),
                                fk_usuario = ManejoNulos.ManageNullInteger(dr["fk_usuario"])
                            };
                            lista.Add(oferta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
                //Console.Write(ex.Message);
            }
            return (lista:lista, error:error);
        }
    }
}