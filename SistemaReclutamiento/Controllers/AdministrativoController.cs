using OfficeOpenXml.DataValidation;
using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SistemaReclutamiento.Controllers
{
    [autorizacion(false)]
    public class AdministrativoController : Controller
    {
        DetalleMovAuxTitoModel detalleMovAuxTitoBL = new DetalleMovAuxTitoModel();

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ListarDetalleMovAuxTitoAdministrativo(string FechaIni, string FechaFin, string CodSala)
        {
            List<DetalleMovAuxTitoEntidad> resul = new List<DetalleMovAuxTitoEntidad>();
            bool respuesta = false;
            try
            {         
                resul = detalleMovAuxTitoBL.ListarDetalleMovAuxTitoAdministrativo(Convert.ToDateTime(FechaIni), Convert.ToDateTime(FechaFin), Convert.ToInt32(CodSala));
                respuesta = true;
            }
            catch (Exception)
            {
                resul = new List<DetalleMovAuxTitoEntidad>();
                throw;
            }

            //return Json(new { data = resul }, JsonRequestBehavior.AllowGet);

            //SERVER
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                respuesta,
                data = resul
            };
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return result;

        }
    }
}