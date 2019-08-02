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
        public ActionResult OfertaLaboralListarJson()
        {
            string ola_nombre = Convert.ToString(Request.Params["ola_nombre"]);       
            string ola_cod_empresa =Convert.ToString(Request.Params["ola_cod_empresa"]);
            string ola_cod_cargo = Convert.ToString(Request.Params["ola_cod_cargo"]);         
            int ola_id = 1;
            bool respuestaConsulta = false;
            string errormensaje = "";
            var lista = new List<ofertaLaboralEntidad>();

            try
            {
                lista = ofertaLaboralbl.OfertaLaboralListarJson(ola_cod_empresa,ola_cod_cargo,ola_nombre);
                errormensaje = "Ofertas Listadas";
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