using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;

namespace SistemaReclutamiento.Controllers
{
    public class EducacionSuperiorController : Controller
    {
        educacionSuperiorModel educacionsuperiorbl = new educacionSuperiorModel();
        // GET: EducacionSuperior
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EducacionSuperiorInsertarJson(educacionSuperiorEntidad educacionSuperior)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            educacionSuperior.esu_fecha_reg = DateTime.Now;
            try
            {
                respuestaConsulta = educacionsuperiorbl.EducacionSuperiorInsertarJson(educacionSuperior);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult EducacionSuperiorEditarJson(educacionSuperiorEntidad educacionSuperior)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = educacionsuperiorbl.EducacionSuperiorEditarJson(educacionSuperior);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult EducacionSuperiorEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;

            try
            {
                respuestaConsulta = educacionsuperiorbl.EducacionSuperiorEliminarJson(id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}