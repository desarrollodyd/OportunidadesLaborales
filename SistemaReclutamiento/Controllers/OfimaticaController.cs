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
        public ActionResult OfimaticaListarJson(int fkPosID)
        {
            var errormensaje = "";
            var lista = new List<ofimaticaEntidad>();
            try
            {
                lista = ofimaticabl.OfimaticaListaporPostulanteJson(fkPosID);
                errormensaje = "Cargando Data...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
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
                if (respuestaConsulta)
                {
                    errormensaje = "Se Registró Correctamente";
                }
                else
                {
                    errormensaje = "Error, no se Puede Registrar";
                }
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
                if (respuestaConsulta)
                {
                    errormensaje = "Se Editó Correctamente";
                }
                else
                {
                    errormensaje = "Error, no se Puede Editar";
                }
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
                if (respuestaConsulta)
                {
                    errormensaje = "Se Eliminó Correctamente";
                }
                else
                {
                    errormensaje = "Error, no se Puede Eliminar";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}