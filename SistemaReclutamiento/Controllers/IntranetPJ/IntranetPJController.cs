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
        IntranetMenuModel intranetMenubl = new IntranetMenuModel();
        IntranetActividadesModel intranetActividadesbl = new IntranetActividadesModel();
        PersonaModel personabl = new PersonaModel();
        
        // GET: IntranetPJ
        public ActionResult IntranetPJIndex()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetPjListarDataInicialJson()
        {
            //listar menus, cumpleaños, actividades, informacion de la Empresa, comentarios y Seccion PJ NEWS para el layout
            
            List<IntranetMenuEntidad> intranetMenu = new List<IntranetMenuEntidad>();
            List<IntranetActividadesEntidad> intranetActividades = new List<IntranetActividadesEntidad>();
            //Para Seccion Noticias
            List<IntranetActividadesEntidad> intranetActividadesCumpleanios = new List<IntranetActividadesEntidad>();
            IntranetActividadesEntidad intranetActividadCumpleanio = new IntranetActividadesEntidad();

            List<PersonaEntidad> listaPersona = new List<PersonaEntidad>();
            claseError error = new claseError();

            //Para rotulado de noticias
            List<Tuple<DateTime ,string ,string >> listaNoticias = new List<Tuple<DateTime, string ,string>>();
            List<Tuple<DateTime, string, string>> listaNoticiasDesordenado = new List<Tuple<DateTime, string, string>>();
            Random randNum = new Random();

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
                            listaNoticias.Add(Tuple.Create( m.act_fecha,  m.act_descripcion, "actividad"));
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
                            listaNoticias.Add(Tuple.Create(m.per_fechanacimiento, descripcionNoticia, "cumpleanios"));
                        }
                    }
                }
                else
                {
                    mensajeerrorBD += "Error en Actividades: " + error.Value + "\n";
                }
                //desordenando lista de noticias
                while (listaNoticias.Count > 0)
                {
                    int val = randNum.Next(0, listaNoticias.Count - 1);
                    listaNoticiasDesordenado.Add(listaNoticias[val]);
                    listaNoticias.RemoveAt(val);
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
                    mensajeerrorBD= mensajeerrorBD
                });
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