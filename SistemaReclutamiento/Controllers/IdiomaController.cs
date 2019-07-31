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
        public ActionResult IdiomaListarJson(int fkPosID)
        {
            var errormensaje = "";
            var lista = new List<idiomaEntidad>();
            try
            {
                lista = idiomabl.IdiomaListaporPostulanteJson(fkPosID);
                errormensaje = "Cargando Data...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
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
        public ActionResult IdiomaEditarJson(idiomaEntidad idioma)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = idiomabl.IdiomaEditarJson(idioma);
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
        public ActionResult IdiomaEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;

            try
            {
                respuestaConsulta = idiomabl.IdiomaEliminarJson(id);
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