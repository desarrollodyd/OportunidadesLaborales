using SistemaReclutamiento.Entidades.WebCorporativa;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.WebCorporativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.WebCorporativaAdmin
{
    public class WebCorporativaAdminController : Controller
    {
        WebElementoModel elementobl = new WebElementoModel();
        WebDetalleElementoModel detallebl = new WebDetalleElementoModel();
        // GET: WebCorporativaAdmin
        public ActionResult Menus()
        {
            return View("~/Views/WebCorporativaAdmin/WebMenus.cshtml");
        }

        public ActionResult PanelDepartamento() {
            return View("~/Views/WebCorporativaAdmin/WebDepartamento.cshtml");
        }
        public ActionResult PanelSecciones()
        {
            return View("~/Views/WebCorporativaAdmin/WebSecciones.cshtml");
        }


        //////////////////////////////////////////////////////
        ///

        [HttpPost]
        public ActionResult WebElementoListarxMenuIDxtipoJson(int menu_id=1,int tipo=1,string nombreElemento="")
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<WebElementoEntidad> listaElementos = new List<WebElementoEntidad>();
            List<WebDetalleElementoEntidad> listaDetalleElemento = new List<WebDetalleElementoEntidad>();
            claseError error = new claseError();
            var data = new List<dynamic>();
            try
            {
                var ElementoTupla = elementobl.WebElementoListarxMenuIDxtipoJson(menu_id,tipo);
                error = ElementoTupla.error;
                listaElementos = ElementoTupla.lista;
                if (error.Key.Equals(string.Empty))
                {
                    foreach (var obj in listaElementos)
                    {
                        var elem_id = obj.elem_id;
                        var DetalleElementoTupla = detallebl.WebDetalleElementoListarxElementoIDJson(elem_id);
                        error = DetalleElementoTupla.error;
                        listaDetalleElemento = DetalleElementoTupla.listadetalle;
                        data.Add(new{elemento=obj,detalle=listaDetalleElemento });
                    }
                    mensaje = "Listando "+ nombreElemento;
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Elementoes";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = data.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult WebElementoListarxMenuIDJson(int menu_id = 1)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<WebElementoEntidad> listaElementos = new List<WebElementoEntidad>();
            List<WebDetalleElementoEntidad> listaDetalleElemento = new List<WebDetalleElementoEntidad>();
            claseError error = new claseError();
            var data = new List<dynamic>();
            try
            {
                var ElementoTupla = elementobl.WebElementoListarxMenuIDJson(menu_id);
                error = ElementoTupla.error;
                listaElementos = ElementoTupla.lista;
                if (error.Key.Equals(string.Empty))
                {
                    foreach (var obj in listaElementos)
                    {
                        var elem_id = obj.elem_id;
                        var DetalleElementoTupla = detallebl.WebDetalleElementoListarxElementoIDJson(elem_id);
                        error = DetalleElementoTupla.error;
                        listaDetalleElemento = DetalleElementoTupla.listadetalle;
                        data.Add(new { elemento = obj, detalle = listaDetalleElemento });
                    }
                    mensaje = "Listando Data";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Elementoes";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = data.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult WebDetalleElementoInsertarJson(WebDetalleElementoEntidad detalle,int menu_id)
        {
            WebElementoEntidad elemento = new WebElementoEntidad();
            string mensaje = "";
            bool respuesta = false;
            claseError error = new claseError();
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/WebFiles/";
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            int elementoInsertado = 0;
            try
            {
                //encontrar elemento con menu_id y tipo
                var elementoTupla= elementobl.WebElementoListarxMenuIDxtipoJson(menu_id, detalle.fk_tipo);
                if (elementoTupla.error.Key.Equals(string.Empty))
                {
                    WebElementoEntidad elementoBusqueda = elementoTupla.lista.Where(x => x.fk_tipo_elemento == detalle.fk_tipo).SingleOrDefault();
                    if (elementoBusqueda != null)
                    {
                        detalle.fk_elemento = elementoBusqueda.elem_id;
                        elementoInsertado = elementoBusqueda.elem_id;
                    }
                    else
                    {
                        elemento.elem_orden = 1;
                        elemento.elem_estado = "A";
                        elemento.fk_menu = menu_id;
                        var elementoCreadoTupla = elementobl.WebElementoInsertarJson(elemento);
                        if (elementoCreadoTupla.error.Key.Equals(string.Empty))
                        {
                            detalle.fk_elemento = elementoCreadoTupla.idInsertado;
                            elementoInsertado = elementoCreadoTupla.idInsertado;
                        }
                        else
                        {
                            return Json(new { mensaje = elementoCreadoTupla.error.Value, respuesta });
                        }
                    }
                }
                else {
                    return Json(new { mensaje = elementoTupla.error.Value, respuesta });
                }
                if (detalle.fk_tipo == 1 || detalle.fk_tipo == 2 || detalle.fk_tipo == 3 || detalle.fk_tipo == 4||detalle.fk_tipo==5||detalle.fk_tipo==6||detalle.fk_tipo==7)
                {
                    HttpPostedFileBase imagen_1 = Request.Files[0];
                    //verificar si la primera imagen existe
                    if (imagen_1.ContentLength > 0 && imagen_1 != null)
                    {
                        if (imagen_1.ContentLength <= tamanioMaximo)
                        {
                            extension = Path.GetExtension(imagen_1.FileName);
                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                            {
                                var nombreArchivo = "Imagen_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                                rutaInsertar = Path.Combine(direccion, nombreArchivo);
                                if (!Directory.Exists(direccion))
                                {
                                    System.IO.Directory.CreateDirectory(direccion);
                                }
                                imagen_1.SaveAs(rutaInsertar);
                                detalle.detel_imagen = nombreArchivo;
                            }
                            else
                            {
                                mensaje = "Solo se aceptan formaton PNG ó JPG";
                                return Json(new { mensaje = mensaje, respuesta = respuesta });
                            }
                        }
                        else
                        {
                            return Json(new { respuesta = false, mensaje = "Archivo demasiado grande" });
                        }
                    }
                    if (detalle.fk_tipo == 1)
                    {
                        HttpPostedFileBase imagen_detalle = Request.Files[1];
                        //verificar si la primera imagen existe
                        if (imagen_detalle.ContentLength > 0 && imagen_detalle != null)
                        {
                            if (imagen_detalle.ContentLength <= tamanioMaximo)
                            {
                                extension = Path.GetExtension(imagen_detalle.FileName);
                                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                                {
                                    var nombreArchivo = "DetalleImagen_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                                    rutaInsertar = Path.Combine(direccion, nombreArchivo);
                                    if (!Directory.Exists(direccion))
                                    {
                                        System.IO.Directory.CreateDirectory(direccion);
                                    }
                                    imagen_detalle.SaveAs(rutaInsertar);
                                    detalle.detel_imagen_detalle = nombreArchivo;
                                }
                                else
                                {
                                    mensaje = "Solo se aceptan formaton PNG ó JPG";
                                    return Json(new { mensaje = mensaje, respuesta = respuesta });
                                }
                            }
                            else
                            {
                                return Json(new { respuesta = false, mensaje = "Archivo demasiado grande" });
                            }
                        }
                    }

                }
                //Insertar Detalles
                var totaltupla = detallebl.WebDetalleElementoListarxElementoIDJson(detalle.fk_elemento);
                if (totaltupla.error.Key.Equals(string.Empty))
                {
                    if (totaltupla.listadetalle.Count > 0)
                    {
                        detalle.detel_orden = totaltupla.listadetalle.Max(x => x.detel_orden) + 1;
                    }
                    else
                    {
                        detalle.detel_orden = 1;
                    }

                }
                else
                {
                    detalle.detel_orden = 1;
                }
                var detalleTupla = detallebl.WebDetalleElementoInsertarJson(detalle);
                if (detalleTupla.error.Key.Equals(string.Empty))
                {
                    if (detalleTupla.idInsertado > 0)
                    {
                        respuesta = true;
                        mensaje = "Insertado";
                    }
                    else
                    {
                        mensaje = "No se pudo insertar";
                    }
                }
                else
                {
                    mensaje = detalleTupla.error.Value;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta ,data=elementoInsertado});
        }
        [HttpPost]
        public ActionResult WebDetalleElementoListarxElementoIDJson(int elem_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<WebDetalleElementoEntidad> listaDetalleElemento = new List<WebDetalleElementoEntidad>();
            claseError error = new claseError();
            try
            {
                var DetalleElementoTupla = detallebl.WebDetalleElementoListarxElementoIDJson(elem_id);
                error = DetalleElementoTupla.error;
                listaDetalleElemento = DetalleElementoTupla.listadetalle;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Detalle Elemento";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar los Detalles";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaDetalleElemento.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult WebDetalleElementoEliminarJson(int detel_id)
        {
            string mensaje = "";
            bool respuesta = false;
            WebDetalleElementoEntidad detalle = new WebDetalleElementoEntidad();
            claseError error = new claseError();
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/WebFiles/";
            string rutaEliminar = "";
            try
            {
                var detalleTupla = detallebl.WebDetalleElementoIdObtenerJson(detel_id);
                if (detalleTupla.error.Key.Equals(string.Empty))
                {
                    //eliminar imagenes si las hubiera
                    detalle = detalleTupla.detalle;
                    if (detalle.fk_tipo == 1 || detalle.fk_tipo == 2 || detalle.fk_tipo == 3 || detalle.fk_tipo == 4||detalle.fk_tipo==5||detalle.fk_tipo==6||detalle.fk_tipo==7)
                    {

                        rutaEliminar = Path.Combine(direccion, detalle.detel_imagen);
                        if (System.IO.File.Exists(rutaEliminar))
                        {
                            System.IO.File.Delete(rutaEliminar);
                        }
                        if (detalle.fk_tipo == 1)
                        {
                            rutaEliminar = Path.Combine(direccion, detalle.detel_imagen_detalle);
                            if (System.IO.File.Exists(rutaEliminar))
                            {
                                System.IO.File.Delete(rutaEliminar);
                            }
                        }
                    }
                }
                var eliminadoTupla = detallebl.WebDetalleElementoEliminarJson(detalle.detel_id);
                if (eliminadoTupla.error.Key.Equals(string.Empty))
                {
                    respuesta = eliminadoTupla.eliminado;
                    mensaje = "Eliminado";
                }
                else
                {
                    mensaje = eliminadoTupla.error.Value;
                }

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { respuesta, mensaje });
        }

    }
}