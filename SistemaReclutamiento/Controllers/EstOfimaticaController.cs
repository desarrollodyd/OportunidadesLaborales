using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class EstOfimaticaController : Controller
    {
        // GET: OfimaticaHerramienta
        estOfimaticaModel estofimaticabl = new estOfimaticaModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EstOfimaticaListarJson()
        {
            var errormensaje = "";
            var lista = new List<estOfimaticaEntidad>();
            try
            {
                lista = estofimaticabl.EstOfimaticaListarJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
    }
}