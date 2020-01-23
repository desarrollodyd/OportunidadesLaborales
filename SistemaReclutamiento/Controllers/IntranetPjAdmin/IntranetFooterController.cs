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
    public class IntranetFooterController : Controller
    {
        // GET: IntranetFooter
        IntranetFooterModel intranetFooterbl = new IntranetFooterModel();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IntranetFooterInsertarJson(IntranetFooterEntidad intranetFooter) {
            claseError error = new claseError();
            string errormensaje = "";
            bool response = false;
            try
            {
                var intranetFooterTupla = intranetFooterbl.IntranetFooterInsertarJson(intranetFooter);
                error = intranetFooterTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    if (intranetFooterTupla.idIntranetFooterInsertado > 0)
                    {
                        response = true;
                    }
                    else {
                        errormensaje = "No se Pudo Insertar";
                    }
                }
                else {
                    errormensaje = error.Value;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { mensaje = errormensaje, respuesta = response });
        }
    }
}