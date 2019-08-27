using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Entidades;

namespace SistemaReclutamiento.Controllers
{
    public class UbigeoController : Controller
    {
        ubigeoModel ubigeobl = new ubigeoModel();
        // GET: Ubigeo
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UbigeoListarPaisesJson()
        {
            var errormensaje = "";
            var lista = new List<ubigeoEntidad>();
            try
            {
                lista = ubigeobl.UbigeoListarPaisesJson();
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult UbigeoListarDepartamentosporPaisJson(string ubi_pais_id)
        {
            var errormensaje = "";
            var lista = new List<ubigeoEntidad>();
            try
            {
                lista = ubigeobl.UbigeoListarDepartamentosporPaisJson(ubi_pais_id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult UbigeoListarProvinciasporDepartamentoJson(string ubi_pais_id, string ubi_departamento_id)
        {
            var errormensaje = "";
            var lista = new List<ubigeoEntidad>();
            try
            {
                lista = ubigeobl.UbigeoListarProvinciasporDepartamentoJson(ubi_pais_id,ubi_departamento_id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult UbigeoListarDistritosporProvinciaJson(string ubi_pais_id, string ubi_departamento_id,string ubi_provincia_id)
        {
            var errormensaje = "";
            var lista = new List<ubigeoEntidad>();
            try
            {
                lista = ubigeobl.UbigeoListarDistritosporProvinciaJson(ubi_pais_id, ubi_departamento_id,ubi_provincia_id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult UbigeoObtenerDatosporIdJson(int ubi_id)
        {
            var errormensaje = "";
            var ubigeo = new ubigeoEntidad();
            try
            {
                ubigeo = ubigeobl.UbigeoObtenerDatosporIdJson(ubi_id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = ubigeo, mensaje = errormensaje });
        }
    }
}