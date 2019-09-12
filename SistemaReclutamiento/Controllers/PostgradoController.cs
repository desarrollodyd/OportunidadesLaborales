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

    public class PostgradoController : Controller
    {
        // GET: Postgrado    
        PostgradoModel postgradobl = new PostgradoModel();
     
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostgradoListarJson(int fkPosID)
        {
            var errormensaje = "";
            var lista = new List<PostgradoEntidad>();
            try
            {
                lista = postgradobl.PostgradoListaporPostulanteJson(fkPosID);
                errormensaje = "Cargando Data...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult PostgradoInsertarJson(PostgradoEntidad postgrado)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            postgrado.pos_fecha_reg = DateTime.Now;
            try
            {
                respuestaConsulta = postgradobl.PostgradoInsertarJson(postgrado);
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
        public ActionResult PostgradoEditarJson(PostgradoEntidad postgrado)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = postgradobl.PostgradoEditarJson(postgrado);
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
        public ActionResult PostgradoEliminarJson(int id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                respuestaConsulta = postgradobl.PostgradoEliminarJson(id);
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