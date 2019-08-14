using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class EstIdiomaController : Controller
    {
        estIdiomaModel estidiomabl = new estIdiomaModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EstIdiomaListarJson()
        {
            var errormensaje = "";
            var lista = new List<estIdiomaEntidad>();
            try
            {
                lista = estidiomabl.EstOfimaticaListarJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
    }
}