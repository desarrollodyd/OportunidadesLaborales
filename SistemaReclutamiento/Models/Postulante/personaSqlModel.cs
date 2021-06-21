using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Models
{
    public class PersonaSqlModel
    {
        string _conexion;
        public PersonaSqlModel() {
            _conexion = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;
        }
        public  (PersonaSqlEntidad persona,claseError error) PersonaDniObtenerJson(string per_numdoc)
        {
            PersonaSqlEntidad persona = new PersonaSqlEntidad();
            claseError error = new claseError();
            //string consulta = @"Select top 1 emp.CO_TRAB, emp.NO_TRAB, emp.NO_APEL_PATE, emp.NO_APEL_MATE, emp.TI_SITU, empresa.CO_EMPR, empresa.DE_NOMB, unidad.CO_UNID, unidad.DE_UNID, 
            //            sede.CO_SEDE, sede.DE_SEDE, gerencia.CO_DEPA, gerencia.DE_DEPA, area.CO_AREA, area.DE_AREA, grupo.CO_GRUP_OCUP, grupo.DE_GRUP_OCUP, puesto.CO_PUES_TRAB, 
            //            puesto.DE_PUES_TRAB, emp.FE_INGR_CORP, emp.FE_NACI_TRAB, emp.NU_TLF1, emp.NU_TLF2, emp.NO_DIRE_MAI1    
            //            from TMTRAB_PERS as emp inner join TMTRAB_CALC as periodo on emp.CO_TRAB=periodo.CO_TRAB 
            //            inner join TMEMPR as empresa on periodo.CO_EMPR=empresa.CO_EMPR 
            //            inner join TMUNID_EMPR as unidad on unidad.CO_EMPR=empresa.CO_EMPR and unidad.CO_UNID=periodo.CO_UNID 
            //            inner join TTSEDE as sede on sede.CO_EMPR=empresa.CO_EMPR and periodo.CO_SEDE=sede.CO_SEDE 
            //            inner join TTDEPA as gerencia on gerencia.CO_EMPR=empresa.CO_EMPR and periodo.CO_DEPA=gerencia.CO_DEPA 
            //            inner join TTAREA as area on area.CO_AREA=periodo.CO_AREA and area.CO_EMPR=periodo.CO_EMPR and periodo.CO_DEPA=area.CO_DEPA 
            //            inner join TTGRUP_OCUP as grupo on grupo.CO_EMPR=empresa.CO_EMPR and grupo.CO_GRUP_OCUP=periodo.CO_GRUP_OCUP 
            //            inner join TTPUES_TRAB as puesto on puesto.CO_EMPR=empresa.CO_EMPR and puesto.CO_PUES_TRAB=periodo.CO_PUES_TRAB 
            //            where emp.CO_TRAB=@p0
            //            order by emp.FE_INGR_CORP desc";
            string consulta = @"Select 
                                CO_TRAB,
                                NO_APEL_PATE,   
                                NO_APEL_MATE,
                                NO_TRAB ,
                                NO_DIRE_MAI1
                                from 
                                TMTRAB_PERS where CO_TRAB=@p0";
            try
            {
                using (var con = new SqlConnection(_conexion))
                {
                    con.Open();
                    var query = new SqlCommand(consulta, con);
                    query.Parameters.AddWithValue("@p0", per_numdoc);
                    using (var dr = query.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {                             
                                persona.CO_TRAB = ManejoNulos.ManageNullStr(dr["CO_TRAB"]);
                                persona.NO_TRAB = ManejoNulos.ManageNullStr(dr["NO_TRAB"]);
                                persona.NO_APEL_PATE = ManejoNulos.ManageNullStr(dr["NO_APEL_PATE"]);
                                persona.NO_APEL_MATE = ManejoNulos.ManageNullStr(dr["NO_APEL_MATE"]);
                                //persona.TI_SITU = ManejoNulos.ManageNullStr(dr["TI_SITU"]);
                                //persona.CO_EMPR = ManejoNulos.ManageNullStr(dr["CO_EMPR"]);
                                //persona.DE_NOMB = ManejoNulos.ManageNullStr(dr["DE_NOMB"]);
                                //persona.CO_UNID = ManejoNulos.ManageNullStr(dr["CO_UNID"]);
                                //persona.DE_UNID = ManejoNulos.ManageNullStr(dr["DE_UNID"]);
                                //persona.CO_SEDE = ManejoNulos.ManageNullStr(dr["CO_SEDE"]);
                                //persona.DE_SEDE = ManejoNulos.ManageNullStr(dr["DE_SEDE"]);
                                //persona.CO_DEPA = ManejoNulos.ManageNullStr(dr["CO_DEPA"]);
                                //persona.DE_DEPA = ManejoNulos.ManageNullStr(dr["DE_DEPA"]);
                                //persona.CO_AREA = ManejoNulos.ManageNullStr(dr["CO_AREA"]);
                                //persona.DE_AREA = ManejoNulos.ManageNullStr(dr["DE_AREA"]);
                                //persona.CO_GRUP_OCUP = ManejoNulos.ManageNullStr(dr["CO_GRUP_OCUP"]);
                                //persona.DE_GRUP_OCUP = ManejoNulos.ManageNullStr(dr["DE_GRUP_OCUP"]);
                                //persona.CO_PUES_TRAB = ManejoNulos.ManageNullStr(dr["CO_PUES_TRAB"]);
                                //persona.DE_PUES_TRAB = ManejoNulos.ManageNullStr(dr["DE_PUES_TRAB"]);
                                //persona.FE_INGR_CORP = Convert.ToDateTime(Convert.IsDBNull(dr["FE_INGR_CORP"]) ? DateTime.Now : dr["FE_INGR_CORP"]);
                                //persona.FE_NACI_TRAB = Convert.ToDateTime(Convert.IsDBNull(dr["FE_NACI_TRAB"]) ? DateTime.Now : dr["FE_NACI_TRAB"]);
                                //persona.NU_TLF1 = ManejoNulos.ManageNullStr(dr["NU_TLF1"]);
                                //persona.NU_TLF2 = ManejoNulos.ManageNullStr(dr["NU_TLF2"]);
                                persona.NO_DIRE_MAI1 = ManejoNulos.ManageNullStr(dr["NO_DIRE_MAI1"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error.Respuesta = false;
                error.Mensaje = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return (persona:persona,error:error);
        }
    }
}