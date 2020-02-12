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

namespace SistemaReclutamiento.Controllers.IntranetPJAdmin
{
    public class IntranetPjAdminController : Controller
    {
        IntranetUsuarioModel usuarioIntranetbl = new IntranetUsuarioModel();
        IntranetMenuModel intranetMenubl = new IntranetMenuModel();
        IntranetAccesoModel usuarioAccesobl = new IntranetAccesoModel();
        UsuarioModel usuariobl = new UsuarioModel();
        PersonaModel personabl = new PersonaModel();
        // GET: IntranetPjAdmin
        public ActionResult Index()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJAdminIndex.cshtml");
        }

        public ActionResult PanelMenus()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJMenus.cshtml");
        }
        public ActionResult PanelActividades()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJActividades.cshtml");
        }
        public ActionResult PanelComentarios()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJComentarios.cshtml");
        }
        public ActionResult PanelFooter()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJFooter.cshtml");
        }
        public ActionResult PanelArchivos()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJArchivos.cshtml");
        }
        public ActionResult PanelSecciones(int menu_id=1)
        {
            //List<IntranetMenuEntidad> intranetMenu = new List<IntranetMenuEntidad>();
            //claseError error = new claseError();
            //string mensajeerrorBD = "";
            //string mensaje = "";
            //try
            //{
            //    var menuTupla = intranetMenubl.IntranetMenuListarJson();
            //    error = menuTupla.error;
            //    if (error.Key.Equals(string.Empty))
            //    {
            //        intranetMenu = menuTupla.intranetMenuLista;
            //        ViewBag.Menu = intranetMenu;
            //    }
            //    else
            //    {
            //        mensajeerrorBD += "Error en Menus: " + error.Value + "\n";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    mensaje = ex.Message;
            //}
            
            return View("~/Views/IntranetPJAdmin/IntranetPJSecciones1.cshtml");
        }

        #region seccion1

        public ActionResult PanelSecciones1()
        {
            return View("~/Views/IntranetPjAdmin/IntranetPJSecciones1.cshtml");
        }
        #endregion


        #region Region Acceso a Mantenimiento Intranet PJ
        public ActionResult Login()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJAdminLogin.cshtml");
        }
        [HttpPost]
        public ActionResult IntranetPJAdminValidarCredencialesJson(string usu_login, string usu_password)
        {
            bool respuesta = false;
            string errormensaje = "";
            string mensajeConsola = "";
            UsuarioEntidad usuario = new UsuarioEntidad();
            PersonaEntidad persona = new PersonaEntidad();
            claseError error = new claseError();
            string pendiente = "";
            try
            {
                var usuarioTupla = usuarioAccesobl.UsuarioIntranetSGCValidarCredenciales(usu_login.ToLower());
                error = usuarioTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    usuario = usuarioTupla.intranetUsuarioSGCEncontrado;
                    if (usuario.usu_id > 0)
                    {
                        if (usuario.usu_estado == "A")
                        {
                            if (usuario.usu_tipo == "EMPLEADO")
                            {
                                if (usuario.usu_contrasenia == Seguridad.EncriptarSHA512(usu_password.Trim()))
                                {
                                    Session["usuSGC_full"] = usuariobl.UsuarioObtenerxID(usuario.usu_id);
                                    persona = personabl.PersonaIdObtenerJson(usuario.fk_persona);
                                    Session["perSGC_full"] = persona;
                                    respuesta = true;
                                    errormensaje = "Bienvenido, " + usuario.usu_nombre;
                                }
                                else
                                {
                                    errormensaje = "Contraseña no Coincide";
                                }
                            }
                            else
                            {
                                errormensaje = "Usuario no Pertenece a CPJ";
                            }
                        }
                        else
                        {
                            errormensaje = "Usuario no se Encuentra Activo";
                        }
                    }
                    else
                    {
                        errormensaje = "Usuario no Encontrado";
                    }
                }
                else
                {
                    errormensaje = "Ha ocurrido un problema";
                    mensajeConsola = error.Value;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + "";
                mensajeConsola = error.Value;
            }

            return Json(new { mensajeconsola = mensajeConsola, respuesta = respuesta, mensaje = errormensaje, estado = pendiente/*, usuario=usuario*/ });
        }
        [HttpPost]
        public ActionResult IntranetPJAdminCerrarSesionLoginJson()
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {
                Session["usuSGC_full"] = null;
                Session["perSGC_full"] = null;
                respuestaConsulta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        #endregion

        #region Region Token
        public ActionResult PanelConfiguracionToken() {
            return View("~/Views/IntranetPJAdmin/IntranetPJToken.cshtml");
        }
        [HttpPost]
        public ActionResult ListarUsuariosTokenJson() {
            string errormensaje = "";
            bool response = false;
            List<UsuarioPersonaEntidad> listaUsuarios = new List<UsuarioPersonaEntidad>();
            try {
                var listaTupla = usuariobl.IntranetListarUsuariosTokenJson();
                if (listaTupla.error.Key.Equals(string.Empty))
                {
                    listaUsuarios = listaTupla.listaUsuarios;
                    errormensaje = "Listando Usuarios Mesa de Partes";
                    response = true;
                }
                else {
                    errormensaje = listaTupla.error.Value;
                }
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message;
            }
            return Json(new {data=listaUsuarios.ToList(),respuesta=response,mensaje=errormensaje });
        }
        #endregion
    }
}