using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Controllers
{
    public class EducacionSuperiorController : Controller
    {
        EducacionSuperiorModel educacionsuperiorbl = new EducacionSuperiorModel();
      
        // GET: EducacionSuperior
        public ActionResult Index()
        {
            return View();
        }
        [SeguridadMenu(false)]
        [HttpPost]
    
        public ActionResult EducacionSuperiorListarJson(int fkPosID)
        {
            var errormensaje = "";
            var lista = new List<EducacionSuperiorEntidad>();
            try
            {
                lista = educacionsuperiorbl.EducacionSuperiorListaporPostulanteJson(fkPosID);
                errormensaje = "Cargando Data...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
        }
        [SeguridadMenu(false)]
        [HttpPost]
        public ActionResult EducacionSuperiorInsertarJson(EducacionSuperiorEntidad educacionSuperior)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            educacionSuperior.esu_fecha_reg = DateTime.Now;
            try
            {
                respuestaConsulta = educacionsuperiorbl.EducacionSuperiorInsertarJson(educacionSuperior);
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
        [SeguridadMenu(false)]
        [HttpPost]
        public ActionResult EducacionSuperiorEditarJson(EducacionSuperiorEntidad educacionSuperior)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = educacionsuperiorbl.EducacionSuperiorEditarJson(educacionSuperior);
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
        [SeguridadMenu(false)]
        [HttpPost]
        public ActionResult EducacionSuperiorEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;

            try
            {
                respuestaConsulta = educacionsuperiorbl.EducacionSuperiorEliminarJson(id);
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