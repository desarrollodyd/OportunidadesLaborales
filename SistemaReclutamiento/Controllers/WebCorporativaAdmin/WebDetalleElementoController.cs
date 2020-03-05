using SistemaReclutamiento.Entidades.WebCorporativa;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.WebCorporativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.WebCorporativaAdmin
{
    public class WebDetalleElementoController : Controller
    {
        WebDetalleElementoModel detallebl = new WebDetalleElementoModel();
        // GET: WebDetalleElemento
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WebDetalleElementoListarxElementoIDJson(int elem_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<WebDetalleElementoEntidad> listaDetalleElemento = new List<WebDetalleElementoEntidad>();
            claseError error = new claseError();
            try
            {
                var DetalleElementoTupla = detallebl.WebDetalleElementoListarxElementoIDJson(elem_id);
                error = DetalleElementoTupla.error;
                listaDetalleElemento = DetalleElementoTupla.listadetalle;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Detalle Elemento";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar los Detalles";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaDetalleElemento.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
    }
}