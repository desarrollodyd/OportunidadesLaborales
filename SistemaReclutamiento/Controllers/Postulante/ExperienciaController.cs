using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    [autorizacion(false)]
    public class ExperienciaController : Controller
    {
        // GET: Experiencia
        ExperienciaModel experienciabl = new ExperienciaModel();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExperienciaListarJson(int fkPosID)
        {
            var errormensaje = "";
            var lista = new List<ExperienciaEntidad>();
            try
            {
                lista = experienciabl.ExperienciaListaporPostulanteJson(fkPosID);
                errormensaje = "Cargando Data...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult ExperienciaInsertarJson(ExperienciaEntidad experiencia)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            experiencia.exp_fecha_reg = DateTime.Now;
            experiencia.exp_estado = "A";
            try
            {
                respuestaConsulta = experienciabl.ExperienciaInsertarJson(experiencia);
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
        public ActionResult ExperienciaEditarJson(ExperienciaEntidad experiencia)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = experienciabl.ExperienciaEditarJson(experiencia);
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
        public ActionResult ExperienciaEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;

            try
            {
                respuestaConsulta = experienciabl.ExperienciaEliminarJson(id);
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