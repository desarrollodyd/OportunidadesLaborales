using SistemaReclutamiento.Models;
using SistemaReclutamiento.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class EducacionBasicaController : Controller
    {
        educacionBasicaModel educacionBasicabl = new educacionBasicaModel();
        // GET: educacionBasica
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EducacionBasicaInsertarJson(educacionBasicaEntidad educacionBasica)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {             
                respuestaConsulta = educacionBasicabl.EducacionBasicaInsertarJson(educacionBasica);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult EducacionBasicaEditarJson(educacionBasicaEntidad educacionBasica)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = educacionBasicabl.EducacionBasicaEditarJson(educacionBasica);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}