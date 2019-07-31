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
            //int fk_ubigeo = Convert.ToInt32(Request.Params["fk_ubigeo"]);
            string ola_cod_empresa =Convert.ToString(Request.Params["ola_cod_empresa"]);
            string ola_cod_cargo = Convert.ToString(Request.Params["ola_cod_cargo"]);
            //string ola_nombre = "Auxiliar";
            //int fk_ubigeo = 173;
            //string ola_cod_empresa = "22";
            //string ola_cod_cargo = "001";
            int ola_id = 1;
            string errormensaje = "";
            var lista = new List<ofertaLaboralEntidad>();

            try
            {
                lista = ofertaLaboralbl.OfertaLaboralListarJson(ola_cod_empresa,ola_cod_cargo,ola_nombre);
                errormensaje = "Paso por Aqui";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
    }
}