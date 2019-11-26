using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPJAdmin
{
    public class IntranetActividadesController : Controller
    {
        IntranetActividadesModel intranetActividadesbl = new IntranetActividadesModel();
        claseError error = new claseError();
        // GET: IntranetActividades
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetActividadesListarJson(int fk_layout)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetActividadesEntidad> listaActividades = new List<IntranetActividadesEntidad>();
            try
            {
                var ActividadesTupla = intranetActividadesbl.IntranetActividadesListarJson();
                error = ActividadesTupla.error;
                listaActividades = ActividadesTupla.intranetActividadesLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Actividadess";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Actividadess";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaActividades.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetActividadesInsertarJson(IntranetActividadesEntidad intranetActividades)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetActividadesInsertado = 0;
            try
            {
                var ActividadesTupla = intranetActividadesbl.IntranetActividadesInsertarJson(intranetActividades);
                error = ActividadesTupla.error;

                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Se Registró Correctamente";
                    respuesta = true;
                    idIntranetActividadesInsertado = ActividadesTupla.idIntranetActividadesInsertado;
                }
                else
                {
                    mensaje = "No se Pudo insertar las Actividades";
                    mensajeConsola = error.Value;
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetActividadesInsertado = idIntranetActividadesInsertado, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetActividadesEditarJson(IntranetActividadesEntidad intranetActividades)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            try
            {
                var ActividadesTupla = intranetActividadesbl.IntranetActividadesEditarJson(intranetActividades);
                error = ActividadesTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = ActividadesTupla.intranetActividadesEditado;
                    errormensaje = "Se Editó Correctamente";
                }
                else
                {
                    mensajeConsola = error.Value;
                    errormensaje = "Error, no se Puede Editar";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetActividadesEliminarJson(int act_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            try
            {
                var ActividadesTupla = intranetActividadesbl.IntranetActividadesEliminarJson(act_id);
                error = ActividadesTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = ActividadesTupla.intranetActividadesEliminado;
                    errormensaje = "Actividades Eliminado";
                }
                else
                {
                    errormensaje = "Error, no se Puede Eliminar";
                    mensajeConsola = error.Value;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }
    }
}