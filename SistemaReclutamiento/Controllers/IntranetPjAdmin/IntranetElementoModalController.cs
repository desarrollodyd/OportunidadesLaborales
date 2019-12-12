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
        [HttpPost]
        public ActionResult IntranetElementoModalInsertarJson(IntranetElementoModalEntidad intranetElementoModal)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetElementoModalInsertado = 0;
            claseError error = new claseError();
            try
            {
                var totalElementosTupla = intranetElementoModalbl.IntranetElementoModalObtenerTotalRegistrosxSeccionJson(intranetElementoModal.fk_seccion_elemento);
                error = totalElementosTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    intranetElementoModal.emod_orden = totalElementosTupla.intranetElementoModalTotal + 1;
                    var seccionTupla = intranetElementoModalbl.IntranetElementoModalInsertarJson(intranetElementoModal);
                    error = seccionTupla.error;

                    if (error.Key.Equals(string.Empty))
                    {
                        mensaje = "Se Registró Correctamente";
                        respuesta = true;
                        idIntranetElementoModalInsertado = seccionTupla.idIntranetElementoModalInsertado;
                    }
                    else
                    {
                        mensaje = "No se Pudo insertar el Elemento Modal";
                        mensajeConsola = error.Value;
                    }
                }
                else
                {
                    mensaje = "Error al Insertar el Nuevo Elemento";
                    mensajeConsola = error.Value;
                }


            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetElementoModalInsertado = idIntranetElementoModalInsertado, mensajeconsola = mensajeConsola });
        }
    }
    
}