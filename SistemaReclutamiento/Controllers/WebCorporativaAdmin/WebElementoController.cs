using SistemaReclutamiento.Entidades.WebCorporativa;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.WebCorporativa;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.WebCorporativaAdmin
{
    [autorizacion(false)]
    public class WebElementoController : Controller
    {
        // GET: WebElemento
        WebElementoModel elementobl = new WebElementoModel();
        WebDetalleElementoModel detallebl = new WebDetalleElementoModel();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WebElementoListarxMenuIDJson(int menu_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<WebElementoEntidad> listaElementos = new List<WebElementoEntidad>();
            claseError error = new claseError();
            try
            {
                var ElementoTupla = elementobl.WebElementoListarxMenuIDJson(menu_id);
                error = ElementoTupla.error;
                listaElementos = ElementoTupla.lista;
                if (error.Respuesta)
                {
                    mensaje = "Listando Elementos";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar las Elementoes";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaElementos.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult WebElementoInsertarJson(WebElementoEntidad elemento)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idElementoInsertado = 0;
            claseError error = new claseError();
            try
            {
                var totalElementosTupla = elementobl.WebElementoListarxMenuIDJson(elemento.fk_menu);
                error = totalElementosTupla.error;
                if (error.Respuesta)
                {
                    if (totalElementosTupla.lista.Count > 0)
                    {
                        elemento.elem_orden = totalElementosTupla.lista.Max(x => x.elem_orden) + 1;
                    }
                    else
                    {
                        elemento.elem_orden = 1;
                    }
                    var idInsertado = elementobl.WebElementoInsertarJson(elemento);
                    error = idInsertado.error;

                    if (error.Respuesta)
                    {
                        mensaje = "Se Registró Correctamente";
                        respuesta = true;
                        idElementoInsertado = idInsertado.idInsertado;
                    }
                    else
                    {
                        mensaje = "No se Pudo insertar el Elemento";
                        mensajeConsola = error.Mensaje;
                    }
                }
                else
                {
                    mensaje = "Error al Insertar el Nuevo Elemento";
                    mensajeConsola = error.Mensaje;
                }


            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idElementoInsertado = idElementoInsertado, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult WebElementoIdObtenerJson(int elem_id)
        {
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            WebElementoEntidad intranetElemento = new WebElementoEntidad();
            try
            {
                var intranetElementoTupla = elementobl.WebElementoIdObtenerJson(elem_id);
                error = intranetElementoTupla.error;
                if (error.Respuesta)
                {
                    intranetElemento = intranetElementoTupla.elemento;
                    errormensaje = "Obteniendo Data";
                    response = true;
                }
                else
                {
                    errormensaje = error.Mensaje;
                }
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message + "Consulte con Administrador";
            }
            return Json(new { respuesta = response, mensaje = errormensaje, data = intranetElemento });
        }
        [HttpPost]
        public ActionResult WebElementoEditarJson(WebElementoEntidad elemento)
        {
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            try
            {
                var intranetElementoTupla = elementobl.WebElementoEditarJson(elemento);
                error = intranetElementoTupla.error;
                if (error.Respuesta)
                {
                    response = intranetElementoTupla.WebElementoEditado;
                    errormensaje = "Elemento Editado";
                }
                else
                {
                    errormensaje = error.Mensaje;
                }

            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult WebElementoEliminarJson(int elem_id)
        {
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            List<WebDetalleElementoEntidad> listadetalles = new List<WebDetalleElementoEntidad>();
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/WebFiles/";
            string rutaEliminar = "";
            try
            {
                var listaTupla = detallebl.WebDetalleElementoListarxElementoIDJson(elem_id);
                if (listaTupla.error.Respuesta) {
                    listadetalles = listaTupla.listadetalle;
                    foreach(var m in listadetalles)
                    {
                        if (m.fk_tipo == 4 || m.fk_tipo == 5 || m.fk_tipo == 6 || m.fk_tipo == 7) {
                            rutaEliminar = Path.Combine(direccion, m.detel_imagen);
                            if (System.IO.File.Exists(rutaEliminar))
                            {
                                System.IO.File.Delete(rutaEliminar);
                            }
                            if (m.fk_tipo == 7) {
                                rutaEliminar = Path.Combine(direccion, m.detel_imagen_detalle);
                                if (System.IO.File.Exists(rutaEliminar))
                                {
                                    System.IO.File.Delete(rutaEliminar);
                                }
                            }
                        }
                        
                    }
                }
                var intranetElementoTupla = elementobl.WebElementoEliminarJson(elem_id);
                error = intranetElementoTupla.error;
                if (error.Respuesta)
                {
                    response = intranetElementoTupla.eliminado;
                    errormensaje = "Elemento Eliminado";
                }
                else
                {
                    errormensaje = error.Mensaje;
                }

            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult WebElementoEditarOrdenJson(WebElementoEntidad[] arrayElementos)
        {
            WebElementoEntidad elemento = new WebElementoEntidad();
            claseError error = new claseError();
            bool response = false;
            string errormensaje = "";
            int tamanio = arrayElementos.Length;
            foreach (var m in arrayElementos)
            {
                elemento.elem_id = m.elem_id;
                elemento.elem_orden = m.elem_orden;
                var reordenadoTupla = elementobl.WebElementoEditarOrdenJson(elemento);
                error = reordenadoTupla.error;
                if (error.Respuesta)
                {
                    response = reordenadoTupla.reordenado;
                    errormensaje = "Editado";
                }
                else
                {
                    response = false;
                    errormensaje = "No se Pudo Editar";
                    return Json(new { respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
                }
            }
            return Json(new { tamanioelemento = tamanio, respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
        }
    }
}