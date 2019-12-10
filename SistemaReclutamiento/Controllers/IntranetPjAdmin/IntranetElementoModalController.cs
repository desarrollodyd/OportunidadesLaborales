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
    public class IntranetElementoModalController : Controller
    {
        // GET: IntranetElementoModal
        IntranetElementoModalModel intranetElementoModalbl = new IntranetElementoModalModel();
        claseError error = new claseError();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetElementoModalListarxSeccionElementoJson(int fk_seccion_elemento)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetElementoModalEntidad> listaElementoModal = new List<IntranetElementoModalEntidad>();
            try
            {
                var DetalleElementoTupla = intranetElementoModalbl.IntranetElementoModalListarxSeccionElementoIDJson(fk_seccion_elemento);
                error = DetalleElementoTupla.error;
                listaElementoModal = DetalleElementoTupla.intranetElementoModalListaxseccionelementoID;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Elementos Modal";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar los Elementos Modales";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaElementoModal.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
    }
}