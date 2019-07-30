using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class IdiomaController : Controller
    {
        // GET: Idiom
        idiomaModel idiomabl = new idiomaModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IdiomaInsertarJson(idiomaEntidad idioma)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            idioma.idi_fecha_reg = DateTime.Now;
            try
            {
                respuestaConsulta = idiomabl.IdiomaInsertarJson(idioma);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult IdiomaEditarJson(idiomaEntidad idioma)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = idiomabl.IdiomaEditarJson(idioma);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IdiomaEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;

            try
            {
                respuestaConsulta = idiomabl.IdiomaEliminarJson(id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}