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
        
        // GET: IntranetPJ
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
            List<IntranetActividadesEntidad> intranetActividades = new List<IntranetActividadesEntidad>();

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
                //listando actividades
                var actividadesTupla = intranetActividadesbl.IntranetActividadesListarJson();
                error = actividadesTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    intranetActividades = actividadesTupla.intranetActividadesLista;
                    if (intranetActividades.Count > 0)
                    {
                        foreach (var m in intranetActividades)
                        {
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
                                                                    ListaDetalleElementoModal.Add(new
                                                                    {
                                                                        itemDetalleElementosModal.detelm_id,
                                                                        itemDetalleElementosModal.detelm_descripcion,
                                                                        itemDetalleElementosModal.detelm_nombre,
                                                                        itemDetalleElementosModal.detelm_extension,
                                                                        itemDetalleElementosModal.detelm_ubicacion,
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
                                                itemDetalleElemento.detel_extension,
                                                itemDetalleElemento.detel_ubicacion,
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
                //listando
            }
            catch (Exception ex) {
                mensaje = ex.Message;
                respuesta = false;
            }
            return Json(
                new {
                    dataMenus = intranetMenu.ToList(),
                    dataActividades = intranetActividades.ToList(),
                    dataCumpleanios = listaPersona,
                    respuesta = respuesta,
                    mensaje = mensaje,
                    listaNoticias = listaNoticiasDesordenado,
                    dataSecciones = ListaSeccion.ToList(),
                    mensajeerrorBD = mensajeerrorBD
                });
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
    }
}