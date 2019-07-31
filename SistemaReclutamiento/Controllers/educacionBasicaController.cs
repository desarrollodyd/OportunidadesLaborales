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
        public ActionResult EducacionBasicaListarJson(int fkPosID)
        {
            var errormensaje = "";
            var lista = new List<educacionBasicaEntidad>();
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

        [HttpPost]
        public ActionResult EducacionBasicaInsertarJson(educacionBasicaEntidad educacionBasica)
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

        [HttpPost]
        public ActionResult EducacionBasicaEditarJson(educacionBasicaEntidad educacionBasica)
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