using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class OfimaticaController : Controller
    {
        ofimaticaModel ofimaticabl = new ofimaticaModel();
        // GET: Ofimatica
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OfimaticaInsertarJson(ofimaticaEntidad ofimatica)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            ofimatica.ofi_fecha_reg = DateTime.Now;
            try
            {
                respuestaConsulta = ofimaticabl.OfimaticaInsertarJson(ofimatica);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult OfimaticaEditarJson(ofimaticaEntidad ofimatica)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = ofimaticabl.OfimaticaEditarJson(ofimatica);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult OfimaticaEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;

            try
            {
                respuestaConsulta =ofimaticabl.OfimaticaEliminarJson(id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}