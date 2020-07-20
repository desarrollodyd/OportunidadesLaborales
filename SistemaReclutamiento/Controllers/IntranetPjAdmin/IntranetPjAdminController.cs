using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        IntranetDetalleElementoModel detalleelementobl = new IntranetDetalleElementoModel();
        IntranetDetalleElementoModalModel detalleelementomodalbl = new IntranetDetalleElementoModalModel();
        UsuarioModel usuariobl = new UsuarioModel();
        PersonaModel personabl = new PersonaModel();

        IntranetFichaModel fichabl = new IntranetFichaModel();
        string pathArchivosIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        claseError error = new claseError();
        RutaImagenes rutaImagenes = new RutaImagenes();
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
        public ActionResult PanelFichas()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJFichas.cshtml");
        }

        public ActionResult FichaFormulario()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJFichaFormulario.cshtml");
        }

        [HttpPost]
        public ActionResult IntranetFichasEmpleadoListarJson(DateTime desde,DateTime hasta , int estado)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<cum_envio> listaEnvios = new List<cum_envio>();
            try
            {
                string tipo = "Empleado";
                var envioTupla = fichabl.IntranetFichaListarJson(tipo, desde, hasta);
                error = envioTupla.error;
                listaEnvios = envioTupla.intranetFichaLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Fichas";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Fichas";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaEnvios.ToList(), respuesta, mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetFichasPostulanteListarJson(DateTime desde, DateTime hasta, int estado)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<cum_envio> listaEnvios = new List<cum_envio>();
            try
            {
                string tipo = "Postulante";
                var envioTupla = fichabl.IntranetFichaListarJson(tipo, desde, hasta);
                error = envioTupla.error;
                listaEnvios = envioTupla.intranetFichaLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Fichas";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Fichas";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaEnvios.ToList(), respuesta, mensaje, mensajeconsola = mensajeConsola });
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
        public ActionResult PanelConfiguracionToken()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJToken.cshtml");
        }
        public ActionResult PanelSistemas()
        {
            return View("~/Views/IntranetPJAdmin/IntranetPJSistemas.cshtml");
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

        #region Edicion de Hash para imagenes de Detalle Elemento y Detalle elemento modal
        public ActionResult IntranetEditarHashDetallesJson() {
            bool response = false;
            string errormensaje = "";
            List<IntranetDetalleElementoEntidad> listaDetalleElemento = new List<IntranetDetalleElementoEntidad>();
            List<IntranetDetalleElementoModalEntidad> listaDetalleElementoModal = new List<IntranetDetalleElementoModalEntidad>();
            List<IntranetDetalleElementoEntidad> listaDetalleElementoDevuelta = new List<IntranetDetalleElementoEntidad>();
            List<IntranetDetalleElementoModalEntidad> listaDetalleElementoModalDevuelta = new List<IntranetDetalleElementoModalEntidad>();
            int totalDetalles=0, totaldetallesEditados=0, totaldetallemodal=0, totaldetallemodalEditado = 0;
            try
            {
                //Detalleelemento
                var listadetelemtupla = detalleelementobl.IntranetDetalleElementoListarJson();
                if (listadetelemtupla.error.Key.Equals(string.Empty))
                {
                    listaDetalleElemento = listadetelemtupla.intranetDetalleElementoLista.Where(x => x.detel_extension != "").ToList();
                    totalDetalles = listaDetalleElemento.Count;
                    if (listaDetalleElemento.Count > 0) {
                        
                        foreach (var detalle in listaDetalleElemento) {
                            detalle.detel_hash= rutaImagenes.ImagenIntranetActividades(pathArchivosIntranet, detalle.detel_nombre+"."+detalle.detel_extension);
                            var detalleElementoEditado = detalleelementobl.IntranetDetalleElementoEditarHashJson(detalle);
                            if (detalleElementoEditado.error.Key.Equals(string.Empty))
                            {
                                totaldetallesEditados++;
                            }
                            else {
                                errormensaje += detalleElementoEditado.error.Value;
                            }
                        }
                    }
                    errormensaje += "Detalle Elemento,";
                }
                else
                {
                    errormensaje += listadetelemtupla.error.Value;
                }
                //DetalleElementoModal
                var listadetelemodTupla = detalleelementomodalbl.IntranetDetalleElementoModalListarJson();
                if (listadetelemodTupla.error.Key.Equals(string.Empty))
                {
                    listaDetalleElementoModal = listadetelemodTupla.intranetDetalleElementoModalLista.Where(x => x.detelm_extension != "").ToList();
                    totaldetallemodal = listaDetalleElementoModal.Count;
                    if (listaDetalleElementoModal.Count > 0)
                    {
                        foreach (var detallemodal in listaDetalleElementoModal)
                        {
                            detallemodal.detelm_hash = rutaImagenes.ImagenIntranetActividades(pathArchivosIntranet, detallemodal.detelm_nombre + "." + detallemodal.detelm_extension);
                            var detalleElementoModalEditado = detalleelementomodalbl.IntranetDetalleElementoModalEditarHashJson(detallemodal);
                            if (detalleElementoModalEditado.error.Key.Equals(string.Empty))
                            {
                                totaldetallemodalEditado++;
                            }
                            else
                            {
                                errormensaje += detalleElementoModalEditado.error.Value;
                            }
                        }
                    }
                    errormensaje += " Detalle Elemento Modal,";
                }
                else {
                    errormensaje += listadetelemodTupla.error.Value;
                }
                response = true;
                errormensaje += " Editados";

            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            //DetalleElemento
            //DetalleElementoModal
            return Json(new {
                totaldetalleElemento =totalDetalles,
                totaldetalleElementoEditado =totaldetallesEditados,
                totaldetalleElementoModal= totaldetallemodal,
                totalDetalleElementoModalEditados= totaldetallemodalEditado,
                respuesta =response,
                mensaje =errormensaje},JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}