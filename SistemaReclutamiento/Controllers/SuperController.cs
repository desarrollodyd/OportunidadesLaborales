using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class SuperController : Controller
    {
        // GET: Super
        public ActionResult Control()
        {
            return View("~/Views/Desarrollo/PanelPrincipal.cshtml");
        }
        [HttpPost]
        public ActionResult SubMenuListarJson()
        {
            var errormensaje = "";
            SubMenuModel submenubl = new SubMenuModel();
            var lista = new List<SubMenuEntidad>();
            bool response = false;
            try
            {
                lista = submenubl.SubMenuListarJson();
               
                if (lista.Count > 0)
                {
                    errormensaje = "Cargando Data...";
                    response = true;
                }
                else {
                    errormensaje = "No Hay Datos";
                    response = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
                response = false;
            }
            return Json(new { data = lista.ToList(), respuesta = response, mensaje = errormensaje });
        }
    }
}