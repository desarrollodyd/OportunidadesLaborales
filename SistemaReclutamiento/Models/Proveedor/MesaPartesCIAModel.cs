using Npgsql;
using SistemaReclutamiento.Entidades.Proveedor;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models.Proveedor
{
    public class MesaPartesCIAModel
    {
        string _conexion;
        public MesaPartesCIAModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<MesaPartesCIAEntidad> MesaPartesListarCompanias()
        {
            List<MesaPartesCIAEntidad> lista = new List<MesaPartesCIAEntidad>();
            string consulta = @"SELECT cia_id, cia_codigo, cia_nombre, cia_estado
	                            FROM mesa_partes.mes_cia
	                            where cia_estado='A';";
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
                                var CIA = new MesaPartesCIAEntidad
                                {

                                    cia_id = ManejoNulos.ManageNullInteger(dr["cia_id"]),
                                    cia_codigo = ManejoNulos.ManageNullStr(dr["cia_codigo"]),
                                    cia_nombre = ManejoNulos.ManageNullStr(dr["cia_nombre"]),
                                    cia_estado = ManejoNulos.ManageNullStr(dr["cia_estado"]),
                                    
                                };
                                lista.Add(CIA);
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