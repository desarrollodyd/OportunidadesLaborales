using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Entidades.FichaCumplimiento;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using SistemaReclutamiento.Models.FichaCumplimiento;

using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

        CumUsuarioModel cumusubl = new CumUsuarioModel();
        CumEnvioModel cumenviobl = new CumEnvioModel();
        CumEnvioDetModel cumenviodetbl = new CumEnvioDetModel();

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
        public ActionResult PanelFichas(string token="")
        {
            string _token = "";
            _token = token;
            if (token != "")
            {

            }
            return View("~/Views/IntranetPJAdmin/IntranetPJFichas.cshtml");
        }

        public ActionResult FichaFormulario(string id)
        {
            var envio_id = Seguridad.Desencriptar(id);
            ViewBag.envioid = envio_id;
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
                string tipo = "EMPLEADO";
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
                string tipo = "POSTULANTE";
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
        public ActionResult EnviarJson(string[] listaEmpleados)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<cum_usuario> listausuarios = new List<cum_usuario>();
            CumUsuarioEntidad cumusuario = new CumUsuarioEntidad();
            CumEnvioEntidad cumenvio = new CumEnvioEntidad();
            CumEnvioDetalleEntidad cumenviodet = new CumEnvioDetalleEntidad();
            var correopersonal = "";
            var correocorporativo = "";
            var clave = "";
            int idcumusu = 0;
            try
            {
               
                foreach (var item in listaEmpleados)
                {
                    var itemarray = item.Split('|');


                    var usuarioTupla = fichabl.IntranetUsuarioListarJson(itemarray[0]);
                    correopersonal = itemarray[2];
                    correocorporativo = itemarray[1];
                    var cumusuarioExiste = usuarioTupla.intranetCumusuarioLista;
                    int existe = usuarioTupla.intranetCumusuarioLista.Count;

                    string path = Path.GetRandomFileName();
                    path = path.Replace(".", "");
                    clave = path.Substring(0, 8);
                
                    if (existe > 0)
                    {
                        //cumusuario.cus_fecha_act = DateTime.Now;
                        //cumusuario.cus_correo = correopersonal;
                        //cumusuario.cus_id = cumusuarioExiste[0].cus_id;
                        //var usuaupdate = cumusubl.CumUsuarioEditarcorreoJson(cumusuario);
                        idcumusu= cumusuarioExiste[0].cus_id;
                    }
                    else
                    {
                        cumusuario.cus_estado = "A";
                        cumusuario.cus_firma = "";
                        cumusuario.cus_dni = itemarray[0];
                        cumusuario.cus_correo = correopersonal;
                        cumusuario.cus_tipo = "EMPLEADO";
                        cumusuario.cus_fecha_reg = DateTime.Now;
                        cumusuario.cus_fecha_act = DateTime.Now;
                        cumusuario.cus_clave = clave;
                        var usuainsert = cumusubl.CumUsuarioInsertarsinfkuserJson(cumusuario);
                        idcumusu = usuainsert.idInsertado;
                    }

                    if (idcumusu > 0)
                    {
                        cumenvio.env_fecha_reg = DateTime.Now;
                        cumenvio.env_fecha_act = DateTime.Now;
                        cumenvio.env_estado = "1";
                        cumenvio.fk_cuestionario = 1;
                        cumenvio.fk_usuario = idcumusu;
                        var envio = cumenviobl.CumEnvioInsertarJson(cumenvio);

                        if (envio.idInsertado > 0)
                        {
                            cumenviodet.end_fecha_reg = DateTime.Now;
                            cumenviodet.end_fecha_act = DateTime.Now;
                            cumenviodet.end_estado = "1";
                            cumenviodet.end_dni = item;
                            cumenviodet.fk_envio = envio.idInsertado;
                            cumenviodet.end_correo_corp = correocorporativo;
                            cumenviodet.end_correo_pers = correopersonal;
                            var enviodet = cumenviodetbl.CumEnvioDetalleInsertarJson(cumenviodet);

                            //////envio correo aqui/////
                        }

                    }
                }

                mensaje = "Fichas Registradas";
                respuesta = true;

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult ReEnviarJson(int envioID)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            CumEnvioEntidad envio = new CumEnvioEntidad();
            try
            {
                var envioTupla = cumenviobl.CumEnvioIdObtenerJson(envioID);
                error = envioTupla.error;
                envio = envioTupla.cumEnvio;
                if (envio.fk_usuario > 0)
                {
                    int cususuario = envio.fk_usuario;
                    var cum_usua = cumusubl.CumUsuarioIdObtenerJson(cususuario);
                    var entidadusuario = cum_usua.cumUsuario;
                    var clave = entidadusuario.cus_clave;
                    var correo = entidadusuario.cus_correo;
                    var nombre = "";
                    var id = envioID.ToString();
                    var encriptado = Seguridad.Encriptar(id);
                    Correo correo_enviar = new Correo();
                    string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
                    //MailMessage message = new MailMessage("s3k.zimbra@gmail.com", persona.per_correoelectronico, "correo de confirmacion", cuerpo_correo);
                    correo_enviar.EnviarCorreo(
                        correo,
                        "Link de Ficha Sintomatológica",
                        "Hola! : " + nombre + " \n " +
                        "Tu clave es la que necesitaras para guardar tu ficha : " + clave + " \n " +
                        "Ingrese al siguiente Link y complete el formulario"
                        + "\n solo se puede editar el mismo dia de envio, \n" +
                        " Link Ficha Sintomatológica : " + basepath + "IntranetPJAdmin/FichaFormulario?id=" + encriptado
                        );

                    mensaje = "Se reenvio Ficha";
                    respuesta = true;
                }
                else
                {
                    mensaje = "No Se pudo reenviar Ficha";
                    respuesta = false;
                }
                

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta, mensaje, mensajeconsola = mensajeConsola });
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