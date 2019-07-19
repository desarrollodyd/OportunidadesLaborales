using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;

namespace SistemaReclutamiento.Controllers
{
    public class LoginController : Controller
    {
        usuarioModel usuariobl = new usuarioModel();
        personaModel personabl = new personaModel();
        ubigeoModel ubigeobl = new ubigeoModel();
        postulanteModel postulantebl = new postulanteModel();
        
        public ActionResult Index(string id)
        {
            var usuario = new usuarioEntidad();
            var errormensaje = "";
            if (id != "" || id != null) {
                try {
                    usuario = usuariobl.UsuarioObtenerTokenJson(id);
                }
                catch(Exception ex)
                {
                    errormensaje = ex.Message + "token invalido";
                }
                ViewBag.Usuario = usuario;
                ViewBag.ErrorMessage = errormensaje;
            }
            return View();

        }
        public ActionResult ValidarUsuarioIndex() {
            ViewBag.Usuario = "usuario";
            return View();
        }
        [HttpPost]
        public ActionResult CambiarPasswordUsuario(string usu_password, string usu_id) {
            bool respuesta = false;
            string mensaje = "";
            var usuario = new usuarioEntidad();
            usuario.usu_id = Convert.ToInt32(Seguridad.Desencriptar(usu_id));
            string password_encriptado = Seguridad.EncriptarSHA512(usu_password);
            try {
                respuesta = usuariobl.UsuarioEditarEstadoJson(usuario.usu_id, password_encriptado);
            }
            catch (Exception ex) {
                mensaje = ex.Message + "";
            }
            return Json(new { respuesta=respuesta,mensaje=mensaje});
        }
        [HttpPost]
        public ActionResult ValidarLoginJson(string usu_login, string usu_password, string usu_clave_temp)
        {
            bool respuesta = false;
            string mensaje = "";
            var usuario = new usuarioEntidad();        
            var persona = new personaEntidad();          
            try
            {
                usuario = usuariobl.ValidarCredenciales(usu_login);
                if (usuario.usu_id > 0)
                {
                    if (usuario.usu_estado=="P")
                    {
                        if (usuario.usu_contrasenia == usu_password.Trim())
                        {
                            mensaje = "*";                                            
                            Session["usu_full"] = usuario;
                            Session["usu_nombre"] = usuario.usu_nombre;
                            Session["usu_id"] = usuario.usu_id;
                            Session["fk_persona"] = usuario.fk_persona;
                            Session["per_full"] = personabl.PersonaIdObtenerJson(usuario.fk_persona);
                            persona = personabl.PersonaIdObtenerJson(usuario.fk_persona);
                            Session["ubigeo"] = ubigeobl.UbigeoObtenerDatosporIdJson(persona.fk_ubigeo);
                            Session["postulante"] = postulantebl.PostulanteIdObtenerporPersonaJson(persona.per_id);                         
                        }
                        else {
                            mensaje = "La contraseña ingresada es erronea";
                        }
                        
                        
                    }                      
                    else
                    {
                        string password_encriptado = Seguridad.EncriptarSHA512(usu_password.Trim());
                        if (usuario.usu_contrasenia == password_encriptado)
                        {
                            Session["usu_id"] = usuario.usu_id;
                            Session["usu_nombre"] = usuario.usu_nombre;
                            Session["usu_full"] = usuario;
                            Session["per_full"] = personabl.PersonaIdObtenerJson(usuario.fk_persona);
                            persona = personabl.PersonaIdObtenerJson(usuario.fk_persona);                         
                            Session["ubigeo"] = ubigeobl.UbigeoObtenerDatosporIdJson(persona.fk_ubigeo);
                            Session["postulante"] = postulantebl.PostulanteIdObtenerporPersonaJson(persona.per_id);
                            Session["fk_persona"] = usuario.fk_persona;
                            respuesta = true;
                            mensaje = "Bienvenido, " + usuario.usu_nombre;
                        }
                        else
                        {
                            mensaje = "La contraseña ingresada es erronea";
                        }
                    }
                }
                else
                {
                    mensaje = "No se ha encontrando el usuario ingresado";
                }
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + "";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje/*, usuario=usuario*/ });
        }
        [HttpPost]
        public ActionResult CerrarSesionLoginJson()
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                Session["usu_id"] = null;
                Session["usu_nombre"] = null;
                Session["usu_full"] = null;
                respuestaConsulta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}