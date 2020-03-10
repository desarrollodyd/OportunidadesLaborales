using SistemaReclutamiento.Entidades.WebCorporativa;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.WebCorporativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.WebCorporativaAdmin
{
    public class WebCorporativaAdminController : Controller
    {
        WebElementoModel elementobl = new WebElementoModel();
        WebDetalleElementoModel detallebl = new WebDetalleElementoModel();
        // GET: WebCorporativaAdmin
        public ActionResult Menus()
        {
            return View("~/Views/WebCorporativaAdmin/WebMenus.cshtml");
        }

        public ActionResult PanelDepartamento() {
            return View("~/Views/WebCorporativaAdmin/WebDepartamento.cshtml");
        }
        public ActionResult PanelSecciones()
        {
            return View("~/Views/WebCorporativaAdmin/WebSecciones.cshtml");
        }


        //////////////////////////////////////////////////////
        ///

        [HttpPost]
        public ActionResult WebElementoListarxMenuIDxtipoJson(int menu_id=1,int tipo=1,string nombreElemento="")
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<WebElementoEntidad> listaElementos = new List<WebElementoEntidad>();
            List<WebDetalleElementoEntidad> listaDetalleElemento = new List<WebDetalleElementoEntidad>();
            claseError error = new claseError();
            var data = new List<dynamic>();
            try
            {
                var ElementoTupla = elementobl.WebElementoListarxMenuIDxtipoJson(menu_id,tipo);
                error = ElementoTupla.error;
                listaElementos = ElementoTupla.lista;
                if (error.Key.Equals(string.Empty))
                {
                    foreach (var obj in listaElementos)
                    {
                        var elem_id = obj.elem_id;
                        var DetalleElementoTupla = detallebl.WebDetalleElementoListarxElementoIDJson(elem_id);
                        error = DetalleElementoTupla.error;
                        listaDetalleElemento = DetalleElementoTupla.listadetalle;
                        data.Add(new{elemento=obj,detalle=listaDetalleElemento });
                    }
                    mensaje = "Listando "+ nombreElemento;
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar las Elementoes";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = data.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }


    }
}