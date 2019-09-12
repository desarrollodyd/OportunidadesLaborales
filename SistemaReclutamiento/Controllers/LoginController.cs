using System;
using System.Collections.Generic;
using System.Configuration;
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
        UsuarioModel usuariobl = new UsuarioModel();
        PersonaModel personabl = new PersonaModel();
        UbigeoModel ubigeobl = new UbigeoModel();
        PostulanteModel postulantebl = new PostulanteModel();
        ConfiguracionModel configuracionbl = new ConfiguracionModel();
        #region Region Postulante
        public ActionResult PostulanteIndex()
        {
            if (Session["usu_full"] != null)
            {
                ViewBag.Message = "Bienvenido.";
                return View("~/Views/Persona/DatosPersonaVista.cshtml");
            }
            else
            {
                ViewBag.Message = "Login De Acceso";
                return View();
            }        
        }

        public ActionResult PostulanteActivacion(string id) {
            var usuario = new UsuarioEntidad();
            var datoPendiente = new List<dynamic>();
            var errormensaje = "";
            bool respuestaConsulta = false;
           
            if (id != "" || id != null)
            {
                try
                {
                    usuario = usuariobl.PostulanteUsuarioObtenerTokenJson(id);
                    if (usuario.usu_id != 0)
                    {
                        datoPendiente.Add(new { usuarioID = Seguridad.Encriptar(usuario.usu_id.ToString()), contrasenia = Seguridad.Encriptar(usuario.usu_contrasenia) });
                        respuestaConsulta = true;
                    }
                    else
                    {
                        errormensaje = "Ya completo su Registro";
                    }
                }
                catch (Exception ex)
                {
                    errormensaje = ex.Message + "token invalido";
                }
                ViewBag.respuesta = respuestaConsulta;
                ViewBag.data = datoPendiente;
                ViewBag.mensaje = errormensaje;
            }
            else
            {
                ViewBag.ErrorMessage = "No se envio la Clave";
            }
            ViewBag.Usuario = "usuario";
            return View("~/Views/Login/ValidarUsuarioIndex.cshtml");
        }
   
        [HttpPost]
        public ActionResult PostulanteCambiarPasswordUsuario(string usu_password, string usu_id) {
            //string ruta = "";
            string nemonic = "RUTA_FOTO_POSTULANTE";
            bool respuestaConsulta = false;
            string errormensaje = "";
            var usuario = new UsuarioEntidad();
            var persona = new PersonaEntidad();
            var postulante = new PostulanteEntidad();
            var configuracion = configuracionbl.ConfiguracionObtenerporNemonicJson(nemonic);
            RutaImagenes rutaImagenes = new RutaImagenes();
            usuario.usu_id = Convert.ToInt32(Seguridad.Desencriptar(usu_id));
            string password_encriptado = Seguridad.EncriptarSHA512(usu_password);
            try {
                respuestaConsulta = usuariobl.PostulanteUsuarioEditarEstadoJson(usuario.usu_id, password_encriptado);
                var usuarioData = usuariobl.UsuarioObtenerxID(usuario.usu_id);
                Session["usu_full"] = usuarioData;
                Session["per_full"] = personabl.PersonaIdObtenerJson(usuarioData.fk_persona);
                persona = personabl.PersonaIdObtenerJson(usuarioData.fk_persona);
                Session["ubigeo"] = ubigeobl.UbigeoObtenerDatosporIdJson(persona.fk_ubigeo);
                postulante = postulantebl.PostulanteIdObtenerporUsuarioJson(usuarioData.usu_id);
                Session["postulante"] = postulante;
                rutaImagenes.imagenPostulante_CV(configuracion.config_nombre,postulante.pos_foto);
                errormensaje = "Contraseña actualizada correctamente";
            }
            catch (Exception ex) {
                errormensaje = ex.Message + "";
            }
            return Json(new { respuesta= respuestaConsulta, mensaje= errormensaje });
        }
      
        [HttpPost]
        public ActionResult PostulanteValidarLoginJson(string usu_login, string usu_password)
        {
            bool respuesta = false;
            string nemonic = "RUTA_FOTO_POSTULANTE";
            string errormensaje = "";
            var usuario = new UsuarioEntidad();        
            var persona = new PersonaEntidad();
            var postulante = new PostulanteEntidad();
            var configuracion = configuracionbl.ConfiguracionObtenerporNemonicJson(nemonic);
            RutaImagenes rutaImagenes = new RutaImagenes();
            string pendiente = "";
            try
            {
                usuario = usuariobl.PostulanteValidarCredenciales(usu_login.ToLower());
                if (usuario.usu_id > 0)
                {
                    if (usuario.usu_estado=="P")
                    {
                        if (usuario.usu_contrasenia == Seguridad.EncriptarSHA512(usu_password.Trim()))
                        {
                            pendiente = usuario.usu_clave_temp;
                            errormensaje = "Usuario Pendiente de Activacion";                                            
                        }
                        else {
                            errormensaje = "La contraseña ingresada es erronea";
                        }                       
                    }                      
                    else
                    {
                        string password_encriptado = Seguridad.EncriptarSHA512(usu_password.Trim());
                        if (usuario.usu_contrasenia == password_encriptado)
                        {
                            Session["usu_full"] = usuariobl.UsuarioObtenerxID(usuario.usu_id);
                            persona = personabl.PersonaIdObtenerJson(usuario.fk_persona);
                            Session["per_full"] = persona;
                            Session["ubigeo"] = ubigeobl.UbigeoObtenerDatosporIdJson(persona.fk_ubigeo);
                            postulante = postulantebl.PostulanteIdObtenerporUsuarioJson(usuario.usu_id);
                            Session["postulante"] = postulante;
                            rutaImagenes.imagenPostulante_CV(configuracion.config_nombre,postulante.pos_foto);
                            respuesta = true;
                            errormensaje = "Bienvenido, " + usuario.usu_nombre;
                        }
                        else
                        {
                            errormensaje = "La contraseña ingresada es erronea";
                        }
                    }
                }
                else
                {
                    errormensaje = "No se ha encontrando el usuario ingresado";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + "";
            }

            return Json(new { respuesta = respuesta, mensaje = errormensaje,estado=pendiente/*, usuario=usuario*/ });
        }
    
        [HttpPost]
        public ActionResult PostulanteRecuperarContrasenia(string correo_recuperacion)
        {
            var errormensaje = "";          
            string usuario_envio = "";
            string contrasenia_envio = "";
            string nombre = "";
            string contrasenia = "";
            UsuarioEntidad usuario = new UsuarioEntidad();
            PersonaEntidad persona = new PersonaEntidad();
            bool respuestaConsulta = false;
            

            try
            {
                usuario = usuariobl.PostulanteValidarCredenciales(correo_recuperacion.ToLower());
                if (usuario.usu_id > 0)
                {
                    usuario_envio = usuario.usu_nombre;
                    contrasenia_envio = GeneradorPassword.GenerarPassword(8);
                    contrasenia = Seguridad.EncriptarSHA512(contrasenia_envio);

                    respuestaConsulta = usuariobl.PostulanteUsuarioEditarContraseniaJson(usuario.usu_id, contrasenia);
                    if (respuestaConsulta)
                    {
                        persona = personabl.PersonaIdObtenerJson(usuario.fk_persona);
                        nombre = persona.per_nombre + " " + persona.per_apellido_pat + " " + persona.per_apellido_mat;
                        //string cuerpo_correo = "";
                        Correo correo = new Correo();
                        //MailMessage message = new MailMessage("s3k.zimbra@gmail.com", persona.per_correoelectronico, "correo de confirmacion", cuerpo_correo);
                        correo.EnviarCorreo(
                        persona.per_correoelectronico,
                                 "Correo de Confirmacion",
                                 "Hola! : " + nombre + " \n " +
                                 "Sus credenciales son las siguientes:\n Usuario : " + usuario_envio + "\n Contraseña : " + contrasenia_envio);
                        errormensaje = "Verifique su Correo ,Se le ha enviado su Contraseña nueva , Gracias.";
                    }
                    else
                    {
                        errormensaje = "No se pudo editar la contraseña";
                    }
                }
                else
                {
                    errormensaje = "Verifique su Correo ,No se encontro el Registro, Gracias.";
                }               
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
     
        [HttpPost]
        public ActionResult PostulanteCerrarSesionLoginJson()
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {

                Session["usu_full"] = null;
                Session["per_full"] = null;
                Session["ubigeo"] = null;
                Session["postulante"] = null;
                Session["rutaPerfil"] = null;
                Session["rutaCv"] = null;
                respuestaConsulta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        #endregion

        #region Proveedor
        public ActionResult ProveedorIndex()
        {
            //if (Session["usu_full"] != null)
            //{
            //    ViewBag.Message = "Bienvenido.";
            //    return View("~/Views/Persona/DatosPersonaVista.cshtml");
            //}
            //else
            //{
            //    ViewBag.Message = "Login De Acceso";
            //    return View();
            //}
            return View();
        }
     
        [HttpPost]
     
        public ActionResult ProveedorValidarLoginJson(string usu_login, string usu_password)
        {
            bool respuesta = false;
            //string nemonic = "RUTA_FOTO_POSTULANTE";
            string errormensaje = "";
            var usuario = new UsuarioEntidad();
            var persona = new PersonaEntidad();
            string proveedor = "PROVEEDOR";
            //var postulante = new postulanteEntidad();
            //var configuracion = configuracionbl.ConfiguracionObtenerporNemonicJson(nemonic);
            RutaImagenes rutaImagenes = new RutaImagenes();
            //string pendiente = "";
            try
            {
                usuario = usuariobl.ProveedorValidarCredenciales(usu_login.ToLower());
                if (usuario.usu_id > 0)
                {
                    if (usuario.usu_tipo.Equals(proveedor))
                    {
                        string password_encriptado = Seguridad.EncriptarSHA512(usu_password.Trim());
                        if (usuario.usu_contrasenia == password_encriptado)
                        {
                            Session["usu_proveedor"] = usuariobl.UsuarioObtenerxID(usuario.usu_id);
                            persona = personabl.PersonaIdObtenerJson(usuario.fk_persona);
                            Session["per_proveedor"] = persona;
                            //Session["ubigeo"] = ubigeobl.UbigeoObtenerDatosporIdJson(persona.fk_ubigeo);
                            //postulante = postulantebl.PostulanteIdObtenerporUsuarioJson(usuario.usu_id);
                            //Session["postulante"] = postulante;
                            //rutaImagenes.imagenPostulante_CV(configuracion.config_nombre, postulante.pos_foto);
                            respuesta = true;
                            errormensaje = "Bienvenido, " + usuario.usu_nombre;
                        }
                        else
                        {
                            errormensaje = "La contraseña ingresada es erronea";
                        }
                    }
                    else {
                        errormensaje = "Usted no tiene permiso para Acceder a Esta Seccion";
                    }
                    
                    
                }
                else
                {
                    errormensaje = "No se ha encontrando el usuario ingresado";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + "";
            }

            return Json(new { respuesta = respuesta, mensaje = errormensaje,/* estado = pendiente, usuario=usuario*/ });
        }

        [HttpPost]
        public ActionResult ProveedorCerrarSesionLoginJson()
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {

                Session["usu_proveedor"] = null;
                Session["per_proveedor"] = null;
                //Session["ubigeo"] = null;
                //Session["rutaPerfil"] = null;
               // Session["rutaCv"] = null;
                respuestaConsulta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        #endregion
    }
}