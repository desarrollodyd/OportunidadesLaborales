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
    public class IntranetMenuController : Controller
    {
        IntranetMenuModel intranetMenubl = new IntranetMenuModel();
        IntranetSeccionModel intranetSeccionbl = new IntranetSeccionModel();
        claseError error = new claseError();
        // GET: IntranetMenu
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IntranetMenuListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetMenuEntidad> listaMenus = new List<IntranetMenuEntidad>();
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuListarJson();
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
        public ActionResult IntranetMenuSeccionListarJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetMenuEntidad> listaMenus = new List<IntranetMenuEntidad>();
            List<IntranetSeccionEntidad> listaSecciones = new List<IntranetSeccionEntidad>();
            var datos = new List<dynamic>();
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuListarJson();
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

                foreach (var term in listaMenus)
                {
                    var seccionTupla = intranetSeccionbl.IntranetSeccionListarTodoxMenuIDJson(term.menu_id);
                    error = seccionTupla.error;
                    listaSecciones = seccionTupla.intranetSeccionListaTodoxMenuID;
                    datos.Add(new
                    {
                        term.menu_id,
                        term.menu_orden,
                        term.menu_titulo,
                        secciones = listaSecciones
                    }); ;
                }


            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = datos.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }


        [HttpPost]
        public ActionResult IntranetMenuListarTodoJson()
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            List<IntranetMenuEntidad> listaMenus = new List<IntranetMenuEntidad>();
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuListarTodoJson();
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
            return Json(new { data = listaMenus.ToList(), respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetMenuIdObtenerJson(int menu_id)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            IntranetMenuEntidad menu = new IntranetMenuEntidad();
            try
            {
                var menuTupla = intranetMenubl.IntranetMenuIdObtenerJson(menu_id);
                error = menuTupla.error;
                menu = menuTupla.intranetMenu;
                if (error.Key.Equals(string.Empty))
                {
                    mensaje = "Obteniendo Informacion del Menu Seleccionado";
                    respuesta = true;
                }
                else
                {
                    mensajeConsola = error.Value;
                    mensaje = "No se Pudo Obtener La Informacionb del Menu Seleccionado";
                }

            }
            catch (Exception exp)
            {
                mensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = menu, respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola });
        }
        [HttpPost]
        public ActionResult IntranetMenuNuevoJson(IntranetMenuEntidad intranetMenu)
        {
            string mensaje = "";
            string mensajeConsola = "";
            bool respuesta = false;
            int idIntranetMenuInsertado=0;
            int totalRegistros = 0;
            claseError error = new claseError();
            try
            {
                var totalMenuTupla = intranetMenubl.IntranetMenuObtenerTotalRegistrosJson();
                error = totalMenuTupla.error;
                if (error.Key.Equals(string.Empty))
                {
                    totalRegistros = totalMenuTupla.intranetMenuTotal;
                    intranetMenu.menu_orden = totalRegistros + 1;
                    var menuTupla = intranetMenubl.IntranetMenuInsertarJson(intranetMenu);
                    error = menuTupla.error;

                    if (error.Key.Equals(string.Empty))
                    {
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

            return Json(new { respuesta = respuesta, mensaje = mensaje,mensajeconsola=mensajeConsola});
        }
        [HttpPost]
        public ActionResult IntranetMenuEditarJson(IntranetMenuEntidad intranetMenu)
        {
            string errormensaje = "";
            bool respuestaConsulta = false;
            string mensajeConsola = "";
            claseError error = new claseError();
            if(intranetMenu.menu_id>0){
                try
                {
                    var menuTupla = intranetMenubl.IntranetMenuEditarJson(intranetMenu);
                    error = menuTupla.error;
                    if (error.Key.Equals(string.Empty))
                    {
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
            }
            else{
                errormensaje = "Error, falta Menu ID";
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

        [HttpPost]
        public ActionResult IntranetMenuEliminarVariosJson(int[] listaMenuEliminar) {
            string errormensaje = "";
            string mensajeConsola = "";
            bool respuestaConsulta = false;
            claseError error = new claseError();
            try
            {
                for (int i = 0; i <= listaMenuEliminar.Length - 1; i++) {
                    var menuTupla = intranetMenubl.IntranetMenuEliminarJson(listaMenuEliminar[i]);
                    error = menuTupla.error;
                    if (error.Key.Equals(string.Empty))
                    {
                        respuestaConsulta = menuTupla.intranetMenuEliminado;
                        errormensaje = "Menus Eliminados";
                    }
                    else
                    {
                        errormensaje = "Error, no se Puede Eliminar";
                        mensajeConsola = error.Value;
                    }
                }
                respuestaConsulta = true;
            }
            catch (Exception ex) {
                errormensaje = "Error, no se Puede Eliminar, " + ex.Message;
                respuestaConsulta = false;
            }

            return Json(new { respuesta = respuestaConsulta , mensaje = errormensaje, mensajeconsola = mensajeConsola });
        }

        [HttpPost]
        public ActionResult IntranetMenuEditarOrdenJson(IntranetMenuEntidad[] arrayMenus) {
            IntranetMenuEntidad intranetMenu = new IntranetMenuEntidad();
            claseError error = new claseError();
            bool response = false;
            string errormensaje = "";
            int tamanio = arrayMenus.Length;
            foreach (var m in arrayMenus) {
                intranetMenu.menu_id = m.menu_id;
                intranetMenu.menu_orden = m.menu_orden;
                var reordenadoTupla = intranetMenubl.IntranetMenuEditarOrdenJson(intranetMenu);
                error = reordenadoTupla.error;
                if (error.Key.Equals(string.Empty)) {
                    response = reordenadoTupla.intranetMenuReordenado;
                    errormensaje = "Editado";
                }
                else
                {
                    response = false;
                    errormensaje = "No se Pudo Editar";
                    return Json(new { respuesta = response, mensaje =errormensaje , mensajeconsola = "" });
                }
            }
            return Json(new {tamaniomenu= tamanio,respuesta = response, mensaje = errormensaje, mensajeconsola = "" });
        }
    }
}