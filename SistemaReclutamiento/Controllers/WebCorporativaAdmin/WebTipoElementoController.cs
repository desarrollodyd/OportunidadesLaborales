using SistemaReclutamiento.Entidades.WebCorporativa;
using SistemaReclutamiento.Models.WebCorporativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.WebCorporativaAdmin
{
    public class WebTipoElementoController : Controller
    {
        WebTipoElementoModel tipoelementobl = new WebTipoElementoModel();
        // GET: WebTipoElemento
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WebTipoElementoListarJson()
        {
            string errormensaje = "";
            bool response = false;
            List<WebTipoElementoEntidad> lista = new List<WebTipoElementoEntidad>();
            try
            {
                var listaTupla = tipoelementobl.WebTipoElementoListarJson();
                if (listaTupla.error.Respuesta)
                {
                    lista = listaTupla.lista;
                    response = true;
                    errormensaje = "Listando Tipos";
                }
                else {
                    errormensaje = listaTupla.error.Mensaje;
                }
            }
            catch (Exception ex) {
                errormensaje = ex.Message;
            }
            return Json(new { respuesta=response,mensaje=errormensaje,data=lista.ToList()});
        }
    }
}