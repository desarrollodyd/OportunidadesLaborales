using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class ExperienciaController : Controller
    {
        // GET: Experiencia
        experienciaModel experienciabl = new experienciaModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ExperienciaInsertarJson(experienciaEntidad experiencia)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            experiencia.exp_fecha_reg = DateTime.Now;
            experiencia.exp_estado = "A";
            try
            {
                respuestaConsulta = experienciabl.ExperienciaInsertarJson(experiencia);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ExperienciaEditarJson(experienciaEntidad experiencia)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = experienciabl.ExperienciaEditarJson(experiencia);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult ExperienciaEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;

            try
            {
                respuestaConsulta = experienciabl.ExperienciaEliminarJson(id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}