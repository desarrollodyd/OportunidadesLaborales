using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class OfimaticaHerramientaController : Controller
    {
        // GET: OfimaticaHerramienta
        ofimaticaHerramientaModel ofimaticaHerramientabl = new ofimaticaHerramientaModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OfimaticaHerramientaListarJson()
        {
            var errormensaje = "";
            var lista = new List<ofimaticaHerramientaEntidad>();
            try
            {
                lista = ofimaticaHerramientabl.OfimatiacaHerramientaListarJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
    }
}