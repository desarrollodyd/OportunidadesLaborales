using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    public class IntranetSeccionController : Controller
    {
        // GET: IntranetSeccion
        IntranetSeccionModel intranetSeccionbl = new IntranetSeccionModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetSeccionListarxMenuIDJson(int menu_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetSeccionEntidad> listaMenus = new List<IntranetSeccionEntidad>();
            claseError error = new claseError();
            try
            {
                var seccionTupla = intranetSeccionbl.IntranetSeccionListarxMenuIDJson( menu_id);
                error = seccionTupla.error;
                listaMenus = seccionTupla.intranetSeccionListaxMenuID;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Secciones";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Secciones";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaMenus.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetSeccionListarTodoxMenuIDJson(int menu_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetSeccionEntidad> listaSecciones = new List<IntranetSeccionEntidad>();
            claseError error = new claseError();
            try
            {
                var seccionTupla = intranetSeccionbl.IntranetSeccionListarTodoxMenuIDJson(menu_id);
                error = seccionTupla.error;
                listaSecciones = seccionTupla.intranetSeccionListaTodoxMenuID;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Secciones";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Secciones";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaSecciones.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetSeccionEditarJson(IntranetSeccionEntidad intranetSeccion)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            claseError error = new claseError();
            try
            {
                var seccionTupla = intranetSeccionbl.IntranetSeccionEditarEstadoJson(intranetSeccion);
                error = seccionTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = seccionTupla.intranetSeccionEditado;
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
        public ActionResult IntranetSeccionInsertarJson(IntranetSeccionEntidad intranetSeccion)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetSeccionInsertado = 0;
            claseError error = new claseError();
            try
            {
                var totalSeccionesTupla = intranetSeccionbl.IntranetSeccionObtenerTotalRegistrosJson(intranetSeccion.fk_menu);
                error = totalSeccionesTupla.error;
                if (error.Key.Equals(string.Empty)) {
                    intranetSeccion.sec_orden = totalSeccionesTupla.intranetSeccionTotal + 1;
                    var seccionTupla = intranetSeccionbl.IntranetSeccionInsertarJson(intranetSeccion);
                    error = seccionTupla.error;

                    if (error.Key.Equals(string.Empty))
                    {
                        mensaje = "Se Registró Correctamente";
                        respuesta = true;
                        idIntranetSeccionInsertado = seccionTupla.idIntranetSeccionInsertado;
                    }
                    else
                    {
                        mensaje = "No se Pudo insertar el Menu";
                        mensajeConsola = error.Value;
                    }
                }
                else
                {
                    mensaje = "Error al Insertar la Nueva Seccion";
                    mensajeConsola = error.Value;
                }
                

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetMenuInsertado = idIntranetSeccionInsertado, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetSeccionEliminarJson(int sec_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            claseError error = new claseError();
            string mensajeConsola = "";
            try
            {
                var seccionTupla = intranetSeccionbl.IntranetSeccionEliminarJson(sec_id);
                error = seccionTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = seccionTupla.intranetSeccionEliminado;
                    errormensaje = "Seccion Eliminada";
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
        [HttpPost]
        public ActionResult IntranetSeccionEditarOrdenJson(IntranetSeccionEntidad[] arraySecciones)
        {
            IntranetSeccionEntidad intranetSeccion = new IntranetSeccionEntidad();
            claseError error = new claseError();
            bool response = false;
            string errormensaje = "";
            int tamanio = arraySecciones.Length;
            foreach (var m in arraySecciones)
            {
                intranetSeccion.sec_id = m.sec_id;
                intranetSeccion.sec_orden = m.sec_orden;
                var reordenadoTupla = intranetSeccionbl.IntranetSeccionEditarOrdenJson(intranetSeccion);
                error = reordenadoTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    response = reordenadoTupla.intranetSeccionReordenado;
                    errormensaje = "Editado";
                }
                else
                {
                    response = false;
                    errormensaje = "No se Pudo Editar";
                    return Json(new { respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
                }
            }
            return Json(new { tamanioseccion = tamanio, respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
        }

    }
}