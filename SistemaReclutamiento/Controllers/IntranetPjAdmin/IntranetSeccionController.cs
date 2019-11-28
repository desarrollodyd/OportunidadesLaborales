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
    public class IntranetSeccionController : Controller
    {
        // GET: IntranetSeccion
        IntranetSeccionModel intranetSeccionbl = new IntranetSeccionModel();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetSeccionListarxMenuIDJson(int menu_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetSeccionEntidad> listaMenus = new List<IntranetSeccionEntidad>();
            claseError error = new claseError();
            try
            {
                var seccionTupla = intranetSeccionbl.IntranetSeccionListarxMenuIDJson( menu_id);
                error = seccionTupla.error;
                listaMenus = seccionTupla.intranetSeccionListaxMenuID;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Secciones";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Secciones";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaMenus.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
    }
}