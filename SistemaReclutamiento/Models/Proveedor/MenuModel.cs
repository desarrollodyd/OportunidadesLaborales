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
    public class MenuModel
    {
        string _conexion;
        string moduloBusqueda = "Proveedor";
        public MenuModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<MenuEntidad> lista,claseError error) MenuListarJson(int fk_usuario)
        {
            List<MenuEntidad> lista = new List<MenuEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT distinct men_descripcion, men_orden, men_icono, men_estado, men_id, men_descripcion_eng, men_tipo,                        fk_modulo
	                            FROM seguridad.seg_menu
	                            join seguridad.seg_modulo
	                            on seguridad.seg_menu.fk_modulo=seguridad.seg_modulo.mod_id
								join seguridad.seg_submenu
								on seguridad.seg_submenu.fk_menu=seguridad.seg_menu.men_id
								join seguridad.seg_permiso
								on seguridad.seg_permiso.fk_submenu=seguridad.seg_submenu.snu_id
	                            where seguridad.seg_modulo.mod_tipo=@p0
								and seguridad.seg_permiso.fk_usuario=@p1 and seguridad.seg_menu.men_estado='A';";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", moduloBusqueda);
                    query.Parameters.AddWithValue("@p1", fk_usuario);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var menu = new MenuEntidad
                                {

                                    men_descripcion = ManejoNulos.ManageNullStr(dr["men_descripcion"]),
                                    men_orden = ManejoNulos.ManageNullInteger(dr["men_orden"]),
                                    men_icono = ManejoNulos.ManageNullStr(dr["men_icono"]),
                                    men_estado = ManejoNulos.ManageNullStr(dr["men_estado"]),
                                    men_id = ManejoNulos.ManageNullInteger(dr["men_id"]),
                                    men_descripcion_eng = ManejoNulos.ManageNullStr(dr["men_descripcion_eng"]),
                                    men_tipo = ManejoNulos.ManageNullStr(dr["men_tipo"]),
                                    fk_modulo = ManejoNulos.ManageNullInteger(dr["fk_modulo"]),

                                };

                                lista.Add(menu);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (lista:lista,error:error);
        }
        public List<MenuEntidad>  MenuListarporTipoJson()
        {
            List<MenuEntidad> lista = new List<MenuEntidad>();
            
            string consulta = @"SELECT 
                                men_descripcion, 
                                men_orden, 
                                men_icono, 
                                men_estado, 
                                men_id,
                                men_descripcion_eng,
                                men_tipo,                       
                                fk_modulo
	                            FROM seguridad.seg_menu join seguridad.seg_modulo
	                            on seguridad.seg_menu.fk_modulo=seguridad.seg_modulo.mod_id
	                            where seguridad.seg_modulo.mod_tipo=@p0 and seguridad.seg_menu.men_estado='A';";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", moduloBusqueda);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var menu = new MenuEntidad
                                {

                                    men_descripcion = ManejoNulos.ManageNullStr(dr["men_descripcion"]),
                                    men_orden = ManejoNulos.ManageNullInteger(dr["men_orden"]),
                                    men_icono = ManejoNulos.ManageNullStr(dr["men_icono"]),
                                    men_estado = ManejoNulos.ManageNullStr(dr["men_estado"]),
                                    men_id = ManejoNulos.ManageNullInteger(dr["men_id"]),
                                    men_descripcion_eng = ManejoNulos.ManageNullStr(dr["men_descripcion_eng"]),
                                    men_tipo = ManejoNulos.ManageNullStr(dr["men_tipo"]),
                                    fk_modulo = ManejoNulos.ManageNullInteger(dr["fk_modulo"]),

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
            return  lista;
        }
        public MenuEntidad MenuIdObtenerJson(int men_id)
        {
            MenuEntidad menu = new MenuEntidad();
            string consulta = @"SELECT 
                                men_descripcion, 
                                men_orden, 
                                men_icono,
                                men_estado,
                                men_id, 
                                men_descripcion_eng, 
                                men_tipo, 
                                fk_modulo
	                                FROM seguridad.seg_menu
                                where men_id=@p0 and seguridad.seg_menu.men_estado='A';";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", men_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                menu.men_descripcion = ManejoNulos.ManageNullStr(dr["men_descripcion"]);
                                menu.men_orden = ManejoNulos.ManageNullInteger(dr["men_orden"]);
                                menu.men_icono = ManejoNulos.ManageNullStr(dr["men_icono"]);
                                menu.men_estado = ManejoNulos.ManageNullStr(dr["men_estado"]);
                                menu.men_id = ManejoNulos.ManageNullInteger(dr["men_id"]);
                                menu.men_descripcion_eng = ManejoNulos.ManageNullStr(dr["men_descripcion_eng"]);
                                menu.men_tipo = ManejoNulos.ManageNullStr(dr["men_tipo"]);
                                menu.fk_modulo = ManejoNulos.ManageNullInteger(dr["fk_modulo"]);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return menu;
        }
        public bool MenuInsertarJson(MenuEntidad menu)
        {
            bool response = false;
            string consulta = @"INSERT INTO seguridad.seg_menu(
	                    men_descripcion, men_icono, men_estado, fk_modulo, men_id)
	                    VALUES ( @p0, @p1, @p2, @p3,@p4); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", menu.men_descripcion);
                    query.Parameters.AddWithValue("@p1", menu.men_icono);
                    query.Parameters.AddWithValue("@p2", menu.men_estado);
                    query.Parameters.AddWithValue("@p3", menu.fk_modulo);
                    query.Parameters.AddWithValue("@p4", menu.men_id);
           
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
        public int MenuObtenerUltimo()
        {
            int men_id = 0;
            string consulta = @"select men_id
                                  from seguridad.seg_menu
                                     order by men_id desc
                                     limit 1;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", men_id);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                men_id = ManejoNulos.ManageNullInteger(dr["men_id"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return men_id;
        }
        public bool MenuEliminarJson(int men_id)
        {
            bool response = false;
            string consulta = @"
                                DELETE FROM 
                                seguridad.seg_menu
                                WHERE  men_id=@p0;";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", ManejoNulos.ManageNullInteger(men_id));
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