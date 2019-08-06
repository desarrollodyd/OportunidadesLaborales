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
            DateTime fecha_fin = DateTime.Now;
            DateTime fecha_ayuda;
            bool respuestaConsulta = false;
            string errormensaje = "";
            var lista = new List<ofertaLaboralEntidad>();
            if (reporte.ola_rango_fecha == "hoy")
            {
                reporte.ola_fecha_ini = DateTime.Parse(fecha_fin.ToShortDateString());
                //reporte.ola_fecha_ini = fecha_inicio;
            }
            if (reporte.ola_rango_fecha == "semana")
            {
                fecha_ayuda = fecha_fin.AddDays(-7);
                reporte.ola_fecha_ini = DateTime.Parse(fecha_ayuda.ToShortDateString());
            }
            if (reporte.ola_rango_fecha == "mes")
            {
                int dias = DateTime.DaysInMonth(fecha_fin.Year, fecha_fin.Month);
                fecha_ayuda = fecha_fin.AddDays(-dias);
                reporte.ola_fecha_ini = DateTime.Parse(fecha_ayuda.ToShortDateString());
            }
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