using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPJAdmin
{
    public class IntranetActividadesController : Controller
    {
        IntranetActividadesModel intranetActividadesbl = new IntranetActividadesModel();
        claseError error = new claseError();
        // GET: IntranetActividades
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetActividadesListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetActividadesEntidad> listaActividades = new List<IntranetActividadesEntidad>();
            try
            {
                var ActividadesTupla = intranetActividadesbl.IntranetActividadesListarJson();
                error = ActividadesTupla.error;
                listaActividades = ActividadesTupla.intranetActividadesLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Actividadess";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Actividadess";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaActividades.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetActividadesIdObtenerJson(int act_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            IntranetActividadesEntidad actividad = new IntranetActividadesEntidad();
            try
            {
                var actividadesTupla = intranetActividadesbl.IntranetActividadesIdObtenerJson(act_id);
                error = actividadesTupla.error;
                actividad = actividadesTupla.intranetActividades;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Obteniendo Informacion de la Actividad Seleccionada";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudo Obtener La Informacion de la Actividad Seleccionada";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = actividad, respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetActividadesInsertarJson(IntranetActividadesEntidad intranetActividades)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetActividadesInsertado = 0;
            try
            {
                var ActividadesTupla = intranetActividadesbl.IntranetActividadesInsertarJson(intranetActividades);
                error = ActividadesTupla.error;

                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Se Registró Correctamente";
                    respuesta = true;
                    idIntranetActividadesInsertado = ActividadesTupla.idIntranetActividadesInsertado;
                }
                else
                {
                    mensaje = "No se Pudo insertar las Actividades";
                    mensajeConsola = error.Value;
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetActividadesInsertado = idIntranetActividadesInsertado, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetActividadesEditarJson(IntranetActividadesEntidad intranetActividades)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            try
            {
                var ActividadesTupla = intranetActividadesbl.IntranetActividadesEditarJson(intranetActividades);
                error = ActividadesTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = ActividadesTupla.intranetActividadesEditado;
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
        public ActionResult IntranetActividadesEliminarJson(int act_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            try
            {

                var ActividadesTupla = intranetActividadesbl.IntranetActividadesEliminarJson(act_id);
                error = ActividadesTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = ActividadesTupla.intranetActividadesEliminado;
                    errormensaje = "Actividades Eliminado";
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
        public ActionResult IntranetActividadesListarTodoJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetActividadesEntidad> listaActividades = new List<IntranetActividadesEntidad>();
            try
            {
                var ActividadesTupla = intranetActividadesbl.IntranetActividadesListarTodoJson();
                error = ActividadesTupla.error;
                listaActividades = ActividadesTupla.intranetActividadesLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Actividadess";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Actividadess";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaActividades.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetActividadGuardarJson(IntranetActividadesEntidad intranetActividad)
        {
            HttpPostedFileBase file = Request.Files[0];
            string mensaje = "";
            string mensajeConsola = "";
            string accion = "";
            bool respuesta = false;
            int idIntranetActividadInsertado = 0;
            claseError error = new claseError();
            int tamanioMaximo = 4194304;
            string extension = "";
            if (intranetActividad.act_id == 0)
            {
                //Insertar
                try
                {
                    //IMAGEN
                    if (file.ContentLength > 0 && file != null)
                    {
                        if (file.ContentLength <= tamanioMaximo) {
                            extension = Path.GetExtension(file.FileName);
                            if (extension == ".jpg" || extension == ".png") {
                                var nombreArchivo = ("Actividad_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                                if (!Directory.Exists("~/Content/intranet/images/png/"))
                                {
                                    System.IO.Directory.CreateDirectory("~/Content/intranet/images/png/");
                                }
                                file.SaveAs(Server.MapPath("~/Content/intranet/images/png/" + nombreArchivo));
                                intranetActividad.act_imagen = nombreArchivo;
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
                    else {
                        intranetActividad.act_imagen = "";
                    }
                    var actividadTupla = intranetActividadesbl.IntranetActividadesInsertarJson(intranetActividad);
                    error = actividadTupla.error;

                    if (error.Key.Equals(string.Empty))
                    {
                        mensaje = "Se Registró Correctamente";
                        respuesta = true;
                        idIntranetActividadInsertado = actividadTupla.idIntranetActividadesInsertado;
                        accion = "Insertado";
                    }
                    else
                    {
                        mensaje = "No se Pudo insertar la Actividad";
                        mensajeConsola = error.Value;
                    }

                }
                catch (Exception exp)
                {
                    mensaje = exp.Message + " ,Llame Administrador";
                }

            }
            else
            {
                //Editar
                try
                {

                    if (intranetActividad.img_ubicacion == "")
                    {
                        intranetActividad.act_imagen = intranetActividad.img_ubicacion;
                        var actividadTupla = intranetActividadesbl.IntranetActividadesEditarJson(intranetActividad);
                        error = actividadTupla.error;
                        if (error.Key.Equals(string.Empty))
                        {
                            respuesta = actividadTupla.intranetActividadesEditado;
                            mensaje = "Se Editó la Actividad Correctamente";
                            accion = "Editado";
                        }
                        else
                        {
                            mensajeConsola = error.Value;
                            mensaje = "Error, no se Puede Editar";
                        }
                    }
                    else {
                        if (file.ContentLength > 0 && file != null)
                        {
                            if (file.ContentLength <= tamanioMaximo)
                            {
                                extension = Path.GetExtension(file.FileName);
                                if (extension == ".jpg" || extension == ".png")
                                {
                                    var nombreArchivo = ("Actividad_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                                    if (!Directory.Exists("~/Content/intranet/images/png/"))
                                    {
                                        System.IO.Directory.CreateDirectory("~/Content/intranet/images/png/");
                                    }
                                    file.SaveAs(Server.MapPath("~/Content/intranet/images/png/" + nombreArchivo));
                                    intranetActividad.act_imagen = nombreArchivo;
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
                            intranetActividad.act_imagen = "";
                        }
                        var actividadTupla = intranetActividadesbl.IntranetActividadesEditarJson(intranetActividad);
                        error = actividadTupla.error;
                        if (error.Key.Equals(string.Empty))
                        {
                            respuesta = actividadTupla.intranetActividadesEditado;
                            mensaje = "Se Editó la Actividad Correctamente";
                            accion = "Editado";
                        }
                        else
                        {
                            mensajeConsola = error.Value;
                            mensaje = "Error, no se Puede Editar";
                        }

                    }
                    
                }
                catch (Exception exp)
                {
                    mensaje = exp.Message + " ,Llame Administrador";
                }
            }


            return Json(new { respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola, accion = accion });
        }
        [HttpPost]
        public ActionResult IntranetActividadesEliminarVariosJson(int[] listaActividadesEliminar)
        {
            string errormensaje = "";
            string mensajeConsola = "";
            bool respuestaConsulta = false;
            claseError error = new claseError();
            try
            {
                for (int i = 0; i <= listaActividadesEliminar.Length - 1; i++)
                {
                    var actividadTupla = intranetActividadesbl.IntranetActividadesEliminarJson(listaActividadesEliminar[i]);
                    error = actividadTupla.error;
                    if (error.Key.Equals(string.Empty))
                    {
                        respuestaConsulta = actividadTupla.intranetActividadesEliminado;
                        errormensaje = "Actividad Eliminada";
                    }
                    else
                    {
                        errormensaje = "Error, no se Puede Eliminar";
                        mensajeConsola = error.Value;
                    }
                }
                respuestaConsulta = true;
            }
            catch (Exception ex)
            {

            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
    }
}