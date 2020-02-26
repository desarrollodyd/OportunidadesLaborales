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
    public class WebDepartamentoController : Controller
    {
        WebDepartamentoModel departamentobl = new WebDepartamentoModel();
        // GET: WebDepartamento
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WebDepartamentoListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<WebDepartamentoEntidad> listaDepartamentos = new List<WebDepartamentoEntidad>();
            claseError error = new claseError();
            try
            {
                var departamentoTupla = departamentobl.WebDepartamentoListarJson();
                error = departamentoTupla.error;
                listaDepartamentos = departamentoTupla.lista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Departamentos";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar los Departamentos";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaDepartamentos.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult WebDepartamentoIdObtenerJson(int dep_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            WebDepartamentoEntidad departamento = new WebDepartamentoEntidad();
            claseError error = new claseError();
            try
            {
                var departamentoTupla = departamentobl.WebDepartamentoIdObtenerJson(dep_id);
                error = departamentoTupla.error;
                departamento = departamentoTupla.departamento;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Departamento Obtennido";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se pudo obtener informacion del Departamento";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = departamento, respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult WebDepartamentoInsertarJson(WebDepartamentoEntidad departamento)
        {
            //2 archivos
            HttpPostedFileBase imagen_departamento = Request.Files[0];
            HttpPostedFileBase imagen_departamento_detalle = Request.Files[1];
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            claseError error = new claseError();
            int idInsertadoo = 0;
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/WebFiles/";
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            try
            {
                //verificar si la primera imagen existe
                if (imagen_departamento.ContentLength > 0 && imagen_departamento!=null) {
                    if (imagen_departamento.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(imagen_departamento.FileName);
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                        {
                            var nombreArchivo = "Departamento_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                            rutaInsertar = Path.Combine(direccion, nombreArchivo);
                            if (!Directory.Exists(direccion))
                            {
                                System.IO.Directory.CreateDirectory(direccion);
                            }
                            imagen_departamento.SaveAs(rutaInsertar);
                            departamento.dep_imagen = nombreArchivo;
                        }
                        else
                        {
                            mensaje = "Solo se aceptan formaton PNG ó JPG";
                            return Json(new { mensaje = mensaje, respuesta = respuesta });
                        }
                    }
                    else {
                        return Json(new { respuesta = false, mensaje = "Archivo demasiado grande" });
                    }
                }
                //segunda imagen
                if (imagen_departamento_detalle.ContentLength > 0 && imagen_departamento_detalle != null)
                {
                    if (imagen_departamento_detalle.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(imagen_departamento.FileName);
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                        {
                            var nombreArchivo = "Departamento_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                            rutaInsertar = Path.Combine(direccion, nombreArchivo);
                            if (!Directory.Exists(direccion))
                            {
                                System.IO.Directory.CreateDirectory(direccion);
                            }
                            imagen_departamento_detalle.SaveAs(rutaInsertar);
                            departamento.dep_imagen_detalle = nombreArchivo;
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
                //Insercion a BD
                var departamentoTupla = departamentobl.WebDepartamentoInsertarJson(departamento);
                error = departamentoTupla.error;
               
                if (error.Key.Equals(string.Empty))
                {
                    idInsertadoo = departamentoTupla.idDeptatamentoInsertado;
                    if (idInsertadoo > 0) {
                        mensaje = "Departamento Insertado";
                        respuesta = true;
                    }
                    else
                    {
                        mensaje = "Ocurrio un error al Insertar";
                    }
                  
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se pudo insertar del Departamento";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = departamento, respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult WebDepartamentoEditarJson(WebDepartamentoEntidad departamento)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            claseError error = new claseError();
            bool editado = false;
            HttpPostedFileBase imagen_departamento = Request.Files[0];
            HttpPostedFileBase imagen_departamento_detalle = Request.Files[1];
            var direccion = Server.MapPath("/") + Request.ApplicationPath + "/WebFiles/";
            WebDepartamentoEntidad departamentoActual = new WebDepartamentoEntidad();
            int tamanioMaximo = 4194304;
            string extension = "";
            string rutaInsertar = "";
            try
            {
                var departamentIdTupla = departamentobl.WebDepartamentoIdObtenerJson(departamento.dep_id);
                if (departamentIdTupla.error.Key.Equals(string.Empty)) {
                    departamentoActual = departamentIdTupla.departamento;
                }
                //verificar si la primera imagen existe
                if (imagen_departamento.ContentLength > 0 && imagen_departamento != null)
                {
                    if (imagen_departamento.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(imagen_departamento.FileName);
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                        {
                            var nombreArchivo = "Departamento_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                            rutaInsertar = Path.Combine(direccion, nombreArchivo);
                            if (!Directory.Exists(direccion))
                            {
                                System.IO.Directory.CreateDirectory(direccion);
                            }
                            imagen_departamento.SaveAs(rutaInsertar);
                            departamento.dep_imagen = nombreArchivo;
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
                    departamento.dep_imagen = departamentoActual.dep_imagen;
                }
                //segunda imagen
                if (imagen_departamento_detalle.ContentLength > 0 && imagen_departamento_detalle != null)
                {
                    if (imagen_departamento_detalle.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(imagen_departamento.FileName);
                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                        {
                            var nombreArchivo = "Departamento_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                            rutaInsertar = Path.Combine(direccion, nombreArchivo);
                            if (!Directory.Exists(direccion))
                            {
                                System.IO.Directory.CreateDirectory(direccion);
                            }
                            imagen_departamento_detalle.SaveAs(rutaInsertar);
                            departamento.dep_imagen_detalle = nombreArchivo;
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
                else
                {
                    departamento.dep_imagen_detalle = departamentoActual.dep_imagen_detalle;
                }

                var departamentoTupla = departamentobl.WebDepartamentoEditarJson(departamento);
                error = departamentoTupla.error;

                if (error.Key.Equals(string.Empty))
                {
                    editado = departamentoTupla.DepartamentoEditado;
                    if (editado !=false)
                    {
                        mensaje = "Departamento Editado";
                        respuesta = true;
                    }
                    else
                    {
                        mensaje = "Ocurrio un error al Editar";
                    }
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se pudo editar el Departamento";
                }
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = departamento, respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult WebDepartamentoEliminarJson(int dep_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "/WebFiles/";
            string rutaEliminar = "";
            claseError error = new claseError();
            try
            {
                var departamentoIdTupla = departamentobl.WebDepartamentoIdObtenerJson(dep_id);
                if (departamentoIdTupla.error.Key.Equals(string.Empty)){
                    //Eliminar Archivos Primero
                    var dep_imagen = departamentoIdTupla.departamento.dep_imagen;
                    rutaEliminar = Path.Combine(direccion, dep_imagen);
                    if (System.IO.File.Exists(rutaEliminar))
                    {
                        System.IO.File.Delete(rutaEliminar);
                    }
                    var dep_imagen_detalle = departamentoIdTupla.departamento.dep_imagen_detalle;
                    rutaEliminar = Path.Combine(direccion, dep_imagen_detalle);
                    if (System.IO.File.Exists(rutaEliminar))
                    {
                        System.IO.File.Delete(rutaEliminar);
                    }
                }

                var departamentoTupla = departamentobl.WebDepartamentoEliminarJson(dep_id);
                error = departamentoTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Departamento Eliminado";
                    respuesta = departamentoTupla.DepartamentoEliminado;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se pudo Eliminar";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
    }
}