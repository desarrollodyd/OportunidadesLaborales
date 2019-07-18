using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class postulanteController : Controller
    {
        postulanteModel postulantebl = new postulanteModel();
        // GET: postulante
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostulanteEditarJson(postulanteEntidad postulante)
        {
            var errormensaje = "";
            bool respuestaConsulta = true;         
            try
            {
                respuestaConsulta = postulantebl.PostulanteEditarJson(postulante);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}