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
    public class IntranetSaludosCumpleaniosController : Controller
    {
        // GET: IntranetSaludosCumpleanios
        IntranetSaludoCumpleaniosModel intranetSaludoCumpleaniobl = new IntranetSaludoCumpleaniosModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetSaludoCumpleanioListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetSaludoCumpleanioEntidad> listaComentarios = new List<IntranetSaludoCumpleanioEntidad>();
            claseError error = new claseError();
            try
            {
                var saludoTupla = intranetSaludoCumpleaniobl.IntranetSaludoCumpleanioListarJson();
                error = saludoTupla.error;
                listaComentarios = saludoTupla.intranetSaludoCumpleanioLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Comentarios";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar los Comentarios";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaComentarios.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
    }
}