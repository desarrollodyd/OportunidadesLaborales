using SistemaReclutamiento.Models;
using SistemaReclutamiento.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Controllers
{
    [SeguridadMenu]
    public class EducacionBasicaController : Controller
    {
        EducacionBasicaModel educacionBasicabl = new EducacionBasicaModel();
        // GET: educacionBasica
        [SeguridadMenu(false)]
        public ActionResult Index()
        {
            return View();
        }
        [SeguridadMenu(false)]
        [HttpPost]
        public ActionResult EducacionBasicaListarJson(int fkPosID)
        {
            var errormensaje = "";
            var lista = new List<EducacionBasicaEntidad>();
            try
            {
                lista = educacionBasicabl.EducacionBasicaListaporPostulanteJson(fkPosID);
                errormensaje = "Cargando Data...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta=true, mensaje = errormensaje });
        }
        [SeguridadMenu(false)]
        [HttpPost]
        public ActionResult EducacionBasicaInsertarJson(EducacionBasicaEntidad educacionBasica)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {             
                respuestaConsulta = educacionBasicabl.EducacionBasicaInsertarJson(educacionBasica);
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
        public ActionResult EducacionBasicaEditarJson(EducacionBasicaEntidad educacionBasica)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = educacionBasicabl.EducacionBasicaEditarJson(educacionBasica);
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
        public ActionResult EducacionBasicaEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;

            try {
                respuestaConsulta = educacionBasicabl.EducacionBasicaEliminarJson(id);
                if (respuestaConsulta)
                {
                    errormensaje = "Se Eliminó Correctamente";
                }
                else
                {
                    errormensaje = "Error, no se Puede Eliminar";
                }
            }
            catch (Exception exp) {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });            
        }
    }
}