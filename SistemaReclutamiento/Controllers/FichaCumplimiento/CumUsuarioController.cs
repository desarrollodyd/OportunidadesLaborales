using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.FichaCumplimiento;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Controllers
{
    [autorizacion(false)]
    public class CumUsuarioController : Controller
    {
        // GET: CumUsuario
        CumUsuarioModel cumUsuariobl = new CumUsuarioModel();
        CumUsuPreguntaModel cumUsuPreguntabl = new CumUsuPreguntaModel();
        CumUsuRespuestaModel cumUsuRespuestabl = new CumUsuRespuestaModel();
        IntranetFichaModel intranetFichabl = new IntranetFichaModel();
        SQLModel sqlBL = new SQLModel();
        CumEnvioDetModel envDetModelbl = new CumEnvioDetModel();
        CumEnvioModel cumEnviobl = new CumEnvioModel();
        CumEnvioDetModel cumEnvioDetbl = new CumEnvioDetModel();

        public ActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult CumUsuarioFkUsuarioObtenerJson(int fk_usuario)
        //{
        //    var errormensaje = "";
        //    var response = false;
        //    CumUsuarioEntidad cumUsuario = new CumUsuarioEntidad();
        //    try
        //    {
        //        var cumUsuarioTupla = cumUsuariobl.CumUsuarioFkUsuarioObtenerJson(fk_usuario);
        //        if (cumUsuarioTupla.error.Respuesta)
        //        {
        //            cumUsuario=cumUsuarioTupla.cumUsuario;
        //            if (cumUsuario.cus_id != 0)
        //            {
        //                var cumPreguntaTupla = cumUsuPreguntabl.CumUsuPreguntaListarxUsuarioJson(cumUsuario.cus_id);
        //                if (cumPreguntaTupla.error.Respuesta)
        //                {
        //                    List<CumUsuPreguntaEntidad> listaPreguntas = new List<CumUsuPreguntaEntidad>();
        //                    foreach (var preg in cumPreguntaTupla.lista)
        //                    {
        //                        CumUsuPreguntaEntidad pregunta = new CumUsuPreguntaEntidad();
        //                        pregunta = preg;
        //                        var cumRespuestaTupla = cumUsuRespuestabl.CumUsuRespuestaListarxUsuPreguntaJson(pregunta.upr_id);
        //                        if (cumRespuestaTupla.error.Respuesta)
        //                        {
        //                            pregunta.CumUsuRespuesta=cumRespuestaTupla.lista;
        //                        }
        //                        else
        //                        {
        //                            errormensaje = cumRespuestaTupla.error.Mensaje;
        //                        }
        //                        listaPreguntas.Add(pregunta);
        //                    }
        //                    cumUsuario.CumUsuPregunta = listaPreguntas;

        //                }
        //                else
        //                {
        //                    errormensaje = cumPreguntaTupla.error.Mensaje;
        //                }
        //            }
        //            else
        //            {
        //                cumUsuario.cus_fecha_reg = DateTime.Now;

        //            }
        //            errormensaje = "Cargando Data";
        //            response = true;
        //        }
        //        else
        //        {
        //            errormensaje = cumUsuarioTupla.error.Mensaje;
        //        }
            
        //    }
        //    catch (Exception exp)
        //    {
        //        errormensaje = exp.Message + ",Llame Administrador";
        //    }
        //    return Json(new { data = cumUsuario, respuesta = response, mensaje = errormensaje });
        //}
        [HttpPost]
        public ActionResult CumFichaInsertarJson()
        {
            HttpPostedFileBase file = Request.Files[0];
            string request=Request.Params["usuario"];
            string extension = "";
            string rutaInsertar = "";
            string errormensaje = "";
            bool response = false;
            int tamanioMaximo = 4194304;
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "/CumplimientoFiles/CumUsuario";
            dynamic jsonObj = JsonConvert.DeserializeObject(request);

            CumUsuarioEntidad cumUsuario = new CumUsuarioEntidad();
            List<CumUsuPreguntaEntidad> listaPreguntas = new List<CumUsuPreguntaEntidad>();

            CumUsuPreguntaEntidad cumUsuPregunta = new CumUsuPreguntaEntidad();
            CumUsuRespuestaEntidad cumUsuRespuesta = new CumUsuRespuestaEntidad();
            CumEnvioDetalleEntidad cumEnvioDetalle = new CumEnvioDetalleEntidad();
            CumEnvioEntidad cumEnvio = new CumEnvioEntidad();
            //Insertar Usuario
            var usuario = jsonObj.CumEnvio.CumUsuario;
            var preguntas = usuario.CumUsuPregunta;
            var envioDetalle = jsonObj;
            var envio = jsonObj.CumEnvio;




            //Insertar Usuario
            cumUsuario.cus_id = Convert.ToInt32(usuario.cus_id);
            cumUsuario.cus_fecha_act = DateTime.Now;
            cumUsuario.cus_dni = Convert.ToString(usuario.cus_dni);
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
            var cumUsuarioTupla = cumUsuariobl.CumUsuarioEditarFirmaJson(cumUsuario);
            if (cumUsuarioTupla.error.Respuesta)
            {
                foreach (var preg in preguntas)
                {
                    CumUsuPreguntaEntidad pregunta = new CumUsuPreguntaEntidad();
                    List<CumUsuRespuestaEntidad> listaRespuesta = new List<CumUsuRespuestaEntidad>();
                    pregunta.upr_pregunta = preg.upr_pregunta;
                    pregunta.upr_dni = cumUsuario.cus_dni;
                    pregunta.upr_fecha_reg = DateTime.Now;
                    pregunta.upr_estado = "A";
                    pregunta.fk_pregunta = preg.fk_pregunta;
                    pregunta.fk_usuario = cumUsuario.cus_id;
                    pregunta.fk_envio = envio.env_id;
                    //Insertar Preguntas
                    var pregTupla = cumUsuPreguntabl.CumUsuPreguntaInsertarJson(pregunta);
                    if (pregTupla.error.Respuesta)
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
                            respuesta.fk_usuario = cumUsuario.cus_id;
                            respuesta.ure_estado = "A";
                            //Insertar Respuesta
                            var resTupla = cumUsuRespuestabl.CumUsuRespuestaInsertarJson(respuesta);
                            if (resTupla.error.Respuesta)
                            {
                                errormensaje = "Insertado";
                                response = true;
                                respuesta.ure_id = resTupla.idInsertado;
                            }
                            else
                            {
                                return Json(new { respuesta = false, mensaje = "Error" });
                            }
                       
                        }
                    
                    }
                    else
                    {
                        return Json(new { respuesta = false, mensaje = "Error" });
                    }
                  
                }
                //Editar Envios y Detalle
                //Editar envio y Detalle
                cumEnvioDetalle.end_id = envioDetalle.end_id;
                cumEnvioDetalle.end_dni = envioDetalle.end_dni;
                cumEnvioDetalle.end_correo_corp = envioDetalle.end_correo_corp;
                cumEnvioDetalle.end_correo_pers = envioDetalle.end_correo_pers;
                cumEnvioDetalle.end_fecha_act = ManejoNulos.ManageNullDate(envioDetalle.end_fecha_act);
                cumEnvioDetalle.end_fecha_reg = ManejoNulos.ManageNullDate(envioDetalle.end_fecha_reg);
                cumEnvioDetalle.end_estado = envioDetalle.end_estado;
                cumEnvioDetalle.fk_envio = envioDetalle.fk_envio;



                cumEnvio.env_id = envio.env_id;
                cumEnvio.env_nombre = envio.env_nombre;
                cumEnvio.env_tipo = envio.env_tipo;
                cumEnvio.env_fecha_act = ManejoNulos.ManageNullDate(envio.env_fecha_act);
                cumEnvio.env_fecha_reg = ManejoNulos.ManageNullDate(envio.env_fecha_reg);
                cumEnvio.env_estado = envio.env_estado;
                cumEnvio.fk_cuestionario = envio.fk_cuestionario;
                cumEnvio.fk_usuario = envio.fk_usuario;

                //cumEnvioDetalle.CumEnvio = envio;
                //cumEnvioDetalle.CumEnvio.env_estado = "2";
                //cumEnvioDetalle.CumEnvio.env_fecha_act = DateTime.Now;

                cumEnvio.env_estado = "2";
                cumEnvio.env_fecha_act = DateTime.Now;
                var envioTupla = cumEnviobl.CumEnvioEditarJson(cumEnvio);
                if (envioTupla.error.Respuesta)
                {
                    cumEnvioDetalle.end_estado = "2";
                    cumEnvioDetalle.end_fecha_act = DateTime.Now;
                    var envioDetalleTupla = cumEnvioDetbl.CumEnvioDetalleEditarJson(cumEnvioDetalle);
                    if (envioDetalleTupla.error.Respuesta)
                    {
                        response = true;
                        errormensaje = "Editado";
                    }
                    else
                    {
                        errormensaje = "No se pudo editar";
                    }
                }
                else
                {
                    errormensaje = "No se pudo Editar";
                }

            }
            else
            {
                return Json(new { respuesta = response, mensaje = "Error al Insertar" });
            }
            //Insertar Data


            return Json(new {respuesta=response,mensaje=errormensaje, data=cumUsuario });

        }
        [HttpPost]
        public ActionResult CumFichaEditarJson2(CumUsuarioEntidad usuario)
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
            if (usuarioTupla.error.Respuesta)
            {
                if (usuarioTupla.editado)
                {
                    //Editar Preguntas
                    foreach(var preg in cumUsuario.CumUsuPregunta)
                    {
                        preg.upr_fecha_act = DateTime.Now;
                        preg.fk_usuario = cumUsuario.cus_id;
                        var preguntaTupla = cumUsuPreguntabl.CumUsuPreguntaEditarJson(preg);
                        if (preguntaTupla.error.Respuesta){
                            if (preguntaTupla.editado)
                            {
                                //Editar Respuestas
                                foreach(var resp in preg.CumUsuRespuesta)
                                {
                                    resp.fk_usuario = cumUsuario.cus_id;
                                    resp.fk_usu_pregunta = preg.upr_id;
                                    resp.ure_dni = cumUsuario.cus_dni;
                                    var respuestaTupla = cumUsuRespuestabl.CumUsuRespuestaEditarJson(resp);
                                    if (respuestaTupla.error.Respuesta)
                                    {
                                        errormensaje = "Editado";
                                        response = true;
                                    }
                                    else
                                    {
                                        errormensaje = respuestaTupla.error.Mensaje;
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
                            errormensaje = preguntaTupla.error.Mensaje;
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
                errormensaje = usuarioTupla.error.Mensaje;
            }

            return Json(new { respuesta = response, mensaje = errormensaje, data = usuario });
        }
        [HttpPost]
        public ActionResult CumEnvioListarJson(int fk_usuario,string tipo)
        {
            string errormensaje = "";
            bool response = false;
            List<cum_envio> listaEnvio = new List<cum_envio>();
            try
            {
                var envioTupla = intranetFichabl.IntranetFichaPostListarxUsuarioJson(fk_usuario,tipo);
                if (envioTupla.error.Respuesta)
                {
                    listaEnvio = envioTupla.intranetFichaLista;
                    response = true;
                }
                else
                {
                    errormensaje = envioTupla.error.Mensaje;
                }
            }
            catch(Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { mensaje=errormensaje,respuesta=response,data=listaEnvio});

        }
        [HttpPost]
        public ActionResult CumUsuarioObtenerDataporEnvioJson(string codigo, string numdoc, int env_id)
        {
            var errormensaje = "";
            var response = false;
            CumUsuarioEntidad cumUsuario = new CumUsuarioEntidad();
            CumEnvioDetalleEntidad cumEnvioDet = new CumEnvioDetalleEntidad();
            CumEnvioEntidad cumEnvio = new CumEnvioEntidad();
            try
            {
                //buscar usuario con ese codigo y numdoc
                var usuarioTuplaClave = cumUsuariobl.CumUsuarioObtenerporNumDocyClave(numdoc, codigo);
                if (usuarioTuplaClave.error.Respuesta)
                {
                    //cumUsuario = usuarioTupla.cumUsuario;
                    if (usuarioTuplaClave.cumUsuario.cus_id != 0)
                    {
                        //Se encontro, buscar su data de acuerdo al tipo EMPLEADO o POSTULANTE
                        if (usuarioTuplaClave.cumUsuario.cus_tipo.ToUpper().Equals("POSTULANTE"))
                        {
                            //postgres
                            var usuarioTuplaPostgres = cumUsuariobl.CumUsuarioIdObtenerDataCompletaJson(usuarioTuplaClave.cumUsuario.cus_id);
                            if (usuarioTuplaPostgres.error.Respuesta)
                            {
                                cumUsuario = usuarioTuplaPostgres.cumUsuario;
                                cumUsuario.cus_firma_act = cumUsuario.cus_firma;
                            }
                            else
                            {
                                errormensaje = usuarioTuplaPostgres.error.Mensaje;
                            }
                        }
                        else if(usuarioTuplaClave.cumUsuario.cus_tipo.ToUpper().Equals("EMPLEADO"))
                        {
                            //sql
                            cumUsuario = usuarioTuplaClave.cumUsuario;
                            int mes_actual = DateTime.Now.Month;
                            int anio = DateTime.Now.Year;
                            var personaSQLTupla = sqlBL.PersonaSQLObtenerInformacionPuestoTrabajoJson(cumUsuario.cus_dni,mes_actual,anio);
                            if (personaSQLTupla.error.Respuesta)
                            {
                                PersonaSqlEntidad persona = new PersonaSqlEntidad(); 
                                if (personaSQLTupla.persona.CO_TRAB==null)
                                {
                                    if (mes_actual == 1)
                                    {
                                        mes_actual = 12;
                                        anio = anio - 1;
                                    }
                                    else
                                    {
                                        mes_actual = mes_actual - 1;
                                    }
                                    var personaSQLTupla2 = sqlBL.PersonaSQLObtenerInformacionPuestoTrabajoJson(cumUsuario.cus_dni, mes_actual,anio);
                                    if (personaSQLTupla2.error.Respuesta)
                                    {
                                        persona = personaSQLTupla2.persona;
                                    }
                                }
                                else
                                {
                                    persona = personaSQLTupla.persona;
                                }
                               
                                cumUsuario.nombre = persona.NO_TRAB;
                                cumUsuario.apellido_pat = persona.NO_APEL_PATE;
                                cumUsuario.apellido_mat = persona.NO_APEL_MATE;
                                cumUsuario.empresa = persona.DE_NOMB;
                                cumUsuario.sede = persona.DE_SEDE;
                                cumUsuario.celular = persona.NU_TLF1;
                                cumUsuario.direccion = persona.NO_DIRE_TRAB;
                                cumUsuario.ruc = persona.NU_RUCS;
                                cumUsuario.cus_firma_act = cumUsuario.cus_firma;
                            }
                            else
                            {
                                errormensaje = personaSQLTupla.error.Mensaje;
                            }
                        }
                        else
                        {
                            //error
                            errormensaje = "Ha ocurrido un Error";
                        }
                        //Listar Preguntas y Respuestas
                        var cumPreguntaTupla = cumUsuPreguntabl.CumUsuPreguntaListarxUsuarioJson(cumUsuario.cus_id, env_id);
                        if (cumPreguntaTupla.error.Respuesta)
                        {
                            List<CumUsuPreguntaEntidad> listaPreguntas = new List<CumUsuPreguntaEntidad>();
                            foreach (var preg in cumPreguntaTupla.lista)
                            {
                                CumUsuPreguntaEntidad pregunta = new CumUsuPreguntaEntidad();
                                pregunta = preg;
                                var cumRespuestaTupla = cumUsuRespuestabl.CumUsuRespuestaListarxUsuPreguntaJson(pregunta.upr_id);
                                if (cumRespuestaTupla.error.Respuesta)
                                {
                                    pregunta.CumUsuRespuesta = cumRespuestaTupla.lista;
                                }
                                else
                                {
                                    errormensaje = cumRespuestaTupla.error.Mensaje;
                                }
                                listaPreguntas.Add(pregunta);
                            }
                            cumUsuario.CumUsuPregunta = listaPreguntas;
                         

                        }
                        else
                        {
                            errormensaje = cumPreguntaTupla.error.Mensaje;
                        }
                        //Obtener Envio

                        var envioDetTupla = envDetModelbl.CumEnvioDetalleObtenerxEnvioJson(env_id);
                        if (envioDetTupla.error.Respuesta)
                        {
                            cumEnvioDet = envioDetTupla.cumEnvioDet;
                            //llenar Envio;
                            var envioTupla = cumEnviobl.CumEnvioIdObtenerJson(env_id);
                            if (envioTupla.error.Respuesta)
                            {
                                cumEnvio = envioTupla.cumEnvio;
                                if (cumEnvio.env_id != 0)
                                {
                                    cumEnvio.CumUsuario = cumUsuario;
                                    cumEnvioDet.CumEnvio = cumEnvio;
                                    response = true;
                                    errormensaje = "Listando Data";
                                }
                                else
                                {
                                    errormensaje = "Error al Listar Data";
                                }
                            }
                            else
                            {
                                errormensaje = envioTupla.error.Mensaje;
                            }
                        }
                        else
                        {
                            errormensaje = envioDetTupla.error.Mensaje;
                        }
                    }
                    else
                    {
                        errormensaje = "Datos Incorrectos";
                    }
                }
                else
                {
                    errormensaje = usuarioTuplaClave.error.Mensaje;
                }
           
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { mensaje=errormensaje,respuesta=response, data= cumEnvioDet });
        }
        [HttpPost] public ActionResult CumFichaEditarJson()
        {
            string errormensaje = "";
            bool response = false;
            HttpPostedFileBase file = Request.Files["file"];
            string request = Request.Params["usuario"];
            string extension = "";
            string rutaInsertar = "";
            string rutaAnterior = "";
  
            int tamanioMaximo = 4194304;
            string direccion = Server.MapPath("/") + Request.ApplicationPath + "/CumplimientoFiles/CumUsuario";
            dynamic jsonObj = JsonConvert.DeserializeObject(request);
            CumUsuarioEntidad cumUsuario = new CumUsuarioEntidad();
            List<CumUsuPreguntaEntidad> listaPreguntas = new List<CumUsuPreguntaEntidad>();
            CumUsuPreguntaEntidad cumUsuPregunta = new CumUsuPreguntaEntidad();
            CumUsuRespuestaEntidad cumUsuRespuesta = new CumUsuRespuestaEntidad();
            CumEnvioDetalleEntidad cumEnvioDetalle = new CumEnvioDetalleEntidad();
            CumEnvioEntidad cumEnvio = new CumEnvioEntidad();
            //Insertar Usuario
            var usuario = jsonObj.CumEnvio.CumUsuario;
            var preguntas = usuario.CumUsuPregunta;
            var envioDetalle = jsonObj;
            var envio = jsonObj.CumEnvio;
            try
            {
                if (file != null)
                {
                    if (file.ContentLength <= tamanioMaximo)
                    {
                        extension = Path.GetExtension(file.FileName);
                        if (extension == ".pdf" || extension == ".jpg" || extension == ".png" || extension == ".jpeg")
                        {
                            var nombreArchivo = (usuario.cus_dni.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension);
                            rutaInsertar = Path.Combine(direccion, nombreArchivo);
                            rutaAnterior = Path.Combine(direccion, usuario.cus_firma_act.ToString());
                            if (!Directory.Exists(direccion))
                            {
                                System.IO.Directory.CreateDirectory(direccion);
                            }
                            if (System.IO.File.Exists(rutaAnterior))
                            {
                                System.IO.File.Delete(rutaAnterior);
                            }
                            file.SaveAs(rutaInsertar);
                            cumUsuario.cus_firma = nombreArchivo;
                            cumUsuario.cus_fecha_act = DateTime.Now;
                            cumUsuario.cus_id = usuario.cus_id;
                            var usuarioEdicionTupla = cumUsuariobl.CumUsuarioEditarFirmaJson(cumUsuario);
                            if (!usuarioEdicionTupla.error.Respuesta)
                            {
                                errormensaje = "Error al Editar Firma Digital";
                                return Json(new { respuesta = response, mensaje = errormensaje });
                            }
                       
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
                //Editar Preguntas
                foreach(var preg in preguntas)
                {
                    cumUsuPregunta.upr_id = preg.upr_id;
                    cumUsuPregunta.upr_pregunta = preg.upr_pregunta;
                    cumUsuPregunta.upr_fecha_act = DateTime.Now;
                    cumUsuPregunta.fk_pregunta = preg.fk_pregunta;

                    var preguntaTupla = cumUsuPreguntabl.CumUsuPreguntaEditarJson(cumUsuPregunta);
                    if (preguntaTupla.error.Respuesta)
                    {
                        foreach (var resp in preg.CumUsuRespuesta)
                        {
                            cumUsuRespuesta.ure_id =resp.ure_id;
                            cumUsuRespuesta.ure_respuesta =resp.ure_respuesta;
                            cumUsuRespuesta.ure_tipo =resp.ure_tipo;
                            cumUsuRespuesta.ure_orden =resp.ure_orden;
                            cumUsuRespuesta.ure_estado = "A";
                            var respuestaTupla = cumUsuRespuestabl.CumUsuRespuestaEditarJson(cumUsuRespuesta);
                            if (respuestaTupla.error.Respuesta)
                            {
                                errormensaje = "Editado";
                                response = true;
                            }
                            else
                            {
                                errormensaje = respuestaTupla.error.Mensaje;
                            }
                        }
                    }
                }
                //Editar envio y Detalle
                cumEnvioDetalle.end_id = envioDetalle.end_id;
                cumEnvioDetalle.end_dni = envioDetalle.end_dni;
                cumEnvioDetalle.end_correo_corp = envioDetalle.end_correo_corp;
                cumEnvioDetalle.end_correo_pers = envioDetalle.end_correo_pers;
                cumEnvioDetalle.end_fecha_act = ManejoNulos.ManageNullDate(envioDetalle.end_fecha_act);
                cumEnvioDetalle.end_fecha_reg = ManejoNulos.ManageNullDate(envioDetalle.end_fecha_reg);
                cumEnvioDetalle.end_estado = envioDetalle.end_estado;
                cumEnvioDetalle.fk_envio = envioDetalle.fk_envio;



                cumEnvio.env_id = envio.env_id;
                cumEnvio.env_nombre = envio.env_nombre;
                cumEnvio.env_tipo = envio.env_tipo;
                cumEnvio.env_fecha_act = ManejoNulos.ManageNullDate(envio.env_fecha_act);
                cumEnvio.env_fecha_reg = ManejoNulos.ManageNullDate(envio.env_fecha_reg);
                cumEnvio.env_estado = envio.env_estado;
                cumEnvio.fk_cuestionario = envio.fk_cuestionario;
                cumEnvio.fk_usuario = envio.fk_usuario;

                //cumEnvioDetalle.CumEnvio = envio;
                //cumEnvioDetalle.CumEnvio.env_estado = "2";
                //cumEnvioDetalle.CumEnvio.env_fecha_act = DateTime.Now;

                cumEnvio.env_estado = "2";
                cumEnvio.env_fecha_act = DateTime.Now;
                var envioTupla = cumEnviobl.CumEnvioEditarJson(cumEnvio);
                if (envioTupla.error.Respuesta)
                {
                    cumEnvioDetalle.end_estado = "2";
                    cumEnvioDetalle.end_fecha_act = DateTime.Now;
                    var envioDetalleTupla = cumEnvioDetbl.CumEnvioDetalleEditarJson(cumEnvioDetalle);
                    if (envioDetalleTupla.error.Respuesta)
                    {
                        response = true;
                        errormensaje = "Editado";
                    }
                    else
                    {
                        errormensaje = "No se pudo editar";
                    }
                }
                else
                {
                    errormensaje = "No se pudo Editar";
                }

            }
            catch(Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new {mensaje=errormensaje,respuesta=response});
        }

        [HttpPost]
        public ActionResult CumUsuarioObtenerFichaReporteJson(int env_id)
        {
            var errormensaje = "";
            var response = false;
            CumUsuarioEntidad cumUsuario = new CumUsuarioEntidad();
            CumEnvioDetalleEntidad cumEnvioDet = new CumEnvioDetalleEntidad();
            CumEnvioEntidad cumEnvio = new CumEnvioEntidad();
            try
            {
                //Obtener Envio

                var envioDetTupla = envDetModelbl.CumEnvioDetalleObtenerxEnvioJson(env_id);
                if (envioDetTupla.error.Respuesta)
                {
                    cumEnvioDet = envioDetTupla.cumEnvioDet;
                    //llenar Envio;
                    var envioTupla = cumEnviobl.CumEnvioIdObtenerJson(env_id);
                    if (envioTupla.error.Respuesta)
                    {
                        cumEnvio = envioTupla.cumEnvio;
                        if (cumEnvio.env_id != 0)
                        {
                            //cumEnvio.CumUsuario = cumUsuario;
                            cumEnvioDet.CumEnvio = cumEnvio;
                            response = true;
                            errormensaje = "Listando Data";
                        }
                        else
                        {
                            errormensaje = "Error al Listar Data";
                        }
                    }
                    else
                    {
                        errormensaje = envioTupla.error.Mensaje;
                    }
                }
                else
                {
                    errormensaje = envioDetTupla.error.Mensaje;
                }

                //obtener usuario por fk_envio
                var usuarioTuplaClave = cumUsuariobl.CumUsuarioIdObtenerJson(cumEnvio.fk_usuario);
                if (usuarioTuplaClave.error.Respuesta)
                {
                    //cumUsuario = usuarioTupla.cumUsuario;
                    if (usuarioTuplaClave.cumUsuario.cus_id != 0)
                    {
                        //Se encontro, buscar su data de acuerdo al tipo EMPLEADO o POSTULANTE
                        if (usuarioTuplaClave.cumUsuario.cus_tipo.ToUpper().Equals("POSTULANTE"))
                        {
                            //postgres
                            var usuarioTuplaPostgres = cumUsuariobl.CumUsuarioIdObtenerDataCompletaJson(usuarioTuplaClave.cumUsuario.cus_id);
                            if (usuarioTuplaPostgres.error.Respuesta)
                            {
                                cumUsuario = usuarioTuplaPostgres.cumUsuario;
                            }
                            else
                            {
                                errormensaje = usuarioTuplaPostgres.error.Mensaje;
                            }
                        }
                        else if (usuarioTuplaClave.cumUsuario.cus_tipo.ToUpper().Equals("EMPLEADO"))
                        {
                            //sql
                            cumUsuario = usuarioTuplaClave.cumUsuario;
                            int mes_actual = DateTime.Now.Month;
                            int anio = DateTime.Now.Year;
                            var personaSQLTupla = sqlBL.PersonaSQLObtenerInformacionPuestoTrabajoJson(cumUsuario.cus_dni,mes_actual,anio);
                            if (personaSQLTupla.error.Respuesta)
                            {
                                //PersonaSqlEntidad persona = personaSQLTupla.persona;
                                PersonaSqlEntidad persona = new PersonaSqlEntidad();
                                if (personaSQLTupla.persona.CO_TRAB==null)
                                {
                                    if (mes_actual == 1)
                                    {
                                        mes_actual = 12;
                                        anio = anio - 1;
                                    }
                                    else
                                    {
                                        mes_actual = mes_actual - 1;
                                    }
                                    var personaSQLTupla2 = sqlBL.PersonaSQLObtenerInformacionPuestoTrabajoJson(cumUsuario.cus_dni, mes_actual,anio);
                                    if (personaSQLTupla2.error.Respuesta)
                                    {
                                        persona = personaSQLTupla2.persona;
                                    }
                                }
                                else
                                {
                                    persona = personaSQLTupla.persona;
                                }
                                cumUsuario.nombre = persona.NO_TRAB;
                                cumUsuario.apellido_pat = persona.NO_APEL_PATE;
                                cumUsuario.apellido_mat = persona.NO_APEL_MATE;
                                cumUsuario.empresa = persona.DE_NOMB;
                                cumUsuario.sede = persona.DE_SEDE;
                                cumUsuario.celular = persona.NU_TLF1;
                                cumUsuario.direccion = persona.NO_DIRE_TRAB;
                                cumUsuario.ruc = persona.NU_RUCS;
                            }
                            else
                            {
                                errormensaje = personaSQLTupla.error.Mensaje;
                            }
                        }
                        else
                        {
                            //error
                            errormensaje = "Ha ocurrido un Error";
                        }
                        //Listar Preguntas y Respuestas
                        var cumPreguntaTupla = cumUsuPreguntabl.CumUsuPreguntaListarxUsuarioJson(cumUsuario.cus_id, env_id);
                        if (cumPreguntaTupla.error.Respuesta)
                        {
                            List<CumUsuPreguntaEntidad> listaPreguntas = new List<CumUsuPreguntaEntidad>();
                            foreach (var preg in cumPreguntaTupla.lista)
                            {
                                CumUsuPreguntaEntidad pregunta = new CumUsuPreguntaEntidad();
                                pregunta = preg;
                                var cumRespuestaTupla = cumUsuRespuestabl.CumUsuRespuestaListarxUsuPreguntaJson(pregunta.upr_id);
                                if (cumRespuestaTupla.error.Respuesta)
                                {
                                    pregunta.CumUsuRespuesta = cumRespuestaTupla.lista;
                                }
                                else
                                {
                                    errormensaje = cumRespuestaTupla.error.Mensaje;
                                }
                                listaPreguntas.Add(pregunta);
                            }
                            cumUsuario.CumUsuPregunta = listaPreguntas;


                        }
                        else
                        {
                            errormensaje = cumPreguntaTupla.error.Mensaje;
                        }
                        cumEnvioDet.CumEnvio.CumUsuario = cumUsuario;
                    }
                    else
                    {
                        errormensaje = "Datos Incorrectos";
                    }
                }
                else
                {
                    errormensaje = usuarioTuplaClave.error.Mensaje;
                }


            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { mensaje = errormensaje, respuesta = response, data = cumEnvioDet });
        }
        [HttpPost]
        public ActionResult CumUsuarioObtenerEnvioxIdJson(int env_id)
        {
            bool response = false;
            string errormensaje = "";
            CumEnvioEntidad envio = new CumEnvioEntidad();
            try
            {
                var envioTupla = cumEnviobl.CumEnvioIdObtenerJson(env_id);
                if (envioTupla.error.Respuesta)
                {
                    envio = envioTupla.cumEnvio;
                    response = true;
                    errormensaje = "Mostrando observación";
                }
                else
                {
                    errormensaje = envioTupla.error.Mensaje;
                }

            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { respuesta = response, mensaje = errormensaje, data=envio });
        }
        [HttpPost]
        public ActionResult CumUsuarioEditarObservacionEnvioJson(CumEnvioEntidad envio)
        {
            bool response = false;
            string errormensaje = "";
            try
            {
                var envioTupla = cumEnviobl.CumEnvioEditarObservacionJson(envio);
                if (envioTupla.error.Respuesta)
                {
                    response = true;
                    errormensaje = "Editado";
                }
                else
                {
                    errormensaje = envioTupla.error.Mensaje;
                }

            }catch(Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new { respuesta=response,mensaje=errormensaje });
        }
    }
}