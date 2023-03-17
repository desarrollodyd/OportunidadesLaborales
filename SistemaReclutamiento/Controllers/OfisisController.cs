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
    }
}