using SistemaReclutamiento.Entidades.Proveedor;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class SQLModel
    {
        string _conexion;
        string _conexion_concar;
        public SQLModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;
            _conexion_concar = ConfigurationManager.ConnectionStrings["conexionSQLCONCAR"].ConnectionString;
        }
        public (List<TMEMPR> listaempresa, claseError error) EmpresaListarJson()
        {
            List<TMEMPR> lista = new List<TMEMPR>();
            claseError error = new claseError();
            string consulta = @"Select 
                                CO_EMPR,
                                DE_NOMB,
                                DE_NOMB_CORT
                                from 
                                TMEMPR";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    //query.Parameters.AddWithValue("@p0", per_numdoc);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var empresa = new TMEMPR()
                                {
                                    CO_EMPR = ManejoNulos.ManageNullStr(dr["CO_EMPR"]),
                                    DE_NOMB = ManejoNulos.ManageNullStr(dr["DE_NOMB"]),
                                    DE_NOMB_CORT = ManejoNulos.ManageNullStr(dr["DE_NOMB_CORT"]),
                                };
                                lista.Add(empresa);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return (listaempresa: lista, error: error);
        }
        public (List<TTPUES_TRAB> listapuesto, claseError error) PuestoTrabajoObtenerPorEmpresaJson(string CO_EMPR)
        {
            List<TTPUES_TRAB> lista = new List<TTPUES_TRAB>();
            claseError error = new claseError();
            string consulta = @"Select 
                                CO_EMPR,
                                CO_PUES_TRAB,
                                DE_PUES_TRAB
                                from 
                                TTPUES_TRAB where CO_EMPR=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", CO_EMPR);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var puesto = new TTPUES_TRAB()
                                {
                                    CO_EMPR = ManejoNulos.ManageNullStr(dr["CO_EMPR"]),
                                    CO_PUES_TRAB = ManejoNulos.ManageNullStr(dr["CO_PUES_TRAB"]),
                                    DE_PUES_TRAB = ManejoNulos.ManageNullStr(dr["DE_PUES_TRAB"]),
                                };
                                lista.Add(puesto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Key = ex.Data.Count.ToString();
                error.Value = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return (listapuesto: lista, error: error);
        }
        public (List<CPCARTEntidad> lista, claseError error,string cadena) CPCARTListarPagosPorCompania(string nombre_tabla,string cod_proveedor, string tipo_doc, string fecha_inicio, string fecha_final)
        {
            
            claseError error = new claseError();
            List<CPCARTEntidad> lista = new List<CPCARTEntidad>();
            string cadena= "";
            string consulta = @"SELECT [CP_CVANEXO],[CP_CCODIGO],[CP_CTIPDOC],[CP_CNUMDOC],[CP_CFECDOC],[CP_CFECVEN],[CP_CFECREC],[CP_CSITUAC],[CP_CFECCOM],[CP_CSUBDIA],[CP_CCOMPRO],[CP_CDEBHAB],[CP_CCODMON],[CP_NTIPCAM],[CP_NIMPOMN],[CP_NIMPOUS],[CP_NSALDMN],[CP_NSALDUS],[CP_NIGVMN],[CP_NIGVUS],[CP_NIMP2MN],[CP_NIMP2US],[CP_NIMPAJU],[CP_CCUENTA],[CP_CAREA],[CP_CFECUBI],[CP_CTDOCRE],[CP_CNDOCRE],[CP_CFDOCRE],[CP_CTDOCCO],[CP_DFECDOC],[CP_DFECCOM] FROM [" + nombre_tabla+ "] where CP_CCODIGO=@p1 and CP_CTIPDOC=@p2 and CP_DFECDOC between @p3 and @p4 ";

            cadena+=consulta+ "-"+ nombre_tabla+"-"+ cod_proveedor+"-"+tipo_doc+"-"+ fecha_inicio+"-"+fecha_final;
            try
            {
                using (var con = new SqlConnection(_conexion_concar))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
          
                    query.Parameters.AddWithValue("@p1", cod_proveedor.Trim());
                    query.Parameters.AddWithValue("@p2", tipo_doc.Trim());
                    query.Parameters.AddWithValue("@p3", ManejoNulos.ManageNullDate(fecha_inicio));
                    query.Parameters.AddWithValue("@p4", ManejoNulos.ManageNullDate(fecha_final));
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var cartera = new CPCARTEntidad
                                {
                                    CP_CVANEXO = ManejoNulos.ManageNullStr(dr["CP_CVANEXO"]),
                                    CP_CCODIGO = ManejoNulos.ManageNullStr(dr["CP_CCODIGO"]),
                                    CP_CTIPDOC = ManejoNulos.ManageNullStr(dr["CP_CTIPDOC"]),
                                    CP_CNUMDOC = ManejoNulos.ManageNullStr(dr["CP_CNUMDOC"]),
                                    CP_CFECDOC = ManejoNulos.ManageNullStr(dr["CP_CFECDOC"]),
                                    CP_CFECVEN = ManejoNulos.ManageNullStr(dr["CP_CFECVEN"]),
                                    CP_CFECREC = ManejoNulos.ManageNullStr(dr["CP_CFECREC"]),
                                    CP_CSITUAC = ManejoNulos.ManageNullStr(dr["CP_CSITUAC"]),
                                    CP_CFECCOM = ManejoNulos.ManageNullStr(dr["CP_CFECCOM"]),
                                    CP_CSUBDIA = ManejoNulos.ManageNullStr(dr["CP_CSUBDIA"]),
                                    CP_CCOMPRO = ManejoNulos.ManageNullStr(dr["CP_CCOMPRO"]),
                                    CP_CDEBHAB = ManejoNulos.ManageNullStr(dr["CP_CDEBHAB"]),
                                    CP_CCODMON = ManejoNulos.ManageNullStr(dr["CP_CCODMON"]),
                                    CP_NTIPCAM = ManejoNulos.ManageNullDecimal(dr["CP_NTIPCAM"]),
                                    CP_NIMPOMN = ManejoNulos.ManageNullDecimal(dr["CP_NIMPOMN"]),
                                    CP_NIMPOUS = ManejoNulos.ManageNullDecimal(dr["CP_NIMPOUS"]),
                                    CP_NSALDMN = ManejoNulos.ManageNullDecimal(dr["CP_NSALDMN"]),
                                    CP_NSALDUS = ManejoNulos.ManageNullDecimal(dr["CP_NSALDUS"]),
                                    CP_NIGVMN = ManejoNulos.ManageNullDecimal(dr["CP_NIGVMN"]),
                                    CP_NIGVUS = ManejoNulos.ManageNullDecimal(dr["CP_NIGVUS"]),
                                    CP_NIMP2MN = ManejoNulos.ManageNullDecimal(dr["CP_NIMP2MN"]),
                                    CP_NIMP2US = ManejoNulos.ManageNullDecimal(dr["CP_NIMP2US"]),
                                    CP_NIMPAJU = ManejoNulos.ManageNullDecimal(dr["CP_NIMPAJU"]),
                                    CP_CCUENTA = ManejoNulos.ManageNullStr(dr["CP_CCUENTA"]),
                                    CP_CAREA = ManejoNulos.ManageNullStr(dr["CP_CAREA"]),
                                    CP_CFECUBI = ManejoNulos.ManageNullStr(dr["CP_CFECUBI"]),
                                    CP_CTDOCRE = ManejoNulos.ManageNullStr(dr["CP_CTDOCRE"]),
                                    CP_CNDOCRE = ManejoNulos.ManageNullStr(dr["CP_CNDOCRE"]),
                                    CP_CFDOCRE = ManejoNulos.ManageNullStr(dr["CP_CFDOCRE"]),
                                    CP_CTDOCCO = ManejoNulos.ManageNullStr(dr["CP_CTDOCCO"]),
                                    CP_DFECDOC=ManejoNulos.ManageNullDate(dr["CP_DFECDOC"]),
                                    CP_DFECCOM=ManejoNulos.ManageNullDate(dr["CP_DFECCOM"])
                                };

                                lista.Add(cartera);
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

            return (lista,error, cadena);
        }

        public (List<CPPAGOEntidad> lista, claseError error) CPPAGOListarPagosPorNumeroDocumento(string nombre_tabla, string cod_proveedor, string tipo_doc, string num_doc)
        {
            List<CPPAGOEntidad> lista = new List<CPPAGOEntidad>();
            claseError error = new claseError();
            string consulta = @"SELECT [PG_CVANEXO]
                                      ,[PG_CCODIGO]
                                      ,[PG_CTIPDOC]
                                      ,[PG_CNUMDOC]
                                      ,[PG_CORDKEY]
                                      ,[PG_CDEBHAB]
                                      ,[PG_NIMPOMN]
                                      ,[PG_NIMPOUS]
                                      ,[PG_CFECCOM]
                                      ,[PG_CSUBDIA]
                                      ,[PG_CNUMCOM]
                                      ,[PG_CGLOSA]
                                      ,[PG_CCOGAST]
                                      ,[PG_CORIGEN]
                                      ,[PG_CUSUARI]
                                      ,[PG_CCODMON]
                                      ,[PG_DFECCOM]
                          FROM [dbo].[" + nombre_tabla + "] where PG_CCODIGO=@p1 and PG_CTIPDOC=@p2 and PG_CNUMDOC = @p3 ";
            try
            {
                using (var con = new SqlConnection(_conexion_concar))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);

                    query.Parameters.AddWithValue("@p1", cod_proveedor);
                    query.Parameters.AddWithValue("@p2", tipo_doc);
                    query.Parameters.AddWithValue("@p3", num_doc);
           
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var cartera = new CPPAGOEntidad
                                {
                                    PG_CVANEXO= ManejoNulos.ManageNullStr(dr["PG_CVANEXO"]),
                                    PG_CCODIGO = ManejoNulos.ManageNullStr(dr["PG_CCODIGO"]),
                                    PG_CTIPDOC = ManejoNulos.ManageNullStr(dr["PG_CTIPDOC"]),
                                    PG_CNUMDOC = ManejoNulos.ManageNullStr(dr["PG_CNUMDOC"]),
                                    PG_CORDKEY = ManejoNulos.ManageNullStr(dr["PG_CORDKEY"]),
                                    PG_CDEBHAB = ManejoNulos.ManageNullStr(dr["PG_CDEBHAB"]),
                                    PG_NIMPOMN = ManejoNulos.ManageNullDecimal(dr["PG_NIMPOMN"]),
                                    PG_NIMPOUS = ManejoNulos.ManageNullDecimal(dr["PG_NIMPOUS"]),
                                    PG_CFECCOM = ManejoNulos.ManageNullStr(dr["PG_CFECCOM"]),
                                    PG_CSUBDIA = ManejoNulos.ManageNullStr(dr["PG_CSUBDIA"]),
                                    PG_CNUMCOM = ManejoNulos.ManageNullStr(dr["PG_CNUMCOM"]),
                                    PG_CGLOSA = ManejoNulos.ManageNullStr(dr["PG_CGLOSA"]),
                                    PG_CCOGAST = ManejoNulos.ManageNullStr(dr["PG_CCOGAST"]),
                                    PG_CORIGEN = ManejoNulos.ManageNullStr(dr["PG_CORIGEN"]),
                                    PG_CUSUARI = ManejoNulos.ManageNullStr(dr["PG_CUSUARI"]),
                                    PG_CCODMON = ManejoNulos.ManageNullStr(dr["PG_CCODMON"]),
                                    PG_DFECCOM = ManejoNulos.ManageNullDate(dr["PG_DFECCOM"]),
                                };

                                lista.Add(cartera);
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

            return (lista,error);
        }
        public (decimal subtotalSoles,decimal subtotalDolares, claseError error) ObtenerSubtotalporNumeroDocumento(string nombre_tabla, string num_doc, string tipo_doc,string cod_proveedor)
        {
            claseError error = new claseError();
            decimal subtotalSoles=0;
            decimal subtotalDolares = 0;
            string consulta = @"select sum(PG_NIMPOMN) AS subtotalSoles, sum(PG_NIMPOUS) as subtotalDolares  FROM " + nombre_tabla + " WHERE PG_CNUMDOC=@p1 and PG_CTIPDOC=@p2 and PG_CCODIGO=@p3 ;";
            try
            {
                using (var con = new SqlConnection(_conexion_concar))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p1", num_doc);
                    query.Parameters.AddWithValue("@p2", tipo_doc);
                    query.Parameters.AddWithValue("@p3", cod_proveedor);

                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    subtotalSoles= ManejoNulos.ManageNullDecimal(dr["subtotalSoles"]);
                                    subtotalDolares = ManejoNulos.ManageNullDecimal(dr["subtotalDolares"]);
                                }
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

            return (subtotalSoles,subtotalDolares, error);
        }
    }
}