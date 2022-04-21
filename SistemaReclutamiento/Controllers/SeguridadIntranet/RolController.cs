using SistemaReclutamiento.Entidades.SeguridadIntranet;
using SistemaReclutamiento.Models.SeguridadIntranet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.SeguridadIntranet
{
    public class RolController : Controller
    {
        private SEG_RolDAL webRolBl = new SEG_RolDAL();
        public ActionResult ListadoRol()
        {
            return View("~/Views/SeguridadIntranet/RolListadoVista.cshtml");
        }
        [HttpPost]
        public ActionResult GetListadoRol()
        {
            var errormensaje = "";
            var lista = new List<SEG_RolEntidad>();
            bool respuesta = false;
            try
            {
                var listaTupla = webRolBl.GetRoles();
                lista = listaTupla.lista;
                respuesta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { data = lista.ToList(), mensaje = errormensaje,respuesta });
            //  var aa = lista.ToList();

        }

        [HttpPost]
        public ActionResult GuardarRol(SEG_RolEntidad rol)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                var respuestaConsultaTupla = webRolBl.GuardarRol(rol);
                respuestaConsulta = respuestaConsultaTupla.respuesta;

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ActualizarRol(SEG_RolEntidad rol)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                var respuestaConsutltaTupla = webRolBl.ActualizarRol(rol);
                respuestaConsulta = respuestaConsutltaTupla.respuesta;

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ActualizarEstadoRol(int rolId, int estado)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                var respuestaConsultaTupla = webRolBl.ActualizarEstadoRol(rolId, estado);
                respuestaConsulta = respuestaConsultaTupla.respuesta;

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult DeleteRolId(int rolId)
        {
            var errormensaje = "Accion realizada Correctamente.";
            bool respuestaConsulta = false;
            try
            {
                var respuestaConsultaTupla = webRolBl.EliminarRol(rolId);
                respuestaConsulta = respuestaConsultaTupla.respuesta;

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult GetRolId(int WEB_RolID)
        {
            var errormensaje = "";
            var rol = new SEG_RolEntidad();
            bool respuesta = false;
            try
            {
                var listaTupla = webRolBl.GetRolId(WEB_RolID);
                rol = listaTupla.webRol;
                respuesta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { data = rol, mensaje = errormensaje,respuesta });
        }
    }
}