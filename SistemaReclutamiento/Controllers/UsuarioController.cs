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
    public class UsuarioController : Controller
    {
        personaModel personabl = new personaModel();
        usuarioModel usuariobl = new usuarioModel();
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UsuarioInsertarJson(usuarioPersonaEntidad datos)
        {
            var errormensaje = "";
            usuarioEntidad usuario = new usuarioEntidad();
            personaEntidad persona = new personaEntidad();
            int respuestaPersonaInsertada = 0;
            bool respuestaConsulta = false;
            try
            {
                //Insertando persona
                persona.personaDni = datos.personaDni;
                persona.personaNombre = datos.personaNombre;
                persona.personaApellidoPaterno = datos.personaApellidoPaterno;
                persona.personaApellidoMaterno = datos.personaApellidoMaterno;
                persona.personaEmail = datos.personaEmail;
                persona.personaEstado = 1;
                try {
                    respuestaPersonaInsertada = personabl.PersonaInsertarJson(persona);
                }
                catch (Exception ex) {

                }
                if (respuestaPersonaInsertada != 0) {
                    usuario.usuarioContrasenia = Seguridad.Encriptar(datos.usuarioContrasenia);
                    usuario.usuarioEmail = datos.personaEmail;
                    usuario.personaId = respuestaPersonaInsertada;
                    usuario.usuarioValidado = 0;
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
    }
}