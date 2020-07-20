using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class CumUsuarioController : Controller
    {
        // GET: CumUsuario
        CumUsuarioModel cumUsuariobl = new CumUsuarioModel();
        CumUsuPreguntaModel cumUsuPreguntabl = new CumUsuPreguntaModel();
        CumUsuRespuestaModel cumUsuRespuestabl = new CumUsuRespuestaModel();

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CumUsuarioFkUsuarioObtenerJson(int fk_usuario)
        {
            var errormensaje = "";
            var response = false;
            CumUsuarioEntidad cumUsuario = new CumUsuarioEntidad();
            try
            {
                var cumUsuarioTupla = cumUsuariobl.CumUsuarioFkUsuarioObtenerJson(fk_usuario);
                if (cumUsuarioTupla.error.Key.Equals(string.Empty))
                {
                    cumUsuario=cumUsuarioTupla.cumUsuario;
                    if (cumUsuario.cus_id != 0)
                    {
                        var cumPreguntaTupla = cumUsuPreguntabl.CumUsuPreguntaListarxUsuarioJson(cumUsuario.cus_id);
                        if (cumPreguntaTupla.error.Key.Equals(string.Empty))
                        {
                            List<CumUsuPreguntaEntidad> listaPreguntas = new List<CumUsuPreguntaEntidad>();
                            foreach (var preg in cumPreguntaTupla.lista)
                            {
                                CumUsuPreguntaEntidad pregunta = new CumUsuPreguntaEntidad();
                                pregunta = preg;
                                var cumRespuestaTupla = cumUsuRespuestabl.CumUsuRespuestaListarxUsuPreguntaJson(pregunta.upr_id);
                                if (cumRespuestaTupla.error.Key.Equals(string.Empty))
                                {
                                    pregunta.CumUsuRespuesta=cumRespuestaTupla.lista;
                                }
                                else
                                {
                                    errormensaje = cumRespuestaTupla.error.Value;
                                }
                                listaPreguntas.Add(pregunta);
                            }
                            cumUsuario.CumUsuPregunta = listaPreguntas;

                        }
                        else
                        {
                            errormensaje = cumPreguntaTupla.error.Value;
                        }
                    }
                    else
                    {
                        cumUsuario.cus_fecha_reg = DateTime.Now;

                    }
                    errormensaje = "Cargando Data";
                    response = true;
                }
                else
                {
                    errormensaje = cumUsuarioTupla.error.Value;
                }
            
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = cumUsuario, respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult CumFichaInsertarJson()
        {
            UsuarioEntidad usuario = (UsuarioEntidad)Session["usu_full"];
            HttpPostedFileBase file = Request.Files[0];
            string request=Request.Params["usuario"];
            string extension = "";
            string rutaInsertar = "";
            string errormensaje = "";
            bool response = false;
            int tamanioMaximo = 4194304;
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "CumplimientoFiles/CumUsuario";
            int idUsuarioInsertado = 0;
            dynamic jsonObj = JsonConvert.DeserializeObject(request);
            CumUsuarioEntidad cumUsuario = new CumUsuarioEntidad();
            List<CumUsuPreguntaEntidad> listaPreguntas = new List<CumUsuPreguntaEntidad>();
            //Insertar Usuario
            cumUsuario.cus_id = Convert.ToInt32(jsonObj.cus_id);
            cumUsuario.cus_dni = Convert.ToString(jsonObj.cus_dni);
            cumUsuario.cus_tipo = Convert.ToString(jsonObj.cus_tipo);
            cumUsuario.cus_estado = "A";
            cumUsuario.fk_usuario = usuario.usu_id;
            cumUsuario.cus_fecha_reg = DateTime.Now;
            cumUsuario.cus_fecha_act = DateTime.Now;

            //Insertar Imagen
            if (file.ContentLength > 0 && file != null)
            {
                if (file.ContentLength <= tamanioMaximo)
                {
                    extension = Path.GetExtension(file.FileName);
                    if (extension == ".pdf" || extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                    {
                        var nombreArchivo = (cumUsuario.cus_dni.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                        rutaInsertar = Path.Combine(direccion, nombreArchivo);

                        if (!Directory.Exists(direccion))
                        {
                            System.IO.Directory.CreateDirectory(direccion);
                        }
                        file.SaveAs(rutaInsertar);
                        cumUsuario.cus_firma = nombreArchivo;
                    }
                    else
                    {
                        errormensaje = "Solo se admiten archivos de imagen o pdf.";
                        return Json(new { respuesta = response, mensaje = errormensaje });
                    }
                }
                else
                {
                    errormensaje = "El tamaño maximo de arhivo permitido es de 4Mb.";
                    return Json(new { respuesta = response, mensaje = errormensaje });
                }

            }
            else
            {
                errormensaje = "Debe subir un archivo Adjunto";
                return Json(new { respuesta = response, mensaje = errormensaje });
            }

            //insertar Usuario
            var cumUsuarioTupla = cumUsuariobl.CumUsuarioInsertarJson(cumUsuario);
            if (cumUsuarioTupla.error.Key.Equals(string.Empty))
            {
                idUsuarioInsertado = cumUsuarioTupla.idInsertado;
                foreach (var preg in jsonObj.CumUsuPregunta)
                {
                    CumUsuPreguntaEntidad pregunta = new CumUsuPreguntaEntidad();
                    List<CumUsuRespuestaEntidad> listaRespuesta = new List<CumUsuRespuestaEntidad>();
                    pregunta.upr_pregunta = preg.upr_pregunta;
                    pregunta.upr_dni = cumUsuario.cus_dni;
                    pregunta.upr_fecha_reg = DateTime.Now;
                    pregunta.upr_estado = "A";
                    pregunta.fk_pregunta = preg.fk_pregunta;
                    pregunta.fk_usuario = idUsuarioInsertado;
                    //Insertar Preguntas
                    var pregTupla = cumUsuPreguntabl.CumUsuPreguntaInsertarJson(pregunta);
                    if (pregTupla.error.Key.Equals(string.Empty))
                    {
                        int idPreguntaInsertada = pregTupla.idInsertado;
                        foreach (var resp in preg.CumUsuRespuesta)
                        {
                            CumUsuRespuestaEntidad respuesta = new CumUsuRespuestaEntidad();
                            respuesta.ure_respuesta = resp.ure_respuesta;
                            respuesta.ure_orden = resp.ure_orden;
                            respuesta.ure_tipo = resp.ure_tipo;
                            respuesta.ure_dni = cumUsuario.cus_dni;
                            respuesta.fk_usu_pregunta = idPreguntaInsertada;
                            respuesta.fk_usuario = idUsuarioInsertado;
                            respuesta.ure_estado = "A";
                            //Insertar Respuesta
                            var resTupla = cumUsuRespuestabl.CumUsuRespuestaInsertarJson(respuesta);
                            if (resTupla.error.Key.Equals(string.Empty))
                            {
                                errormensaje = "Insertado";
                                response = true;
                                respuesta.ure_id = resTupla.idInsertado;
                            }
                            else
                            {
                                return Json(new { respuesta = false, mensaje = "Error" });
                            }
                            listaRespuesta.Add(respuesta);
                        }
                        pregunta.CumUsuRespuesta = listaRespuesta;
                        listaPreguntas.Add(pregunta);
                    }
                    else
                    {
                        return Json(new { respuesta = false, mensaje = "Error" });
                    }
                  
                }
                cumUsuario.CumUsuPregunta = listaPreguntas;

            }
            else
            {
                return Json(new { respuesta = response, mensaje = "Error al Insertar" });
            }
            //Insertar Data

            return Json(new {respuesta=response,mensaje=errormensaje, data=cumUsuario });

        }
        [HttpPost]
        public ActionResult CumFichaEditarJson(CumUsuarioEntidad usuario)
        {
            string errormensaje = "";
            bool response = false;
            CumUsuarioEntidad cumUsuario = new CumUsuarioEntidad();
            cumUsuario = usuario;
            UsuarioEntidad usuarioSesion = (UsuarioEntidad)Session["usu_full"];
            cumUsuario.fk_usuario = usuarioSesion.usu_id;
            cumUsuario.cus_fecha_act = DateTime.Now;
            //Editar Usuario
            var usuarioTupla = cumUsuariobl.CumUsuarioEditarJson(cumUsuario);
            if (usuarioTupla.error.Key.Equals(string.Empty))
            {
                if (usuarioTupla.editado)
                {
                    //Editar Preguntas
                    foreach(var preg in cumUsuario.CumUsuPregunta)
                    {
                        preg.upr_fecha_act = DateTime.Now;
                        preg.fk_usuario = cumUsuario.cus_id;
                        var preguntaTupla = cumUsuPreguntabl.CumUsuPreguntaEditarJson(preg);
                        if (preguntaTupla.error.Key.Equals(string.Empty)){
                            if (preguntaTupla.editado)
                            {
                                //Editar Respuestas
                                foreach(var resp in preg.CumUsuRespuesta)
                                {
                                    resp.fk_usuario = cumUsuario.cus_id;
                                    resp.fk_usu_pregunta = preg.upr_id;
                                    resp.ure_dni = cumUsuario.cus_dni;
                                    var respuestaTupla = cumUsuRespuestabl.CumUsuRespuestaEditarJson(resp);
                                    if (respuestaTupla.error.Key.Equals(string.Empty))
                                    {
                                        errormensaje = "Editado";
                                        response = true;
                                    }
                                    else
                                    {
                                        errormensaje = respuestaTupla.error.Value;
                                    }
                                }
                                
                            }
                            else
                            {
                                errormensaje = "Error al Editar";
                            }
                           
                        }
                        else
                        {
                            errormensaje = preguntaTupla.error.Value;
                        }
                    }
                 
                }
                else
                {
                    errormensaje = "Error al Editar";
                }
            }
            else
            {
                errormensaje = usuarioTupla.error.Value;
            }

            return Json(new { respuesta = response, mensaje = errormensaje, data = usuario });
        }
    }
}