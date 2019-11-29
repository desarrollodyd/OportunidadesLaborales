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
        [HttpPost]
        public ActionResult IntranetMenuGuardarJson(IntranetMenuEntidad intranetMenu) {
            string mensaje = "";
            string mensajeConsola = "";
            string accion = "";
            bool respuesta = false;
            int idIntranetMenuInsertado = 0;
            claseError error = new claseError();
            if (intranetMenu.menu_id == 0)
            {
                //Insertar
                try
                {
                    var menuTupla = intranetMenubl.IntranetMenuInsertarJson(intranetMenu);
                    error = menuTupla.error;

                    if (error.Key.Equals(string.Empty))
                    {
                        mensaje = "Se Registró Correctamente";
                        respuesta = true;
                        idIntranetMenuInsertado = menuTupla.idIntranetMenuInsertado;
                        accion = "Insertado";
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

            }
            else {
                //Editar
                try
                {
                    var menuTupla = intranetMenubl.IntranetMenuEditarJson(intranetMenu);
                    error = menuTupla.error;
                    if (error.Key.Equals(string.Empty))
                    {
                        respuesta = menuTupla.intranetMenuEditado;
                        mensaje = "Se Editó Correctamente";
                        accion = "Editado";
                    }
                    else
                    {
                        mensajeConsola = error.Value;
                        mensaje = "Error, no se Puede Editar";
                    }
                }
                catch (Exception exp)
                {
                    mensaje = exp.Message + " ,Llame Administrador";
                }
            }
          

            return Json(new { respuesta = respuesta, mensaje = mensaje, mensajeconsola = mensajeConsola,accion=accion });
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
    }
}