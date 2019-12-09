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
using System.Web.Script.Serialization;

namespace SistemaReclutamiento.Controllers.IntranetPJ
{
    public class IntranetPJController : Controller
    {
        IntranetSeccionModel intraSeccionBL = new IntranetSeccionModel();
        IntranetElementoModel intraElementobl = new IntranetElementoModel();
        IntranetDetalleElementoModel intraDetalleElementobl = new IntranetDetalleElementoModel();
        IntranetMenuModel intranetMenubl = new IntranetMenuModel();
        IntranetElementoModalModel intranetElementoModalbl = new IntranetElementoModalModel();
        IntranetDetalleElementoModalModel intranetDetalleElementoModalbl = new IntranetDetalleElementoModalModel();
        IntranetSeccionElementoModel intranetSeccionImagenbl = new IntranetSeccionElementoModel();
        IntranetActividadesModel intranetActividadesbl = new IntranetActividadesModel();
        PersonaModel personabl = new PersonaModel();
        IntranetSaludoCumpleaniosModel intranetSaludoCumpleaniosbl = new IntranetSaludoCumpleaniosModel();
        IntranetCPJLocalModel intranetCPJLocalbl = new IntranetCPJLocalModel();
        //Acceso
        IntranetAccesoModel usuarioAccesobl = new IntranetAccesoModel();
        UsuarioModel usuariobl = new UsuarioModel();
        // GET: IntranetPJ

        RutaImagenes rutaImagenes = new RutaImagenes();
        string PathActividadesIntranet = ConfigurationManager.AppSettings["PathArchivosIntranet"].ToString();
        public ActionResult Index(int? menu)
        {
            claseError error = new claseError();
            var menuTupla = intranetMenubl.IntranetMenuListarJson();
            int? id = 0;
            if (menu != 0)
            {
                id = menu;
            }
            else
            {
                error = menuTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    if (menuTupla.intranetMenuLista.Count > 0)
                    {
                        var elementomenu = menuTupla.intranetMenuLista.First();
                        id = elementomenu.menu_id;
                    }
                    else
                    {
                        id = 0;
                    }

                }
                else
                {
                    id = -1;
                }
            }
            ViewBag.menu_id = id;
            return View("~/Views/IntranetPJ/IntranetPJIndex.cshtml");
        }

