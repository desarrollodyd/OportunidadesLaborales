using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPJAdmin
{
    public class IntranetAplicativoController : Controller
    {
        IntranetAplicativoModel intranetAplicativobl = new IntranetAplicativoModel();
        claseError error = new claseError();
        // GET: IntranetAplicativo
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetAplicativoListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetAplicativoEntidad> listaAplicativo = new List<IntranetAplicativoEntidad>();
            try
            {
                var AplicativoTupla = intranetAplicativobl.IntranetAplicativoListarJson();
                error = AplicativoTupla.error;
                listaAplicativo = AplicativoTupla.intranetAplicativoLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Aplicativos";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Aplicativos";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaAplicativo.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetAplicativoInsertarJson(IntranetAplicativoEntidad intranetAplicativo)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetAplicativoInsertado = 0;
            try
            {
                var AplicativoTupla = intranetAplicativobl.IntranetAplicativoInsertarJson(intranetAplicativo);
                error = AplicativoTupla.error;

                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Se Registró Correctamente";
                    respuesta = true;
                    idIntranetAplicativoInsertado = AplicativoTupla.idIntranetAplicativoInsertado;
                }
                else
                {
                    mensaje = "No se Pudo insertar las Aplicativo";
                    mensajeConsola = error.Value;
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetAplicativoInsertado = idIntranetAplicativoInsertado, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetAplicativoEditarJson(IntranetAplicativoEntidad intranetAplicativo)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            try
            {
                var AplicativoTupla = intranetAplicativobl.IntranetAplicativoEditarJson(intranetAplicativo);
                error = AplicativoTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = AplicativoTupla.intranetAplicativoEditado;
                    errormensaje = "Se Editó Correctamente";
                }
                else
                {
                    mensajeConsola = error.Value;
                    errormensaje = "Error, no se Puede Editar";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetAplicativoEliminarJson(int apl_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            try
            {
                var AplicativoTupla = intranetAplicativobl.IntranetAplicativoEliminarJson(apl_id);
                error = AplicativoTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = AplicativoTupla.intranetAplicativoEliminado;
                    errormensaje = "Aplicativo Eliminado";
                }
                else
                {
                    errormensaje = "Error, no se Puede Eliminar";
                    mensajeConsola = error.Value;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
    }
}