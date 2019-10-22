using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Entidades.Proveedor;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.Proveedor;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        UsuarioModel usuariobl = new UsuarioModel();
        PersonaModel personabl = new PersonaModel();
        UbigeoModel ubigeobl = new UbigeoModel();
        public ActionResult Index()
        {
           return View();
        }
        public ActionResult ReportePagosVista()
        {
            return View();
        }
        public ActionResult ProveedorCambiarPasswordVista() {
            return View();
        }
        #region Seccion de Acceso
        [HttpPost]
        public ActionResult ProveedorInsertarJson(UsuarioPersonaEntidad datos)
        {
            var errormensaje = "";
            //string nombre = datos.per_nombre + " " + datos.per_apellido_pat + " " + datos.per_apellido_mat;
            string usuario_envio = "";
            string contrasenia_envio = "";

            UsuarioEntidad usuario = new UsuarioEntidad();
            PersonaEntidad persona = new PersonaEntidad();

            int respuestaPersonaInsertada = 0;
            int respuestaUsuarioInsertado = 0;
            bool respuestaConsulta = false;

            string contrasenia = "";
            string correo = datos.per_correoelectronico;
            var usuario_repetido = usuariobl.ProveedorUsuarioObtenerxRUC(datos.per_numdoc);
            if (usuario_repetido.usu_id == 0)
            {
                var persona_repetida_tupla = personabl.PersonaEmailObtenerJson(datos.per_correoelectronico);
                var persona_repetida = persona_repetida_tupla.persona;
                var error = persona_repetida_tupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    if (persona_repetida.per_id == 0)
                    {
                        //Seteando datos correspondiente a persona            
                        persona.per_numdoc = datos.per_numdoc;
                        persona.per_nombre = datos.per_numdoc;
                        persona.per_apellido_pat = datos.per_numdoc;
                        persona.per_apellido_mat = datos.per_numdoc;
                        persona.per_correoelectronico = datos.per_correoelectronico;
                        persona.per_estado = "A";
                        //persona.per_tipodoc = datos.per_tipodoc;
                        persona.per_fecha_reg = DateTime.Now;
                        respuestaPersonaInsertada = personabl.PersonaProveedorInsertarJson(persona);
                        if (respuestaPersonaInsertada != 0)
                        {
                            //Insercion de Usuario
                            contrasenia = GeneradorPassword.GenerarPassword(8);
                            usuario.usu_contrasenia = Seguridad.EncriptarSHA512(contrasenia);
                            usuario.usu_nombre = datos.per_numdoc;
                            usuario.fk_persona = respuestaPersonaInsertada;
                            usuario.usu_estado = "P";
                            usuario.usu_cambio_pass = true;
                            usuario.usu_clave_temp = Seguridad.EncriptarSHA512(usuario.usu_nombre);
                            usuario.usu_fecha_reg = DateTime.Now;
                            usuario.usu_tipo = "PROVEEDOR";
                            respuestaUsuarioInsertado = usuariobl.PostulanteUsuarioInsertarJson(usuario);

                            if (respuestaUsuarioInsertado == 0)
                            {
                                return Json(new { respuesta = respuestaConsulta, mensaje = "Error al Intentar Registrar Usuario" });
                            }
                            respuestaConsulta = true;
                            //datos para cuerpo de correo
                            usuario_envio = usuario.usu_nombre;
                            contrasenia_envio = usuario.usu_contrasenia;
                        }
                        else
                        {
                            return Json(new { respuesta = respuestaConsulta, mensaje = "Error al Intentar Registrar a la Persona" });
                        }
                    }
                    else
                    {
                        return Json(new { respuesta = respuestaConsulta, mensaje = "Ya hay un usuario registrado con el correo: " + datos.per_correoelectronico });
                    }
                }
                else {
                    return Json(new { respuesta = respuestaConsulta, mensaje = error.Value });
                }
           
            }

            else {
                return Json(new { respuesta = respuestaConsulta, mensaje = "Ya hay un usuario registrado con el RUC: " + datos.per_numdoc });
            }
            if (respuestaConsulta)
            {
                /*LOGICA PARA ENVIO DE CORREO DE CONFIRMACION*/
                try
                {
                    //string cuerpo_correo = "";
                    Correo correo_enviar = new Correo();
                    string basepath = Request.Url.Scheme + "://" + ((Request.Url.Authority + Request.ApplicationPath).TrimEnd('/')) + "/";
                    //MailMessage message = new MailMessage("s3k.zimbra@gmail.com", persona.per_correoelectronico, "correo de confirmacion", cuerpo_correo);
                    correo_enviar.EnviarCorreo(
                        correo,
                        "Correo de Confirmacion",
                        "Hola! : " + " \n " +
                        "Sus credenciales son las siguientes:\n Usuario : " + usuario_envio + "\n Contraseña : " + contrasenia
                        + "\n puede usar estas credenciales para acceder al sistema, donde se le pedira realizar un cambio de esta contraseña por su seguridad, \n" +
                        " o puede hacer click en el siguiente enlace y seguir los pasos indicados para cambiar su contraseña y completar su registro : " + basepath + "Login/ProveedorActivacion?id=" + usuario.usu_clave_temp
                        );
                    errormensaje = "Verifique su Correo ,Se le ha enviado su Usuario y Contraseña para activar su Registro, Gracias.";
                }
                catch (Exception ex)
                {
                    errormensaje = ex.Message;
                }
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult CambiarPasswordVistaJson(string usu_password)
        {
            UsuarioEntidad usuario = (UsuarioEntidad)Session["usu_proveedor"];
            bool respuestaConsulta = false;
            string errormensaje = "";
            string password_encriptado = Seguridad.EncriptarSHA512(usu_password);
            try
            {
                respuestaConsulta = usuariobl.ProveedorUsuarioEditarContraseniaJson(usuario.usu_id, password_encriptado);
                errormensaje = "Contraseña actualizada correctamente";
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message + "";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }


        #endregion

        #region Seccion Permisos
        [HttpPost]
        public ActionResult ListarDataMenuJson()
        {
            var usuario = (UsuarioEntidad)Session["usu_proveedor"];
            var errormensaje = "";
            bool response = false;
            MenuModel menubl = new MenuModel();
            SubMenuModel submenubl = new SubMenuModel();
            List<MenuEntidad> listamenu = new List<MenuEntidad>();
            List<SubMenuEntidad> listasubmenu = new List<SubMenuEntidad>();
            claseError error = new claseError();
            try
            {
                var tuplalistamenu = menubl.MenuListarJson(usuario.usu_id);
                listamenu = tuplalistamenu.lista;
                error = tuplalistamenu.error;
                if (error.Key.Equals(string.Empty))
                {
                    if (listamenu.Count > 0)
                    {
                        foreach (var m in listamenu)
                        {
                            listasubmenu = submenubl.SubMenuListarPorMenuJson(m.men_id, usuario.usu_id);
                            m.SubMenu = listasubmenu;
                        }
                    }
                    response = true;
                }
                else
                {
                    return Json(new { respuesta = false, mensaje = error.Value });
                }
            }
            catch (Exception ex)
            {
                return Json(new { respuesta = false, mensaje = ex.Message });
            }

            return Json(new { data = listamenu.ToList(), respuesta = response, mensaje = errormensaje });
        }
        public ActionResult UsuarioProveedorListarJson()
        {
            var errormensaje = "";
            UsuarioModel usuariobl = new UsuarioModel();
            var lista = new List<UsuarioEntidad>();
            try
            {
                lista = usuariobl.ProveedorListarUsuariosPorTipoJson();
                errormensaje = "Cargando Usuarios ...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
        }
        #endregion

        #region Seccion Reportes
        [HttpPost]
        public ActionResult MesaPartesListarCompaniasJson()
        {
            var errormensaje = "";
            MesaPartesCIAModel mesapartesbl = new MesaPartesCIAModel();
            var lista = new List<MesaPartesCIAEntidad>();
            try
            {
                lista = mesapartesbl.MesaPartesListarCompanias();
                errormensaje = "Cargando compañias ...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
        }
        public ActionResult ListarPagosporCompaniaJson(string cboCompania, string fecha_inicio, string fecha_final)
        {
            //yyyy - MM - dd HH':'mm':'ss
            bool respuesta = false;
            string cadena = "";
            var errormensaje = "";
            string nombretabla = "CP" + cboCompania + "CART";
            string nombretablapago = "CP" + cboCompania + "PAGO";
            string tipo_doc = "FT";
            UsuarioEntidad usuario = (UsuarioEntidad)Session["usu_proveedor"];
            SQLModel sql = new SQLModel();
            var lista = new List<CPCARTEntidad>();
            try
            {
                var listatupla = sql.CPCARTListarPagosPorCompania(nombretabla, usuario.usu_nombre, tipo_doc, fecha_inicio, fecha_final);
                lista = listatupla.lista;
                cadena = listatupla.cadena;
                var errorlista= listatupla.error;
                if (errorlista.Key.Equals(string.Empty))
                {
                    if (lista.Count > 0)
                    {
                        foreach (var m in lista)
                        {
                            var subtotaltupla = sql.ObtenerSubtotalporNumeroDocumento(nombretablapago, m.CP_CNUMDOC, m.CP_CTIPDOC, usuario.usu_nombre);
                            m.subtotalSoles = subtotaltupla.subtotalSoles;
                            m.subtotalDolares = subtotaltupla.subtotalDolares;
                        }
                    }
                    errormensaje = "Cargando Data ...";
                    respuesta = true;
                }
                else {
                    errormensaje = errorlista.Value;
                    respuesta = false;
                }
                
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje, cadena=cadena });
        }
        public ActionResult ListarPagosporNumeroDocumentoJson(string num_doc, string nombre_tabla)
        {
            //yyyy - MM - dd HH':'mm':'ss
            bool respuesta = false;
            var errormensaje = "";
            string nombretabla = "CP" + nombre_tabla.Trim() + "PAGO";
            string tipo_doc = "FT";
            UsuarioEntidad usuario = (UsuarioEntidad)Session["usu_proveedor"];
            SQLModel sql = new SQLModel();
            var lista = new List<CPPAGOEntidad>();
            try
            {
                var listatupla = sql.CPPAGOListarPagosPorNumeroDocumento(nombretabla, usuario.usu_nombre, tipo_doc, num_doc.Trim());
                lista = listatupla.lista;
                var errorlista = listatupla.error;
                if (errorlista.Key.Equals(string.Empty))
                {
                    errormensaje = "Cargando Data ...";
                    respuesta = true;
                }
                else {
                    errormensaje = errorlista.Value;
                    respuesta = false;
                }
                
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = respuesta, mensaje = errormensaje });
        }
        #endregion
        //public ActionResult Index()
        //{
        //    string replace = "";
        //    ViewBag.controlleractionlist = ListarMetodosPorControlador("EducacionSuperior");
        //    //ViewBag.controllersnames = GetControllerNames();
        //    List<dynamic> lista = new List<dynamic>();
        //    foreach (var m in GetControllerNames())
        //    {
        //        lista.Add(m.Replace("Controller", replace));
        //    }
        //    ViewBag.controllersnames = lista;
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult SubMenuListarPorMenuJson()
        //{
        //    var errormensaje = "";
        //    SubMenuModel submenubl = new SubMenuModel();
        //    MenuModel menubl = new MenuModel();
        //    var listaSubMenu = new List<SubMenuEntidad>();
        //    var listaMenu = new List<MenuEntidad>();
        //    try
        //    {
        //        var tuplalistaMenu = menubl.MenuListarJson();
        //        listaMenu = tuplalistaMenu.lista;
        //        foreach(var m in listaMenu)
        //        {
        //            var lista = submenubl.SubMenuListarPorMenuJson(m.men_id);
        //            foreach (var n in lista) {
        //                listaSubMenu.Add(n);
        //            }
        //            //listaSubMenu = submenubl.SubMenuListarPorMenuJson(m.men_id);
        //        }
        //        errormensaje = "Cargando Data...";
        //    }
        //    catch (Exception exp)
        //    {
        //        errormensaje = exp.Message + ",Llame Administrador";
        //    }
        //    return Json(new { data = listaSubMenu.ToList(), dataMenu=listaMenu.ToList() , respuesta = true, mensaje = errormensaje });
        //}
        //[HttpPost]
        //public ActionResult ModuloListarJson() {
        //    var errormensaje = "";
        //    ModuloModel modulobl = new ModuloModel();
        //    var lista = new List<ModuloEntidad>();
        //    try
        //    {
        //        lista = modulobl.ModuloListarJson();
        //        errormensaje = "Cargando Data...";
        //    }
        //    catch (Exception exp)
        //    {
        //        errormensaje = exp.Message + ",Llame Administrador";
        //    }
        //    return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
        //}

        //public List<dynamic> ListarMetodosPorControlador(string controlador)
        //{
        //    List<dynamic> listametodos = new List<dynamic>();
        //    string nombrecontrolador = controlador+"Controller";
        //    Assembly asm = Assembly.GetExecutingAssembly();
        //    listametodos = asm.GetTypes()
        //                          .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
        //                          .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        //                          .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
        //                          .Where(m => (System.Attribute.GetCustomAttributes(typeof(SeguridadMenu), true).Length > 0))
        //                          //.Where(x=>String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))!= "SeguridadMenu")
        //                          .Where(m => (String.Join(",", (m.GetCustomAttributesData()
        //                                                      .Where(f => f.Constructor.DeclaringType.Name == "SeguridadMenu")
        //                                                      .Select(c => (c.ConstructorArguments[0].Value))))
        //                                                != "False"))
        //                         .Where(m => m.DeclaringType.Name.Equals(nombrecontrolador))
        //                          .Select(x => new
        //                          {
        //                              Controller = x.DeclaringType.Name,
        //                              Action = x.Name,
        //                              ReturnType = x.ReturnType.Name,
        //                              Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
        //                          })
        //                          .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList<dynamic>();
        //    return listametodos;

        //}
        //private static List<Type> GetSubClasses<T>()
        //{
        //    List<Type> lista = new List<Type>();
        //    lista= Assembly.GetCallingAssembly().GetTypes().Where(
        //        type => type.IsSubclassOf(typeof(T))).ToList();
        //    return lista;
        //}

        //public List<string> GetControllerNames()
        //{
        //    List<string> controllerNames = new List<string>();
        //    GetSubClasses<Controller>().ForEach(
        //        type => controllerNames.Add(type.Name));
        //    return controllerNames;
        //}
    }
}