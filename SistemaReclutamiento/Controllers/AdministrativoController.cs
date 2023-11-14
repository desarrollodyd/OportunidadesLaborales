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
        MaquinaDetalleModel maquinaDetalleBL = new MaquinaDetalleModel();
        DetalleContadoresGameModel detalleContadoreGamesBL = new DetalleContadoresGameModel();

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

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ListarMaquinaDetalleAdministrativo(string codMaquina) {
            MaquinaDetalleEntidad result = new MaquinaDetalleEntidad();
            bool respuesta = false;
            try {
                result = maquinaDetalleBL.ListarMaquinaDetalleAdministrativo(codMaquina);
                respuesta = true;
            } catch(Exception) {
                result = new MaquinaDetalleEntidad();
                throw;
            }

            //return Json(new { data = resul }, JsonRequestBehavior.AllowGet);

            //SERVER
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new {
                respuesta,
                data = result
            };
            var resul = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return resul;

        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ListarMaquinasAdministrativo() {
            List<MaquinaDetalleEntidad> result = new List<MaquinaDetalleEntidad>();
            bool respuesta = false;
            try {
                result = maquinaDetalleBL.ListarMaquinasAdministrativo();
                respuesta = true;
            } catch(Exception) {
                result = new List<MaquinaDetalleEntidad>();
                throw;
            }

            //return Json(new { data = resul }, JsonRequestBehavior.AllowGet);

            //SERVER
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new {
                respuesta,
                data = result
            };
            var resul = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return resul;

        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ListarMaquinasAdministrativoxSala(string codSala) {
            List<MaquinaDetalleEntidad> result = new List<MaquinaDetalleEntidad>();
            bool respuesta = false;
            try {
                result = maquinaDetalleBL.ListarMaquinasAdministrativo(codSala);
                respuesta = true;
            } catch(Exception) {
                result = new List<MaquinaDetalleEntidad>();
                throw;
            }

            //return Json(new { data = resul }, JsonRequestBehavior.AllowGet);

            //SERVER
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new {
                respuesta,
                data = result
            };
            var resul = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return resul;

        }
        [HttpPost]
        public ActionResult ListarDetalleContadoresPorFechaOperacion(DateTime fechaInicio,DateTime fechaFin, int codSala) {
            List<DetalleContadoresGameEntidad> result = new List<DetalleContadoresGameEntidad>();
            try {
                result = detalleContadoreGamesBL.ListarDetalleContadoresGamePorFechaOperacionYSala(fechaInicio,fechaFin,codSala);
            } catch(Exception) {
                result = new List<DetalleContadoresGameEntidad>();
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = new
            {
                result
            };
            var resul = new ContentResult {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };
            return resul;
        }

    }
}