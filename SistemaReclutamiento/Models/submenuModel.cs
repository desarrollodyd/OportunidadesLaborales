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
    public class SubMenuModel
    {
        string _conexion;
        public SubMenuModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<SubMenuEntidad> SubMenuListarJson()
        {
            List<SubMenuEntidad> lista = new List<SubMenuEntidad>();
                string consulta = @"SELECT snu_descripcion, snu_url, snu_orden, snu_icono, snu_estado, fk_menu, snu_id,                                 snu_descripcion_eng, snu_template
	                                FROM seguridad.seg_submenu as submenu 
	                                inner join seguridad.seg_menu as menu
	                                on menu.men_id=submenu.fk_menu
	                                inner join seguridad.seg_modulo as modulo
	                                on menu.fk_modulo=modulo.mod_id
	                                where modulo.mod_tipo='Proveedor'
	                                ;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var menu = new SubMenuEntidad
                                {

                                    snu_descripcion = ManejoNulos.ManageNullStr(dr["snu_descripcion"]),
                                    snu_url = ManejoNulos.ManageNullStr(dr["snu_url"]),
                                    snu_orden = ManejoNulos.ManageNullInteger(dr["snu_orden"]),
                                    snu_icono = ManejoNulos.ManageNullStr(dr["snu_icono"]),
                                    snu_estado = ManejoNulos.ManageNullStr(dr["snu_estado"]),
                                    fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]),
                                    snu_id = ManejoNulos.ManageNullInteger(dr["snu_id"]),
                                    snu_descripcion_eng = ManejoNulos.ManageNullStr(dr["snu_descripcion_eng"]),
                                    snu_template = ManejoNulos.ManageNullStr(dr["snu_template"]),
                                };
                                lista.Add(menu);
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
        public List<SubMenuEntidad> SubMenuListarPorMenuJson(int fk_menu)
        {
            List<SubMenuEntidad> lista = new List<SubMenuEntidad>();
            string consulta = @"SELECT 
                                snu_descripcion, 
                                snu_url, 
                                snu_orden,
                                snu_icono, 
                                snu_estado,
                                fk_menu, 
                                snu_id,
                                snu_descripcion_eng,
                                snu_template
	                            FROM seguridad.seg_submenu
                                where fk_menu=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", fk_menu);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var menu = new SubMenuEntidad
                                {

                                    snu_descripcion= ManejoNulos.ManageNullStr(dr["snu_descripcion"]),
                                    snu_url = ManejoNulos.ManageNullStr(dr["snu_url"]),
                                    snu_orden = ManejoNulos.ManageNullInteger(dr["snu_orden"]),
                                    snu_icono = ManejoNulos.ManageNullStr(dr["snu_icono"]),
                                    snu_estado = ManejoNulos.ManageNullStr(dr["snu_estado"]),
                                    fk_menu = ManejoNulos.ManageNullInteger(dr["fk_menu"]),
                                    snu_id = ManejoNulos.ManageNullInteger(dr["snu_id"]),
                                    snu_descripcion_eng = ManejoNulos.ManageNullStr(dr["snu_descripcion_eng"]),
                                    snu_template = ManejoNulos.ManageNullStr(dr["snu_template"]),
                                };

                                lista.Add(menu);
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
    }
}