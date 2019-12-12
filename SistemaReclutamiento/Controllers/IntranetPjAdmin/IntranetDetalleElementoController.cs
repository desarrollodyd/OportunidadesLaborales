using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPJAdmin
{
    public class IntranetDetalleElementoController : Controller
    {
        IntranetDetalleElementoModel intranetDetalleElementonbl = new IntranetDetalleElementoModel();
        IntranetSeccionElementoModel intranetSeccionElementobl = new IntranetSeccionElementoModel();
        claseError error = new claseError();
        string pathArchivosIntranet= ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        // GET: IntranetImagen
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetDetalleElementoListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetDetalleElementoEntidad> listaDetalleElemento = new List<IntranetDetalleElementoEntidad>();
            try
            {
                var ImagenTupla = intranetDetalleElementonbl.IntranetDetalleElementoListarJson();
                error = ImagenTupla.error;
                listaDetalleElemento = ImagenTupla.intranetDetalleElementoLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Imagenes";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Imagenes";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaDetalleElemento.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetDetalleElementoListarxElementoIDJson(int elem_id) {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetDetalleElementoEntidad> listaDetalleElemento = new List<IntranetDetalleElementoEntidad>();
            try
            {
                var DetalleElementoTupla = intranetDetalleElementonbl.IntranetDetalleElementoListarxElementoIDJson(elem_id);
                error = DetalleElementoTupla.error;
                listaDetalleElemento = DetalleElementoTupla.intranetDetalleElementoListaxElementoID;
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
        public ActionResult IntranetDetalleElementoInsertarJson(IntranetDetalleElementoEntidad intranetDetalleElemento)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetDetalleElementoInsertado = 0;
            int tamanioMaximo=0;
            string extension = "";
            string rutaInsertar = "";
            int idSeccionElementoInsertado = 0;
            IntranetSeccionElementoEntidad intranetSeccionElemento = new IntranetSeccionElementoEntidad();
            claseError error = new claseError();
            
            //verificar si es imagen o abre un modal si fk_seccion_elemento==0 es imagen
            if (intranetDetalleElemento.fk_seccion_elemento == 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                tamanioMaximo = 4194304;
                if (file.ContentLength > 0 && file != null)
                {
                    if (file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName);
                        if (extension == ".jpg" || extension == ".png")
                        {
                            string nombreArchivo = ("Elemento_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                            var nombre = (nombreArchivo + extension);
                            rutaInsertar = Path.Combine(pathArchivosIntranet + "/", nombre);
                            if (!Directory.Exists(pathArchivosIntranet + "/"))
                            {
                                System.IO.Directory.CreateDirectory(pathArchivosIntranet + "/");
                            }
                            file.SaveAs(rutaInsertar);
                            intranetDetalleElemento.detel_nombre = nombreArchivo;
                            intranetDetalleElemento.detel_extension = (extension==".jpg"?"jpg":"png");
                        }
                        else
                        {
                            mensaje = "Solo se admiten archivos jpg o png.";
                            respuesta = false;
                            return Json(new { respuesta = respuesta, mensaje = mensaje });
                        }
                    }
                    else
                    {
                        mensaje = "El tamaño maximo de arhivo permitido es de 4Mb.";
                        respuesta = false;
                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                    }

                }
                else
                {
                    mensaje = "Error al Subir el Archivo.";
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                }
            }
            //Lista de Texto
            else if (intranetDetalleElemento.fk_seccion_elemento == 1) {
                try
                {
                    intranetSeccionElemento.sele_orden = 1;
                    intranetSeccionElemento.sele_estado = "A";
                    var seccionElementoTupla = intranetSeccionElementobl.IntranetSeccionElementoInsertarJson(intranetSeccionElemento);
                    error = seccionElementoTupla.error;
                    if (error.Key.Equals(string.Empty))
                    {
                        intranetDetalleElemento.fk_seccion_elemento = seccionElementoTupla.idIntranetSeccionElementoInsertado;
                        var totalDetallesTupla = intranetDetalleElementonbl.IntranetDetalleElementoObtenerTotalRegistrosxElementoJson(intranetDetalleElemento.fk_elemento);
                        if (totalDetallesTupla.error.Key.Equals(string.Empty))
                        {
                            intranetDetalleElemento.detel_orden = totalDetallesTupla.intranetDetalleElementoTotal + 1;
                            //Insertar Imagen si esque hubiera
                            if (intranetDetalleElemento.detel_nombre != ""&&intranetDetalleElemento.detel_nombre!=null) {
                             
                                HttpPostedFileBase file = Request.Files[0];
                                tamanioMaximo = 4194304;
                                if (file.ContentLength > 0 && file != null)
                                {
                                    if (file.ContentLength <= tamanioMaximo)
                                    {
                                        extension = Path.GetExtension(file.FileName);
                                        if (extension == ".jpg" || extension == ".png"||extension==".jpeg")
                                        {
                                            string nombreArchivo = ("Elemento_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                                            var nombre = (nombreArchivo + extension);
                                            rutaInsertar = Path.Combine(pathArchivosIntranet + "/", nombre);

                                            if (!Directory.Exists(pathArchivosIntranet + "/"))
                                            {
                                                System.IO.Directory.CreateDirectory(pathArchivosIntranet + "/");
                                            }

                                            file.SaveAs(rutaInsertar);
                                            intranetDetalleElemento.detel_nombre = nombreArchivo;

                                            if (extension == ".jpg") {
                                                intranetDetalleElemento.detel_extension = "jpg";
                                            }
                                            else if (extension == ".png") {
                                                intranetDetalleElemento.detel_extension = "png";
                                            }
                                            else {
                                                intranetDetalleElemento.detel_extension = "jpeg";
                                            }
                                        }
                                        else
                                        {
                                            mensaje = "Solo se admiten archivos jpg o png.";
                                            respuesta = false;
                                            return Json(new { respuesta = respuesta, mensaje = mensaje });
                                        }
                                    }
                                    else
                                    {
                                        mensaje = "El tamaño maximo de arhivo permitido es de 4Mb.";
                                        respuesta = false;
                                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                                    }

                                }
                                else
                                {
                                    mensaje = "Error al Subir el Archivo.";
                                    respuesta = false;
                                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                                }
                            }
                            
                        }
                        else
                        {
                            mensaje = totalDetallesTupla.error.Value;
                            respuesta = false;
                            return Json(new { respuesta = respuesta, mensaje = mensaje });
                        }
                    }
                    else
                    {
                        mensaje = "Error al Crear Seccion para este Modal.";
                        respuesta = false;
                        return Json(new { respuesta = respuesta, mensaje = mensaje });
                    }
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                }
            }
            //boton para abril un modal, debo de crear una seccion para el detalle elemento modal en la tabla int_seccion_elemento
            else
            {
                var totalDetallesTupla = intranetDetalleElementonbl.IntranetDetalleElementoObtenerTotalRegistrosxElementoJson(intranetDetalleElemento.fk_elemento);
                if (totalDetallesTupla.error.Key.Equals(string.Empty))
                {
                    intranetDetalleElemento.detel_orden = totalDetallesTupla.intranetDetalleElementoTotal + 1;
                    intranetDetalleElemento.fk_seccion_elemento = 0;
                    intranetDetalleElemento.detel_nombre = intranetDetalleElemento.detel_descripcion;
                }
                else
                {
                    mensaje = totalDetallesTupla.error.Value;
                    respuesta = false;
                    return Json(new { respuesta = respuesta, mensaje = mensaje });
                }
            }
            try
            {
                
                var DetalleElementoTupla = intranetDetalleElementonbl.IntranetDetalleElementoInsertarJson(intranetDetalleElemento);
                error = DetalleElementoTupla.error;

                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Se Registró Correctamente";
                    respuesta = true;
                    idIntranetDetalleElementoInsertado = DetalleElementoTupla.idIntranetDetalleElementoInsertado;
                }
                else
                {
                    mensaje = "No se Pudo insertar el Detalle";
                    mensajeConsola = error.Value;
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetDetalleElementoInsertado = idIntranetDetalleElementoInsertado, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetDetalleElementoEditarJson(IntranetDetalleElementoEntidad detalleElemento)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            try
            {
                var ImagenTupla = intranetDetalleElementonbl.IntranetDetalleElementoEditarJson(detalleElemento);
                error = ImagenTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = ImagenTupla.intranetDetalleElementoEditado;
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
        public ActionResult IntranetDetalleElementoEliminarJson(int detel_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            try
            {
                var ImagenTupla = intranetDetalleElementonbl.IntranetDetalleElementoEliminarJson(detel_id);
                error = ImagenTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = ImagenTupla.intranetDetalleElementoEliminado;
                    errormensaje = "Imagen Eliminada";
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