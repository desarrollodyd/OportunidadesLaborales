using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    public class IntranetDetalleElementoModalController : Controller
    {
        // GET: IntranetDetalleElementoModal
        IntranetDetalleElementoModalModel intranetDetalleElementoModalbl = new IntranetDetalleElementoModalModel();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IntranetDetalleElementoModalListarxElementoModalJson(int fk_elemento_modal)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetDetalleElementoModalEntidad> listaElementos = new List<IntranetDetalleElementoModalEntidad>();
            claseError error = new claseError();
            try
            {
                var ElementoTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalListarxElementoIDJson(fk_elemento_modal);
                error = ElementoTupla.error;
                listaElementos = ElementoTupla.intranetDetalleElementoModalListaxElementoID;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Detalle Elemento Modal";
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
            return Json(new { data = listaElementos.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
    }
}