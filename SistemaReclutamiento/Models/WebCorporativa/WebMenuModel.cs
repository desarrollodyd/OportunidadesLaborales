using Npgsql;
using SistemaReclutamiento.Entidades.WebCorporativa;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.WebCorporativa
{
    public class WebMenuModel
    {
        string _conexion;
        public WebMenuModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<WebMenuEntidad> lista, claseError error) WebMenuListarJson()
        {
            List<WebMenuEntidad> lista = new List<WebMenuEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT menu_id, menu_titulo, menu_estado, menu_orden
	                                FROM web_corporativa.web_menu order by menu_orden;";
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
                                var Menu = new WebMenuEntidad
                                {

                                    menu_id = ManejoNulos.ManageNullInteger(dr["menu_id"]),
                                    menu_titulo = ManejoNulos.ManageNullStr(dr["menu_titulo"]),
                                    menu_estado = ManejoNulos.ManageNullStr(dr["menu_estado"]),
                                    menu_orden = ManejoNulos.ManageNullInteger(dr["menu_orden"]),
                                };
                                lista.Add(Menu);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
            }
            return (lista, error: error);
        }
    }
}