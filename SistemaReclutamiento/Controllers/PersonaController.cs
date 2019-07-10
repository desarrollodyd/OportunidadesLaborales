using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
using SistemaReclutamiento.Entidades;

namespace SistemaReclutamiento.Controllers
{
    public class PersonaController : Controller
    {
        personaModel personabl = new personaModel();
        usuarioModel usuariobl = new usuarioModel();
        tipoDocumentoModel tipoDocumentobl = new tipoDocumentoModel();
        // GET: Usuario
        public ActionResult PersonaIndexVista()
        {
            return View();
        }
        public ActionResult PersonaEditarVista(string id)
        {
            var errormensaje = "";
            int idPersona = Convert.ToInt32(Seguridad.Desencriptar(id));
            var persona = new personaEntidad();
            try
            {
                persona = personabl.PersonaIdObtenerJson(idPersona);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            ViewBag.Persona = persona;
            ViewBag.errormensaje = errormensaje;
            return View();
        }
        [HttpPost]
        public ActionResult PersonaInsertarJson(usuarioPersonaEntidad datos)
        {
            var errormensaje = "";
            usuarioEntidad usuario = new usuarioEntidad();
            personaEntidad persona = new personaEntidad();
            int respuestaPersonaInsertada = 0;
            bool respuestaConsulta = false;
            try
            {               
                persona.per_numdoc = datos.per_numdoc;
                persona.per_nombre = datos.per_nombre;
                persona.per_apellido_pat = datos.per_apellido_pat;
                persona.per_apellido_mat = datos.per_apellido_mat;
                persona.per_correoelectronico = datos.per_correoelectronico;
                persona.per_estado = "P";
                persona.per_tipodoc = datos.per_tipodoc;
                persona.fk_ubigeo = 1305;
                try {
                    //Revisar que no hayan personas con el CAMPO Email o DNI iguales dentro de la Base de Datos
                    var personaRepetida = personabl.PersonaDniEmailObtenerJson(persona.per_correoelectronico, persona.per_numdoc);
                    if ( personaRepetida.per_numdoc!=persona.per_numdoc)
                    {
                        if ( personaRepetida.per_correoelectronico!=persona.per_correoelectronico)
                        {
                            //Insertando persona
                            respuestaPersonaInsertada = personabl.PersonaInsertarJson(persona);
                        }
                        else
                        {
                            return Json(new { respuesta = respuestaConsulta, mensaje = "El Email : " + personaRepetida.per_correoelectronico + " ya se encuentra Registrado" });
                        }
                    }
                    else {
                        return Json(new { respuesta = respuestaConsulta, mensaje = "El Nro de Documento : " + personaRepetida.per_numdoc+ " ya se encuentra registrado" });
                    }
                    
                }
                catch (Exception ex) {
                    errormensaje = ex.Message;
                }
                if (respuestaPersonaInsertada != 0) {
                    usuario.usu_contrasenia = Seguridad.EncriptarSHA512(datos.usu_contrasenia);
                    usuario.usu_nombre = datos.per_correoelectronico;
                    usuario.fk_persona = respuestaPersonaInsertada;                    
                    usuario.usu_estado = "A";
                    //usuario.usuarioFechaCreacion = DateTime.Now;
                    respuestaConsulta = usuariobl.UsuarioInsertarJson(usuario);
                }
                
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            if (respuestaConsulta) {
                /*LOGICA PARA ENVIO DE CORREO DE CONFIRMACION*/
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        public ActionResult PersonaEditarJson(personaEntidad persona)
        {
            var errormensaje = "";
            bool respuestaConsulta = true;
            try
            {
                respuestaConsulta = personabl.PersonaEditarJson(persona);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }


        //[HttpPost]
        //public ActionResult TipoDocumentoListarJson()
        //{
        //    var errormensaje = "";
        //    var listaTipoDocumento = new List<tipoDocumentoEntidad>();
        //    try
        //    {
        //        listaTipoDocumento = tipoDocumentobl.tipoDocumentoListarJson();
        //    }
        //    catch (Exception exp)
        //    {
        //        errormensaje = exp.Message + ",Llame Administrador";
        //    }
        //    return Json(new { data = listaTipoDocumento.ToList(), mensaje = errormensaje });
        //}
    }
}