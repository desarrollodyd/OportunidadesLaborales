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
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ValidarLoginJson(string usu_login, string usu_password)
        {
            bool respuesta = false;
            string mensaje = "";
            var usuario = new usuarioEntidad();
            try
            {
                usuario = usuariobl.ValidarCredenciales(usu_login);
                if (usuario.usuarioId > 0)
                {
                    if (usuario.usuarioValidado == 0)
                    {
                        mensaje = "El usuario " + usuario.usuarioEmail + " no ha validado su Email";
                    }
                    else
                    {
                        string usuario_desencriptado = Seguridad.Desencriptar(usuario.usuarioContrasenia.Trim());
                        if (usu_password == usuario_desencriptado)
                        {
                            Session["usuarioId"] = usuario.usuarioId;
                            Session["usuarioEmail"] = usuario.usuarioEmail;
                            Session["usuarioFull"] = usuario;
                            respuesta = true;
                            mensaje = "Bienvenido, " + usuario.usuarioEmail;
                        }
                        else
                        {
                            mensaje = "La contraseña ingresada es erronea";
                        }
                    }
                }
                else
                {
                    mensaje = "No se ha encontrando el correo ingresado";
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
                Session["usuarioId"] = null;
                Session["usuarioEmail"] = null;
                Session["usuarioFull"] = null;
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