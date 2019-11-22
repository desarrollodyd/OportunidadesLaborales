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
       
        int fk_layout = 1;
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

            List<PersonaEntidad> persona = new List<PersonaEntidad>();
            claseError error = new claseError();
            string mensajeerrormenus = "";
            string mensajeerroractividades = "";
            string mensajeerrorcumpleanios = "";
            string mensaje = "";
            bool respuesta = false;
            try {
                //listando menus
                var menuTupla = intranetMenubl.IntranetMenuListarJson(fk_layout=1);
                error = menuTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    intranetMenu = menuTupla.intranetMenuLista;
                }
                else {
                    mensajeerrormenus = error.Value;
                }
                //listando actividades
                var actividadesTupla = intranetActividadesbl.IntranetActividadesListarJson(fk_layout = 1);
                error = actividadesTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    intranetActividades = actividadesTupla.intranetActividadesLista;
                }
                else
                {
                    mensajeerroractividades = error.Value;
                }
                //listando Cumpleaños
                var cumpleaniosTupla = personabl.PersonaObtenerCumpleaniosporDia();
                error = cumpleaniosTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    persona = cumpleaniosTupla.personaLista;
                }
                else
                {
                    mensajeerrorcumpleanios = error.Value;
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
                    dataCumpleanios =persona,
                    respuesta =respuesta,
                    mensaje =mensaje
                });
        }
        



        public ActionResult somopj()
        {
            return View();
        }
    }
}