        [HttpPost]
        public ActionResult ListarDataJson(int menu_id)
        {

            //listar menus, cumpleaños, actividades, informacion de la Empresa, comentarios y Seccion PJ NEWS para el layout
            List<IntranetSeccionEntidad> intranetSeccion = new List<IntranetSeccionEntidad>();
            List<IntranetMenuEntidad> intranetMenu = new List<IntranetMenuEntidad>();
            List<IntranetActividadesEntidad> intranetActividades_ = new List<IntranetActividadesEntidad>();
            List<IntranetActividadesEntidad> intranetActividades = new List<IntranetActividadesEntidad>();
            List<IntranetSaludoCumpleanioEntidad> intraSaludos = new List<IntranetSaludoCumpleanioEntidad>();

            List<PersonaEntidad> listaPersona = new List<PersonaEntidad>();
            claseError error = new claseError();

            //Para rotulado de noticias
            List<Tuple<DateTime ,string ,string >> listaNoticias = new List<Tuple<DateTime, string ,string>>();
            List<Tuple<DateTime, string, string>> listaNoticiasDesordenado = new List<Tuple<DateTime, string, string>>();
            Random randNum = new Random();
            var ListaSeccion = new List<dynamic>();
            string mensajeerrorBD = "";
            string mensaje = "";
            bool respuesta = false;
            //Cantidad de Salas y Apuestas Deportivas
            IntranetCPJLocalEntidad intranetCPJlocal = new IntranetCPJLocalEntidad();
            int cantidadSalas = 0;
            int cantidadApuestasDeportivas = 0;
            try {
                //listando menus
                var menuTupla = intranetMenubl.IntranetMenuListarJson();
                error = menuTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    intranetMenu = menuTupla.intranetMenuLista;
                    
                }
                else {
                    mensajeerrorBD += "Error en Menus: " + error.Value+"\n";
                }

                var mensajesCumpleanios = intranetSaludoCumpleaniosbl.IntranetSaludoCumpleanioActivosListarJson();
                error = mensajesCumpleanios.error;
                if (error.Key.Equals(string.Empty))
                {
                    intraSaludos = mensajesCumpleanios.intranetSaludoCumpleanioLista;

                }
                else
                {
                    mensajeerrorBD += "Error en Menus: " + error.Value + "\n";
                }

                //listando actividades
                var actividadesTupla = intranetActividadesbl.IntranetActividadesListarJson();
                error = actividadesTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    intranetActividades_ = actividadesTupla.intranetActividadesLista;
                    if (intranetActividades_.Count > 0)
                    {
                        foreach (var m in intranetActividades_)
                        {
                            if (m.act_imagen != "")
                            {
                                m.act_imagen = rutaImagenes.ImagenIntranetActividades(PathActividadesIntranet + "\\Actividades\\", m.act_imagen);
                            }
                            
                            intranetActividades.Add(m);
                            listaNoticias.Add(Tuple.Create( m.act_fecha,  m.act_descripcion, "Actividad: "));
                        }
                    }
                }
                else
                {
                    mensajeerrorBD += "Error en Actividades: " + error.Value + "\n";
                }
                //listando Cumpleaños
                var cumpleaniosTupla = personabl.PersonaObtenerCumpleaniosporDia();
                error = cumpleaniosTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    listaPersona = cumpleaniosTupla.personaLista;
                    if (listaPersona.Count > 0) {
                        foreach (var m in listaPersona) {
                            string descripcionNoticia = m.per_nombre + " " + m.per_apellido_pat + " " + m.per_apellido_mat;
                            listaNoticias.Add(Tuple.Create(m.per_fechanacimiento, descripcionNoticia, "Cumpleaños: "));
                        }
                    }
                }
                else
                {
                    mensajeerrorBD += "Error en Cumpleaños: " + error.Value + "\n";
                }
                //desordenando lista de noticias
                while (listaNoticias.Count > 0)
                {
                    int val = randNum.Next(0, listaNoticias.Count - 1);
                    listaNoticiasDesordenado.Add(listaNoticias[val]);
                    listaNoticias.RemoveAt(val);
                }
                
                var seccionesMenu = intraSeccionBL.IntranetSeccionListarxMenuIDJson(menu_id);
                error = seccionesMenu.error;
                if (error.Key.Equals(string.Empty))
                {
                    intranetSeccion = seccionesMenu.intranetSeccionListaxMenuID.Where(x=>x.sec_estado=="A").ToList();
                    if (intranetSeccion.Count > 0)
                    {
                        //secciones
                        foreach (var itemSt in intranetSeccion)
                        {
                            var ListaElementos = new List<dynamic>();
                            var elementos = intraElementobl.IntranetElementoListarxSeccionIDJson(itemSt.sec_id);
                            List<IntranetElementoEntidad> intranetElementos = new List<IntranetElementoEntidad>();
                            intranetElementos = elementos.intranetElementoListaxSeccionID.Where(x=>x.elem_estado=="A").ToList();
                            error = elementos.error;
                            if (intranetElementos.Count > 0)
                            {
                                //elementos
                                foreach (var itemElementos in intranetElementos)
                                {
                                    var ListaDetalleElemento = new List<dynamic>();
                                    var detalleelementos = intraDetalleElementobl.IntranetDetalleElementoListarxElementoIDJson(itemElementos.elem_id);
                                    List<IntranetDetalleElementoEntidad> intranetDetElementos = new List<IntranetDetalleElementoEntidad>();
                                    intranetDetElementos = detalleelementos.intranetDetalleElementoListaxElementoID.Where(x => x.detel_estado == "A").ToList();
                                    error = elementos.error;
                                    if (intranetDetElementos.Count > 0) {
                                        //detalle elementos
                                        foreach (var itemDetalleElemento in intranetDetElementos)
                                        {
                                            if (itemDetalleElemento.detel_nombre != "")
                                            {
                                                itemDetalleElemento.detel_nombre  = rutaImagenes.ImagenIntranetActividades(PathActividadesIntranet + "\\", itemDetalleElemento.detel_nombre+"."+ itemDetalleElemento.detel_extension);
                                            }

                                            var seccion_elemento = new List<dynamic>();
                                            if (itemDetalleElemento.fk_seccion_elemento>0)
                                            {
                                                var seccion_ele = intranetSeccionImagenbl.IntranetSeccionElementoIdObtenerJson(itemDetalleElemento.fk_seccion_elemento);
                                                if (seccion_ele.intranetSeccionElemento.sele_id > 0)
                                                {
                                                    var Lista_elemento_modal = new List<dynamic>();
                                                    var elementosModal = intranetElementoModalbl.IntranetElementoModalListarxSeccionElementoIDJson(seccion_ele.intranetSeccionElemento.sele_id);
                                                    List<IntranetElementoModalEntidad> intranetElementoModal = new List<IntranetElementoModalEntidad>();
                                                    intranetElementoModal = elementosModal.intranetElementoModalListaxseccionelementoID.Where(x => x.emod_estado == "A").ToList();
                                                    error = elementos.error;
                                                    if (intranetElementoModal.Count > 0)
                                                    {
                                                        //elemento modal
                                                        foreach (var itemElementosModal in intranetElementoModal)
                                                        {
                                                            var ListaDetalleElementoModal = new List<dynamic>();
                                                            var detalleelementosModal = intranetDetalleElementoModalbl.IntranetDetalleElementoModalListarxElementoIDJson(itemElementosModal.emod_id);
                                                            List<IntranetDetalleElementoModalEntidad> intranetDetElementoModal = new List<IntranetDetalleElementoModalEntidad>();
                                                            intranetDetElementoModal = detalleelementosModal.intranetDetalleElementoModalListaxElementoID.Where(x => x.detelm_estado == "A").ToList();
                                                            error = elementos.error;
                                                            if (intranetDetElementoModal.Count > 0)
                                                            {
                                                                foreach (var itemDetalleElementosModal in intranetDetElementoModal)
                                                                {
                                                                    if (itemDetalleElementosModal.detelm_nombre != "")
                                                                    {
                                                                        itemDetalleElementosModal.detelm_nombre = rutaImagenes.ImagenIntranetActividades(PathActividadesIntranet + "\\", itemDetalleElementosModal.detelm_nombre + "." + itemDetalleElementosModal.detelm_extension);
                                                                    }
                                                                    ListaDetalleElementoModal.Add(new
                                                                    {
                                                                        itemDetalleElementosModal.detelm_id,
                                                                        itemDetalleElementosModal.detelm_descripcion,
                                                                        itemDetalleElementosModal.detelm_nombre,
                                                                        itemDetalleElementosModal.fk_elemento_modal,
                                                                        itemDetalleElementosModal.detelm_orden,
                                                                        itemDetalleElementosModal.detelm_posicion,
                                                                    });
                                                                }
                                                            }
                                                            Lista_elemento_modal.Add(new
                                                            {
                                                                itemElementosModal.emod_id,
                                                                itemElementosModal.emod_titulo,
                                                                itemElementosModal.emod_descripcion,
                                                                itemElementosModal.emod_contenido,
                                                                itemElementosModal.emod_orden,
                                                                itemElementosModal.fk_seccion_elemento,
                                                                itemElementosModal.fk_tipo_elemento,
                                                                detalle_elemento_modal = ListaDetalleElementoModal
                                                            });
                                                        }
                                                    }

                                                        seccion_elemento.Add(new{
                                                            seccion_ele.intranetSeccionElemento.sele_id,
                                                            seccion_ele.intranetSeccionElemento.sele_orden,
                                                            elemento_modal = Lista_elemento_modal
                                                        });
                                                }
                                            }

                                            ListaDetalleElemento.Add(new {
                                                itemDetalleElemento.detel_id,
                                                itemDetalleElemento.detel_descripcion,
                                                itemDetalleElemento.detel_nombre,
                                                itemDetalleElemento.fk_elemento,
                                                itemDetalleElemento.fk_seccion_elemento,
                                                itemDetalleElemento.detel_orden,
                                                itemDetalleElemento.detel_posicion,
                                                seccion_elemento = seccion_elemento 
                                            });
                                        }

                                    }
                                    ListaElementos.Add(new {
                                        itemElementos.elem_id,
                                        itemElementos.elem_titulo,
                                        itemElementos.elem_descripcion,
                                        itemElementos.elem_contenido,
                                        itemElementos.elem_orden,
                                        itemElementos.fk_seccion,
                                        itemElementos.fk_tipo_elemento,
                                        detalleElemento = ListaDetalleElemento
                                    });
                                }
                            }

                            ListaSeccion.Add(new { itemSt.sec_id,itemSt.sec_orden, elementos = ListaElementos });
                        }
                    }

                }
                else
                {
                    mensajeerrorBD += "Error en Menus: " + error.Value + "\n";
                }




                respuesta = true;
                mensaje = "Listando Data";
                //listando Cantidad de Salas y Apuestas Deportivas
                var cantidadTupla = intranetCPJLocalbl.IntranetCPJLocalListarCantidadLocalesJson();
                error = cantidadTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    cantidadSalas = cantidadTupla.intranetLocalCantidadSalas;
                    cantidadApuestasDeportivas = cantidadTupla.intranetLocalCantidadApuestasDeportivas;
                }
                else {
                    mensajeerrorBD += "Error al Listar Cantidad de Salas y Apuestas Deportivas" + error.Value+"\n";
                }
            }
            catch (Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }


            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                dataMenus = intranetMenu.ToList(),
                dataActividades = intranetActividades.ToList(),
                dataCumpleanios = listaPersona,
                dataSaludos = intraSaludos.ToList(),
                respuesta = respuesta,
                mensaje = mensaje,
                listaNoticias = listaNoticiasDesordenado,
                dataSecciones = ListaSeccion.ToList(),
                mensajeerrorBD = mensajeerrorBD,
                cantidadSalas = cantidadSalas,
                cantidadApuestasDeportivas = cantidadApuestasDeportivas
            };
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;

        }

        public ActionResult Mapa(string tipo)
        {
            
            ViewBag.tipo = tipo;
            return View("~/Views/IntranetPJ/IntranetPJMapa.cshtml");
        }


        public ActionResult somopj()
        {
            return View();
        }

        public ActionResult politicaspj()
        {
            return View();
        }
        public ActionResult mitiempopj()
        {
            return View();
        }
        public ActionResult misherramientaspj()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ListarLocalesporTipoJson(string tipo, string nombre="") {
            string _tipo = tipo;
            string _nombre = nombre.ToLower();
            List<IntranetCPJLocalEntidad> intranetLocalLista = new List<IntranetCPJLocalEntidad>();
            claseError error = new claseError();
            bool response = false;
            string errormensaje = "";
            try {
                var listaTupla = intranetCPJLocalbl.IntranetCPJLocalListarporNombreJson(_tipo,_nombre);
                error = listaTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    intranetLocalLista = listaTupla.intranetCPJLocalesLista;
                    response = true;
                    errormensaje = "Listando Locales";
                }
                else {
                    response = false;
                    errormensaje = error.Value;
                }
            }
            catch (Exception ex) {
                errormensaje = "No se Pudieron Listar los Locales"+ ex.Message;
                response = false;
            }
            return Json(new { data=intranetLocalLista.ToList(),respuesta=response,mensaje=errormensaje});
        }
        public ActionResult Login() {
            return View("~/Views/IntranetPJ/IntranetPJLogin.cshtml");
        }
        [HttpPost]
        public ActionResult IntranetLoginValdidarCredencialesJson(string usu_login,string usu_password)
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
                var usuarioTupla = usuarioAccesobl.UsuarioIntranetValidarCredenciales(usu_login.ToLower());
                error = usuarioTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    usuario = usuarioTupla.intranetUsuarioEncontrado;
                    if (usuario.usu_id > 0)
                    {
                        if (usuario.usu_estado == "A")
                        {
                            if (usuario.usu_tipo == "EMPLEADO")
                            {
                                var contrasenia = Seguridad.EncriptarSHA512(usu_password.Trim());
                                if (usuario.usu_contrasenia == Seguridad.EncriptarSHA512(usu_password.Trim()))
                                {
                                    Session["usu_full"] = usuariobl.UsuarioObtenerxID(usuario.usu_id);
                                    persona = personabl.PersonaIdObtenerJson(usuario.fk_persona);
                                    Session["per_full"] = persona;
                                    respuesta = true;
                                    errormensaje = "Bienvenido, " + usuario.usu_nombre;
                                }
                                else{
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
                else {
                    errormensaje = "Ha ocurrido un problema";
                    mensajeConsola = error.Value;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + "";
                mensajeConsola = error.Value;
            }

            return Json(new { mensajeconsola=mensajeConsola,respuesta = respuesta, mensaje = errormensaje, estado = pendiente/*, usuario=usuario*/ });
        }
        [HttpPost]
        public ActionResult IntranetPJCerrarSesionLoginJson()
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            try
            {

                Session["usu_full"] = null;
                Session["per_full"] = null;
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