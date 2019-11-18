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

    public class EstOfimaticaController : Controller
    {
        // GET: OfimaticaHerramienta
        EstOfimaticaModel estofimaticabl = new EstOfimaticaModel();

        public ActionResult Index()
        {
            return View();
        }
   
        [HttpPost]
        public ActionResult EstOfimaticaListarJson()
        {
            var errormensaje = "";
            var lista = new List<EstOfimaticaEntidad>();
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