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
        IntranetDetalleElementoModalModel intranetDetalleElementoModalbl = new IntranetDetalleElementoModalModel();
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
        [HttpPost]
        public ActionResult IntranetElementoModalEliminarJson(int emod_id) {
            string mensaje = "";
            bool response = false;
            claseError error = new claseError();
            try {
                var intranetDetalleElementoModalTupla = intranetDetalleElementoModalbl.IntranetDetalleElementoModalEliminarxElementoModalJson(emod_id);
                error = intranetDetalleElementoModalTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    //Se eliminaron los detalles, ahora eliminamos el elemento modal
                    var intranetElementoModalTupla = intranetElementoModalbl.IntranetElementoModalEliminarJson(emod_id);
                    error = intranetElementoModalTupla.error;
                    if (error.Key.Equals(string.Empty))
                    {
                        response = true;
                        mensaje = "Se Elimino Correctamente";
                    }
                    else
                    {
                        mensaje = error.Value;
                    }
                }
                else {
                    mensaje = error.Value;
                }
            }
            catch (Exception ex){
                mensaje = ex.Message;
            }
            return Json(new { respuesta=response,mensaje=mensaje });
        }
        [HttpPost]
        public ActionResult IntranetElementoModalIdObtenerJson(int emod_id) {
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            IntranetElementoModalEntidad elementoModal = new IntranetElementoModalEntidad();
            try
            {
                var elementoModaltupla = intranetElementoModalbl.IntranetElementoModalIdObtenerJson(emod_id);
                error = elementoModaltupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    elementoModal = elementoModaltupla.intranetElementoModal;
                    response = true;
                    errormensaje = "Obteniendo Data";
                }
                else {
                    errormensaje = error.Value;
                }
            }
            catch (Exception ex)
            {
                errormensaje = ex.Message + ",Llame al Administrador";
            }
            return Json(new { data = elementoModal, respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult IntranetElementoModalEditarJson(IntranetElementoModalEntidad intranetElementoModal) {
            string errormensaje = "";
            bool response = false;
            claseError error = new claseError();
            try
            {
                intranetElementoModal.emod_contenido = intranetElementoModal.emod_titulo;
                intranetElementoModal.emod_descripcion = intranetElementoModal.emod_titulo;
                var intranetElementModalTupla = intranetElementoModalbl.IntranetElementoModalEditarJson(intranetElementoModal);
                error = intranetElementModalTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    response = intranetElementModalTupla.intranetElementoModalEditado;
                }
                else {
                    errormensaje = error.Value;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message+",Llame al Administrador";
            }
            return Json(new { respuesta = response, mensaje = errormensaje });
        }
    }
    
}