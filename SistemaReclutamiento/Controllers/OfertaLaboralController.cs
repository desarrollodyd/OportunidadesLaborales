using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Entidades;

namespace SistemaReclutamiento.Controllers
{
    public class OfertaLaboralController : Controller
    {
        ofertaLaboralModel ofertaLaboralbl = new ofertaLaboralModel();
        // GET: OfertaLaboral
        public ActionResult OfertaLaboralListarVista()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OfertaLaboralListarJson(ReporteOfertaLaboral reporte)
        {

           // string ola_cod_cargo = Convert.ToString(Request.Form["ola_cod_cargo"]);         
            bool respuestaConsulta = false;
            string errormensaje = "";
            var lista = new List<ofertaLaboralEntidad>();

            try
            {
                lista = ofertaLaboralbl.OfertaLaboralListarJson(reporte);
                errormensaje = "Listando Ofertas";
                respuestaConsulta = true;
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje, respuesta=respuestaConsulta });
        }
    }
}