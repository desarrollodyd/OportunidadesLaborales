using SistemaReclutamiento.Entidades.IntranetPJ;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.IntranetPJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPJ
{
    public class IntranetMenuController : Controller
    {
        IntranetMenuModel intranetMenubl = new IntranetMenuModel();
        claseError error = new claseError();
        // GET: IntranetMenu
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetMenuListarJson(int fk_layout)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetMenuEntidad> listaMenus = new List<IntranetMenuEntidad>();
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuListarJson(fk_layout);
                error = menuTupla.error;
                listaMenus = menuTupla.intranetMenuLista;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Listando Menus";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudieron Listar los Menus";
                }
                
            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = listaMenus.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola=mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetMenuInsertarJson(IntranetMenuEntidad intranetMenu)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetMenuInsertado=0;
            claseError error = new claseError();
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuInsertarJson(intranetMenu);
                error = menuTupla.error;
               
                if (error.Key.Equals(string.Empty)){
                    mensaje = "Se Registró Correctamente";
                    respuesta = true;
                    idIntranetMenuInsertado = menuTupla.idIntranetMenuInsertado;
                }
                else
                {
                    mensaje = "No se Pudo insertar el Menu";
                    mensajeConsola = error.Value;
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje, idIntranetMenuInsertado=idIntranetMenuInsertado,mensajeconsola=mensajeConsola});
        }
        [HttpPost]
        public ActionResult IntranetMenuEditarJson(IntranetMenuEntidad intranetMenu)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            claseError error = new claseError();
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuEditarJson(intranetMenu);
                error = menuTupla.error;
                if (error.Key.Equals(string.Empty)) {
                    respuestaConsulta = menuTupla.intranetMenuEditado;
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

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje,mensajeconsola=mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetMenuEliminarJson(int menu_id)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            claseError error = new claseError();
            string mensajeConsola = "";
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuEliminarJson(menu_id);
                error = menuTupla.error;
                if (error.Key.Equals(string.Empty)) {
                    respuestaConsulta = menuTupla.intranetMenuEliminado;
                    errormensaje = "Menu Eliminado";
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
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje,mensajeconsola=mensajeConsola });
        }
    }
}