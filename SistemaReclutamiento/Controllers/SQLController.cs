using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class SQLController : Controller
    {
        // GET: SQL
        SQLModel sqlbl = new SQLModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TMEMPRListarJson()
        {
            var errormensaje = "";
            var listaempresa = new List<TMEMPR>();
            var error = new claseError();
            bool response = false;
            try
            {
                var sqltupla = sqlbl.EmpresaListarJson();
                listaempresa = sqltupla.listaempresa;
                error = sqltupla.error;
                errormensaje = error.Value;
                if (errormensaje.Equals(string.Empty))
                {
                    response = true;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaempresa.ToList(), respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult TTPUES_TRABListarJson(string CO_EMPR)
        {
            var errormensaje = "";
            var listapuesto = new List<TTPUES_TRAB>();
            var error = new claseError();
            bool response = false;
            try
            {
                var sqltupla = sqlbl.PuestoTrabajoObtenerPorEmpresaJson(CO_EMPR);
                listapuesto = sqltupla.listapuesto;
                error = sqltupla.error;
                errormensaje = error.Value;
                if (errormensaje.Equals(string.Empty))
                {
                    response = true;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listapuesto.ToList(), respuesta = response, mensaje = errormensaje });
        }
    }
}