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
            claseError error = new claseError();
            try
            {
                var menuTupla = menubl.WebMenuListarJson();
                error = menuTupla.error;
                listaMenus = menuTupla.lista.Where(x=>x.menu_estado.Equals("A")).ToList();
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Menus";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
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