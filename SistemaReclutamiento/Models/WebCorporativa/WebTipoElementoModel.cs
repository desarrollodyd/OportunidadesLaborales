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
    public class WebTipoElementoModel
    {
        string _conexion;
        public WebTipoElementoModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public (List<WebTipoElementoEntidad> lista, claseError error) WebTipoElementoListarJson()
        {
            List<WebTipoElementoEntidad> lista = new List<WebTipoElementoEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT tipo_id, tipo_nombre, tipo_estado, tipo_orden
	                            FROM web_corporativa.web_tipo_elemento order by tipo_orden;";
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
                                var tipo = new WebTipoElementoEntidad
                                {

                                    tipo_id = ManejoNulos.ManageNullInteger(dr["tipo_id"]),
                                    tipo_orden = ManejoNulos.ManageNullInteger(dr["tipo_orden"]),
                                    tipo_estado = ManejoNulos.ManageNullStr(dr["tipo_estado"]),
                                    tipo_nombre = ManejoNulos.ManageNullStr(dr["tipo_nombre"]),
                                };
                                lista.Add(tipo);
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
            return (lista, error: error);
        }

    }
}