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
    public class IntranetImagenController : Controller
    {
        IntranetDetalleElementoModel intranetImagenbl = new IntranetDetalleElementoModel();
        claseError error = new claseError();
        // GET: IntranetImagen
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetImagenListarJson(int fk_layout)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetDetalleElementoEntidad> listaImagen = new List<IntranetDetalleElementoEntidad>();
            try
            {
                var ImagenTupla = intranetImagenbl.IntranetImagenListarJson();
                error = ImagenTupla.error;
                listaImagen = ImagenTupla.intranetImagenLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Imagenes";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Imagenes";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaImagen.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetImagenInsertarJson(IntranetDetalleElementoEntidad intranetImagen)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetImagenInsertado = 0;
            try
            {
                var ImagenTupla = intranetImagenbl.IntranetImagenInsertarJson(intranetImagen);
                error = ImagenTupla.error;

                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Se Registró Correctamente";
                    respuesta = true;
                    idIntranetImagenInsertado = ImagenTupla.idIntranetImagenInsertado;
                }
                else
                {
                    mensaje = "No se Pudo insertar las Imagen";
                    mensajeConsola = error.Value;
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetImagenInsertado = idIntranetImagenInsertado, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetImagenEditarJson(IntranetDetalleElementoEntidad intranetImagen)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            try
            {
                var ImagenTupla = intranetImagenbl.IntranetImagenEditarJson(intranetImagen);
                error = ImagenTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = ImagenTupla.intranetImagenEditado;
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
        public ActionResult IntranetImagenEliminarJson(int img_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            try
            {
                var ImagenTupla = intranetImagenbl.IntranetImagenEliminarJson(img_id);
                error = ImagenTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    respuestaConsulta = ImagenTupla.intranetImagenEliminado;
                    errormensaje = "Imagen Eliminada";
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