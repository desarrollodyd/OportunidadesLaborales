using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{

    public class EstIdiomaController : Controller
    {
        EstIdiomaModel estidiomabl = new EstIdiomaModel();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EstIdiomaListarJson()
        {
            var errormensaje = "";
            var lista = new List<EstIdiomaEntidad>();
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