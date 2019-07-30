using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class PostgradoController : Controller
    {
        // GET: Postgrado    
        postgradoModel postgradobl = new postgradoModel();
     
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostgradoInsertarJson(postgradoEntidad postgrado)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            postgrado.pos_fecha_reg = DateTime.Now;
            try
            {
                respuestaConsulta = postgradobl.PostgradoInsertarJson(postgrado);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult PostgradoEditarJson(postgradoEntidad postgrado)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = postgradobl.PostgradoEditarJson(postgrado);

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult PostgradoEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = postgradobl.PostgradoEliminarJson(id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}