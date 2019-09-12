using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class SQLModel
    {
        string _conexion;
        public SQLModel()
        {
            _conexion = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;
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
    }
}