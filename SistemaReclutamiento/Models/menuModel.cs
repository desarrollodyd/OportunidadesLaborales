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
        string moduloBusqueda = "Compartido";
        public MenuModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<MenuEntidad> lista,claseError error) MenuListarJson()
        {
            List<MenuEntidad> lista = new List<MenuEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT men_descripcion, men_orden, men_icono, men_estado, men_id, men_descripcion_eng, men_tipo,                        fk_modulo
	                            FROM seguridad.seg_menu
	                            join seguridad.seg_modulo
	                            on seguridad.seg_menu.fk_modulo=seguridad.seg_modulo.mod_id
	                            where seguridad.seg_modulo.mod_tipo=@p0;";
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
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
            }
            return (lista:lista,error:error);
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
	                                FROM seguridad.seg_menu;
                                where men_id=@p0;";
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

        public bool ModuloInsertarJson(MenuEntidad menu)
        {
            bool response = false;
            string consulta = @"INSERT INTO seguridad.seg_menu(
	men_descripcion, men_orden, men_icono, men_estado, men_descripcion_eng, men_tipo, fk_modulo)
	VALUES ( @p0, @p1, @p2, @p3, @p4, @p5, @p6); ";
            try
            {
                using (var con = new NpgsqlConnection(_conexion))
                {
                    con.Open();
                    var query = new NpgsqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", menu.men_descripcion);
                    query.Parameters.AddWithValue("@p1", menu.men_orden);
                    query.Parameters.AddWithValue("@p2", menu.men_icono);
                    query.Parameters.AddWithValue("@p3", menu.men_estado);
                    query.Parameters.AddWithValue("@p4", menu.men_descripcion_eng);
                    query.Parameters.AddWithValue("@p5", menu.men_tipo);
                    query.Parameters.AddWithValue("@p6", menu.fk_modulo);
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