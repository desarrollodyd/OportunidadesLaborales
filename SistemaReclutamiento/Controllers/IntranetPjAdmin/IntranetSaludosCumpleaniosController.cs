using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    public class IntranetSaludosCumpleaniosController : Controller
    {
        // GET: IntranetSaludosCumpleanios
        IntranetSaludoCumpleaniosModel intranetSaludoCumpleaniobl = new IntranetSaludoCumpleaniosModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetSaludoCumpleanioListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetSaludoCumpleanioEntidad> listaComentarios = new List<IntranetSaludoCumpleanioEntidad>();
            claseError error = new claseError();
            try
            {
                var saludoTupla = intranetSaludoCumpleaniobl.IntranetSaludoCumpleanioListarJson();
                error = saludoTupla.error;
                listaComentarios = saludoTupla.intranetSaludoCumpleanioLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Comentarios";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar los Comentarios";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaComentarios.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetSaludoCumpleanioEditarJson(IntranetSaludoCumpleanioEntidad intranetSaludoCumpleanio)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            claseError error = new claseError();
            try
            {
                var saludoTupla = intranetSaludoCumpleaniobl.IntranetSaludoCumpleanioEditarJson(intranetSaludoCumpleanio);
                error = saludoTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = saludoTupla.intranetSaludoCumpleanioEditado;
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
        public ActionResult IntranetSaludoCumpleanioInsertarJson(IntranetSaludoCumpleanioEntidad intranetSaludoCumpleanio)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            int intranetSaludoCumpleaniosInsertado=0;
            claseError error = new claseError();
            Correo correo_enviar = new Correo();
            string asunto_correo = "";
            string cuerpo_correo = "";
            PersonaEntidad persona = (PersonaEntidad)Session["perIntranet_full"]; ;
            try
            {
                intranetSaludoCumpleanio.sld_fecha_envio = DateTime.Now;
                intranetSaludoCumpleanio.sld_estado = "A";
                var saludoTupla = intranetSaludoCumpleaniobl.IntranetSaludoCumpleanioInsertarJson(intranetSaludoCumpleanio);
                error = saludoTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    intranetSaludoCumpleaniosInsertado = saludoTupla.idIntranetSaludoCumpleanioInsertado;
                    respuestaConsulta = true;
                    errormensaje = "Se Guardó Correctamente";
                    try
                    {
                        //envio de correo
                        asunto_correo = persona.per_nombre.ToUpper() +" "+
                        persona.per_apellido_pat.ToUpper() +" "+ persona.per_apellido_mat.ToUpper() +
                        " te desea un Feliz Cumpleaños";
                        cuerpo_correo = intranetSaludoCumpleanio.sld_cuerpo;
                        correo_enviar.EnviarCorreo(
                            "s3kzimbra@gmail.com",
                            asunto_correo,
                            cuerpo_correo
                            );
                    }
                    catch (Exception ex) {
                        errormensaje = ex.Message;
                    }
                }
                else
                {
                    mensajeConsola = error.Value;
                    errormensaje = "Error, no se Puede Insertar";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetSaludoCumpleanioEliminarJson(int sld_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            claseError error = new claseError();
            string mensajeConsola = "";
            try
            {
                var saludoTupla = intranetSaludoCumpleaniobl.IntranetSaludoCumpleanioEliminarJson(sld_id);
                error = saludoTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = saludoTupla.intranetSaludoCumpleanioEliminado;
                    errormensaje = "Saludo Eliminado";
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
        public ActionResult IntranetMenuSaludoCumpleanioVariosJson(int[] listaComentariosEliminar) {
            string errormensaje = "";
            string mensajeConsola = "";
            bool respuestaConsulta = false;
            claseError error = new claseError();
            try
            {
                for (int i = 0; i <= listaComentariosEliminar.Length - 1; i++)
                {
                    var saludoTupla = intranetSaludoCumpleaniobl.IntranetSaludoCumpleanioEliminarJson(listaComentariosEliminar[i]);
                    error = saludoTupla.error;
                    if (error.Key.Equals(string.Empty))
                    {
                        respuestaConsulta = saludoTupla.intranetSaludoCumpleanioEliminado;
                        errormensaje = "Comentarios Eliminados";
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
                errormensaje = "Error, no se Puede Eliminar, "+ex.Message;
                respuestaConsulta = false;
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
    }
}