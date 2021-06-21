using SistemaReclutamiento.Entidades.WebCorporativa;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.WebCorporativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.WebCorporativaAdmin
{
    public class WebMenuController : Controller
    {
        WebMenuModel menubl = new WebMenuModel();
        WebElementoModel elementobl = new WebElementoModel();
        // GET: WebMenu
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WebMenuListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<WebMenuEntidad> listaMenus = new List<WebMenuEntidad>();
            List<WebElementoEntidad> listaElementos = new List<WebElementoEntidad>();
            claseError error = new claseError();
            try
            {
                var menuTupla = menubl.WebMenuListarJson();
                error = menuTupla.error;
                listaMenus = menuTupla.lista.Where(x=>x.menu_estado.Equals("A")).ToList();
                //Listar Elementos
                foreach(var menu in listaMenus)
                {
                    var elementoTupla = elementobl.WebElementoListarxMenuIDJson(menu.menu_id);
                    if (elementoTupla.error.Respuesta) {
                        menu.elemento = elementoTupla.lista;
                    }
                }
                if (error.Respuesta)
                {
                    mensaje = "Listando Menus";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Mensaje;
                    mensaje = "No se Pudieron Listar los Menus";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaMenus.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
    }
}