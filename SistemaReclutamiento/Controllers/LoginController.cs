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
                if (usuario.usu_id > 0)
                {
                    if (usuario.usu_estado.Equals('P'))
                    {
                        mensaje = "El usuario " + usuario.usu_nombre + " no ha validado su Email";
                    }
                    else
                    {
                        string password_encriptado = Seguridad.EncriptarSHA512(usu_password.Trim());
                        if (usuario.usu_contrasenia == password_encriptado)
                        {
                            Session["usu_id"] = usuario.usu_id;
                            Session["usu_nombre"] = usuario.usu_nombre;
                            Session["usu_full"] = usuario;
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