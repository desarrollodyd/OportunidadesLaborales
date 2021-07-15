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
    public class WebDetalleElementoController : Controller
    {
        WebDetalleElementoModel detallebl = new WebDetalleElementoModel();
        // GET: WebDetalleElemento
        public ActionResult Index()
        {
            return View();
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
                if (error.Respuesta)
                {
                    mensaje = "Listando Detalle Elemento";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
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
        public ActionResult WebDetalleElementoInsertarJson(WebDetalleElementoEntidad detalle) {
            string mensaje = "";
            bool respuesta = false;
            claseError error = new claseError();
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/WebFiles/";
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            try
            {
                if (detalle.fk_tipo == 4 || detalle.fk_tipo == 5 || detalle.fk_tipo == 6||detalle.fk_tipo==7)
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
                    if (detalle.fk_tipo == 7) {
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
                if (totaltupla.error.Respuesta)
                {
                    if (totaltupla.listadetalle.Count > 0)
                    {
                        detalle.detel_orden = totaltupla.listadetalle.Max(x => x.detel_orden) + 1;
                    }
                    else {
                        detalle.detel_orden = 1;
                    }
                    
                }
                else {
                    detalle.detel_orden = 1;
                }
                var detalleTupla = detallebl.WebDetalleElementoInsertarJson(detalle);
                if (detalleTupla.error.Respuesta) {
                    if (detalleTupla.idInsertado > 0)
                    {
                        respuesta = true;
                        mensaje = "Insertado";
                    }
                    else {
                        mensaje = "No se pudo insertar";
                    }
                }
                else {
                    mensaje = detalleTupla.error.Mensaje;
                }
            }
            catch (Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { mensaje,respuesta });
        }
        [HttpPost]
        public ActionResult WebDetalleElementoEliminarJson(int detel_id) {
            string mensaje = "";
            bool respuesta = false;
            WebDetalleElementoEntidad detalle = new WebDetalleElementoEntidad();
            claseError error = new claseError();
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/WebFiles/";
            string rutaEliminar = "";
            try
            {
                var detalleTupla = detallebl.WebDetalleElementoIdObtenerJson(detel_id);
                if (detalleTupla.error.Respuesta) {
                    //eliminar imagenes si las hubiera
                    detalle = detalleTupla.detalle;
                    if (detalle.fk_tipo == 4 || detalle.fk_tipo == 5 || detalle.fk_tipo == 6||detalle.fk_tipo==7)
                    {
                        
                        rutaEliminar = Path.Combine(direccion, detalle.detel_imagen);
                        if (System.IO.File.Exists(rutaEliminar))
                        {
                            System.IO.File.Delete(rutaEliminar);
                        }
                        if (detalle.fk_tipo == 7) {
                            rutaEliminar = Path.Combine(direccion, detalle.detel_imagen_detalle);
                            if (System.IO.File.Exists(rutaEliminar))
                            {
                                System.IO.File.Delete(rutaEliminar);
                            }
                        }
                    }
                }
                var eliminadoTupla = detallebl.WebDetalleElementoEliminarJson(detalle.detel_id);
                if (eliminadoTupla.error.Respuesta)
                {
                    respuesta = eliminadoTupla.eliminado;
                    mensaje = "Eliminado";
                }
                else {
                    mensaje = eliminadoTupla.error.Mensaje;
                }

            }
            catch (Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new {respuesta,mensaje });
        }
        [HttpPost]
        public ActionResult WebDetalleElementoEditarJson(WebDetalleElementoEntidad detalle) {

            int tamanioMaximo = 4194304;
            string extension = "";
            string mensaje = "";
            bool respuesta = false;
            string rutaInsertar = "";
            string rutaAnterior = "";
            WebDetalleElementoEntidad detalleelemento = new WebDetalleElementoEntidad();
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/WebFiles/";
            try
            {
                if (detalle.fk_tipo == 4 || detalle.fk_tipo == 5 || detalle.fk_tipo == 6 || detalle.fk_tipo == 7) {
                    HttpPostedFileBase imagen_1 = Request.Files[0];
                    if (imagen_1.ContentLength > 0 && imagen_1 != null)
                    {
                        if (imagen_1.ContentLength <= tamanioMaximo)
                        {
                            extension = Path.GetExtension(imagen_1.FileName);
                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                            {
                                var nombreArchivo = "Imagen_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                                rutaInsertar = Path.Combine(direccion, nombreArchivo);
                                rutaAnterior= Path.Combine(direccion, detalle.detel_nombre_imagen);
                               
                                if (!Directory.Exists(direccion))
                                {
                                    System.IO.Directory.CreateDirectory(direccion);
                                }
                                if (System.IO.File.Exists(rutaAnterior))
                                {
                                    System.IO.File.Delete(rutaAnterior);
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

                    }
                    else {
                        detalle.detel_imagen = detalle.detel_nombre_imagen;
                    }
                    if (detalle.fk_tipo == 7) {
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
                                    rutaAnterior = Path.Combine(direccion, detalle.detel_nombre_imagen_detalle);
                                    if (!Directory.Exists(direccion))
                                    {
                                        System.IO.Directory.CreateDirectory(direccion);
                                    }
                                    if (System.IO.File.Exists(rutaAnterior))
                                    {
                                        System.IO.File.Delete(rutaAnterior);
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
                        else {
                            detalle.detel_imagen_detalle = detalle.detel_nombre_imagen_detalle;
                        }
                    }
                }
                //Edicion
                var editadoTupla = detallebl.WebDetalleElementoEditarJson(detalle);
                if (editadoTupla.error.Respuesta) {
                    respuesta = editadoTupla.WebDetalleElementoEditado;
                    mensaje = "Editado";
                }
                else {
                    mensaje = editadoTupla.error.Mensaje;
                }
            }
            catch (Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new {respuesta,mensaje });
        }
        [HttpPost]
        public ActionResult WebDetalleElementoIDObtenerJson(int detel_id) {
            string errormensaje = "";
            bool response = false;
            WebDetalleElementoEntidad detalle = new WebDetalleElementoEntidad();
            try
            {
                var detalleTupla = detallebl.WebDetalleElementoIdObtenerJson(detel_id);
                if (detalleTupla.error.Respuesta) {
                    detalle = detalleTupla.detalle;
                    errormensaje = "Obteniendo Detalle";
                    response = true;
                }
                else
                {
                    errormensaje = detalleTupla.error.Mensaje;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { respuesta=response,data=detalle,mensaje=errormensaje });
        }
        [HttpPost]
        public ActionResult WebDetalleElementoEditarOrdenJson(WebDetalleElementoEntidad[] arrayDetElemento)
        {
            WebDetalleElementoEntidad detalle = new WebDetalleElementoEntidad();
            claseError error = new claseError();
            bool response = false;
            string errormensaje = "";
            int tamanio = arrayDetElemento.Length;
            foreach (var m in arrayDetElemento)
            {
                detalle.detel_id = m.detel_id;
                detalle.detel_orden = m.detel_orden;
                var reordenadoTupla = detallebl.WebDetalleElementoEditarOrdenJson(detalle);
                error = reordenadoTupla.error;
                if (error.Respuesta)
                {
                    response = reordenadoTupla.WebDetElementoReordenado;
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