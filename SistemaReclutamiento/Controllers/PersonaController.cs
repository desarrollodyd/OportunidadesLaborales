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
        public ActionResult Index()
        {
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
                persona.personaNroDocumento = datos.personaNroDocumento;
                persona.personaNombre = datos.personaNombre;
                persona.personaApellidoPaterno = datos.personaApellidoPaterno;
                persona.personaApellidoMaterno = datos.personaApellidoMaterno;
                persona.personaEmail = datos.personaEmail;
                persona.personaEstado = 1;
                persona.tipoDocumentoId = datos.tipoDocumentoId;
                try {
                    //Revisar que no hayan personas con el CAMPO Email o DNI iguales dentro de la Base de Datos
                    var personaRepetida = personabl.PersonaDniEmailObtenerJson(persona.personaEmail, persona.personaNroDocumento);
                    if ( personaRepetida.personaNroDocumento!=persona.personaNroDocumento)
                    {
                        if ( personaRepetida.personaEmail!=persona.personaEmail)
                        {
                            //Insertando persona
                            respuestaPersonaInsertada = personabl.PersonaInsertarJson(persona);
                        }
                        else
                        {
                            return Json(new { respuesta = respuestaConsulta, mensaje = "El Email : " + personaRepetida.personaEmail + " ya se encuentra Registrado" });
                        }
                    }
                    else {
                        return Json(new { respuesta = respuestaConsulta, mensaje = "El Nro de Documento : " + personaRepetida.personaNroDocumento+ " ya se encuentra registrado" });
                    }
                    
                }
                catch (Exception ex) {
                    errormensaje = ex.Message;
                }
                if (respuestaPersonaInsertada != 0) {
                    usuario.usuarioContrasenia = Seguridad.Encriptar(datos.usuarioContrasenia);
                    usuario.usuarioEmail = datos.personaEmail;
                    usuario.personaId = respuestaPersonaInsertada;
                    usuario.usuarioValidado = 0;
                    usuario.usuarioEstado = 1;
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
        [HttpPost]
        public ActionResult TipoDocumentoListarJson()
        {
            var errormensaje = "";
            var listaTipoDocumento = new List<tipoDocumentoEntidad>();
            try
            {
                listaTipoDocumento = tipoDocumentobl.tipoDocumentoListarJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaTipoDocumento.ToList(), mensaje = errormensaje });
        }
    }
}