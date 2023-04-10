using Microsoft.Win32;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SistemaReclutamiento.Controllers
{
    [autorizacion(false)]
    public class OfisisController : Controller
    {
        SQLModel sqlbl = new SQLModel();
        [HttpGet]
        public ActionResult PersonaPorDniFechaCese(string dni="")
        {
            PersonaSqlEntidad personaSQL = new PersonaSqlEntidad();
            try
            {
                personaSQL = sqlbl.PersonaPorDniFechaCese(dni);

            }
            catch (Exception ex)
            {
            }

            return Json(new {data=personaSQL }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[HttpGet]
        public ActionResult ListarEmpresas()
        {
            List<TMEMPR> result= new List<TMEMPR>();
            try
            {
                result = sqlbl.ListarEmpresas();

            }
            catch (Exception ex)
            {
                result = new List<TMEMPR>();
            }

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[HttpGet]
        public ActionResult ListarSedesPorEmpresa(string CO_EMPR = "")
        {
            List<TTSEDE> result = new List<TTSEDE>();
            try
            {
                result = sqlbl.ListarSedesPorEmpresa(CO_EMPR);

            }
            catch (Exception ex)
            {
                result = new List<TTSEDE>();
            }

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[HttpGet]
        public ActionResult ListarTrabajadoresPorSedeYEmpresa(int periodo=1, int anio=1, string CO_EMPR="", string CO_SEDE="")
        {
            List<PersonaSqlEntidad> result = new List<PersonaSqlEntidad>();
            try
            {
                result = sqlbl.ListarTrabajadoresPorSedeYEmpresa(periodo,anio,CO_EMPR,CO_SEDE);

            }
            catch (Exception ex)
            {
                result = new List<PersonaSqlEntidad>();
            }

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get|HttpVerbs.Post)]
        public ActionResult ListarCumpleaniosOfisis(string CO_EMPR="", int limite=0)
        {
            List<PersonaSqlEntidad> resul = new List<PersonaSqlEntidad>();
            try
            {
                string limiteStr = string.Empty;
                if(!string.IsNullOrEmpty(CO_EMPR))
                {
                    CO_EMPR =$@" and empresa.CO_EMPR={CO_EMPR}";
                }
                if(limite > 0)
                {
                    limiteStr = $" top {limite}";
                }
                resul = sqlbl.ListarCumpleaniosOfisis(CO_EMPR, limiteStr);
            }
            catch (Exception)
            {
                resul = new List<PersonaSqlEntidad>();
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var data = new ContentResult
            {
                Content = serializer.Serialize(new{ data = resul }),
                ContentType = "application/json"
            };
            return data;
            //return Json(new { data = resul }, JsonRequestBehavior.AllowGet);
        }

    
        [HttpPost]
        public ActionResult ListarEnvios(string COD_EMPRESA, string CO_TRAB, string CO_PLAN, int NU_CORR_PERI, string CO_CPTO_FORM, int PERIODO, int anio)
        {
            TDINFO_TRAB result = new TDINFO_TRAB();
            try
            {
                result = sqlbl.GetInfoEnvio( COD_EMPRESA,  CO_TRAB,  CO_PLAN,  NU_CORR_PERI,  CO_CPTO_FORM,  PERIODO,  anio);

            }
            catch(Exception ex)
            {
                result = new TDINFO_TRAB();
            }

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[HttpGet]
        public ActionResult CreateInfoEnvio(TDINFO_TRAB ofiplan)
        {
            bool result = false;
            try
            {
                result = sqlbl.CreateInfoEnvio(ofiplan);

            }
            catch(Exception ex)
            {
                result = false;
            }

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[HttpGet]
        public ActionResult DeleteInfoEnvio(string COD_EMPRESA, string COD_TRABAJADOR, int PERIODO, int anio)
        {
            bool result = false;
            try
            {
                result = sqlbl.DeleteInfoEnvio( COD_EMPRESA,  COD_TRABAJADOR,  PERIODO,  anio);

            }
            catch(Exception ex)
            {
                result = false;
            }

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //[HttpGet]
        public ActionResult UpdateInfoenvio(string COD_EMPRESA, string CO_TRAB, int NU_CORR_PERI, int PERIODO, int anio, string CO_CPTO_FORM, DateTime FE_USUA_MODI, double NU_DATO_INFO)
        {
            bool result = false;
            try
            {
                result = sqlbl.UpdateInfoenvio( COD_EMPRESA,  CO_TRAB, NU_CORR_PERI, PERIODO,  anio,  CO_CPTO_FORM,  FE_USUA_MODI,  NU_DATO_INFO);

            }
            catch(Exception ex)
            {
                result = false;
            }

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
    }
}