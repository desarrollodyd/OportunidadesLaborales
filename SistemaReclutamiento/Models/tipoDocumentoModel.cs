using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Models
{
    public class tipoDocumentoModel
    {
        string _conexion;
        public tipoDocumentoModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }
        public List<tipoDocumentoEntidad> tipoDocumentoListarJson()
        {
            List<tipoDocumentoEntidad> listaTipoDocumento = new List<tipoDocumentoEntidad>();
            string consulta = @"SELECT [tipoDocumentoId]
                              ,[tipoDocumentoDescripcion]
                          FROM [dbo].[TipoDocumento]";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    using (var dr = query.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var tipoDocumento = new tipoDocumentoEntidad
                            {
                                tipoDocumentoId = ManejoNulos.ManageNullInteger(dr["tipoDocumentoId"]),
                                tipoDocumentoDescripcion = ManejoNulos.ManageNullStr(dr["tipoDocumentoDescripcion"])
                                
                            };
                            listaTipoDocumento.Add(tipoDocumento);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("" + ex.Message + this.GetType().FullName + " " + DateTime.Now.ToLongDateString());
            }
            return listaTipoDocumento;
        }
    }
}