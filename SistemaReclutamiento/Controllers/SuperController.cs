using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class SuperController : Controller
    {
        SubMenuModel submenubl = new SubMenuModel();
        PermisoModel permisobl = new PermisoModel();
        MenuModel menubl = new MenuModel();
        ModuloModel modulobl = new ModuloModel();
        // GET: Super
        public ActionResult Control()
        {
            return View("~/Views/Desarrollo/PanelPrincipal.cshtml");
        }
        public ActionResult CrearMenusView() {
            return View();
        }
        public ActionResult CrearSubmenusView(int men_id)
        {
            try
            {
                var menu = menubl.MenuIdObtenerJson(men_id);
                ViewBag.Menu = menu;
            }
            catch (Exception ex) {

            }
            return View();
        }
        [HttpPost]
        public ActionResult PermisosListarJson(int usu_id)
        {
            var errormensaje = "";
            var lista = new List<SubMenuEntidad>();
            var lista_menu_usuario = new List<PermisoEntidad>();
            bool response = false;
            try
            {
                lista = submenubl.SubMenuListarJson();
                // lista = uMenubl.UMenuListarJson();
                lista_menu_usuario = permisobl.PermisoListarUsuarioJson(usu_id);
                errormensaje = "listando Usuarios";
                response = true;
                //lista_menu_usuario = uMenuPermisobl.UMenuPermisoListarUsuarioJson(usu_id);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), data_lista_menu = lista_menu_usuario,respuesta=response, mensaje = errormensaje });
        }

        [HttpPost]
        public ActionResult SubMenuListarJson()
        {
            var errormensaje = "";
            SubMenuModel submenubl = new SubMenuModel();
            var lista = new List<SubMenuEntidad>();
            bool response = false;
            try
            {
                lista = submenubl.SubMenuListarJson();
               
                if (lista.Count > 0)
                {
                    errormensaje = "Listando Submenus";
                    response = true;
                }
                else {
                    errormensaje = "No Hay Datos";
                    response = false;
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
                response = false;
            }
            return Json(new { data = lista.ToList(), respuesta = response, mensaje = errormensaje });
        }
        public ActionResult SubMenuPermisoTodoInsertar(List<int> submenus, int fk_usuario)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var lista_menu_usuario = new List<PermisoEntidad>();
            try
            {
                foreach (var permisos in submenus)
                {
                    respuestaConsulta = permisobl.PermisoInsertar(permisos, fk_usuario);
                }
                errormensaje = "Permisos Insertados";
                lista_menu_usuario = permisobl.PermisoListarUsuarioJson(fk_usuario);
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, lista_menu_usuario = lista_menu_usuario, mensaje = errormensaje });
        }
        public ActionResult SubMenuPermisoTodoEliminar(List<int> submenus, int fk_usuario)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var lista_menu_usuario = new List<PermisoEntidad>();
            try
            {
                foreach (var permisos in submenus)
                {
                    respuestaConsulta = permisobl.PermisoQuitar(permisos, fk_usuario);
                }
                errormensaje = "Permisos Quitados";
                lista_menu_usuario = permisobl.PermisoListarUsuarioJson(fk_usuario);
             
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        public ActionResult SubMenuPermisoInsertar(int fk_submenu, int fk_usuario)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            var lista_menu_usuario = new List<PermisoEntidad>();
            try
            {
                respuestaConsulta = permisobl.PermisoInsertar(fk_submenu,fk_usuario);
                lista_menu_usuario = permisobl.PermisoListarUsuarioJson(fk_usuario);
                if (respuestaConsulta) {
                    errormensaje = "Permiso Guardado";
                }
                else
                {
                    errormensaje = "Error al Guardar Permiso";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, lista_menu_usuario = lista_menu_usuario, mensaje = errormensaje });
        }
        public ActionResult SubMenuPermisoQuitar(int fk_submenu, int fk_usuario)
        {
            var errormensaje = "";
            bool respuestaConsulta = true;
            var lista_menu_usuario = new List<PermisoEntidad>();
            try
            {
                respuestaConsulta = permisobl.PermisoQuitar(fk_submenu,fk_usuario);
                lista_menu_usuario = permisobl.PermisoListarUsuarioJson(fk_usuario);
                if (respuestaConsulta)
                {
                    errormensaje = "Permiso Quitado";
                }
                else
                {
                    errormensaje = "Error al Quitar Permiso";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, lista_menu_usuario = lista_menu_usuario, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult MenuInsertarJson(MenuEntidad menu)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            int idmoduloinsertado = 0;
            //int ultimoMenu = menubl.MenuObtenerUltimo();
            menu.men_id = menubl.MenuObtenerUltimo()+1 ;
            try
            {
                var modulo = modulobl.ModuloObtenerporTipoJson("Proveedor");
                if (modulo.mod_id > 0) {
                   
                    menu.fk_modulo = modulo.mod_id;
                    menu.men_estado = "A";
                    respuestaConsulta = menubl.MenuInsertarJson(menu);
                    if (respuestaConsulta)
                    {
                        errormensaje = "Se Registró Correctamente";
                    }
                    else
                    {
                        errormensaje = "Error, no se Puede Registrar";
                    }
                }
                else{
                    /*Crear modulo e insertar menu*/
                    ModuloEntidad moduloInsertar = new ModuloEntidad();
                    moduloInsertar.mod_descripcion = "Proveedores";
                    moduloInsertar.mod_descripcion_eng = "Providers";
                    moduloInsertar.mod_icono = "fa fa-paypal";
                    moduloInsertar.mod_tipo = "Proveedor";
                    moduloInsertar.mod_orden = 1;
                    moduloInsertar.mod_estado = "A";
                    idmoduloinsertado = modulobl.ModuloInsertarJson(moduloInsertar);
                    if (idmoduloinsertado > 0)
                    {
                        menu.fk_modulo = idmoduloinsertado;
                        menu.men_estado = "A";
                        respuestaConsulta = menubl.MenuInsertarJson(menu);
                        if (respuestaConsulta)
                        {
                            errormensaje = "Se Registró Correctamente";
                        }
                        else
                        {
                            errormensaje = "Error, no se Puede Registrar";
                        }
                    }
                    else {
                        errormensaje = "Error, no se Puede Registrar";
                    }
                   
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult MenuListarporTipoJson()
        {
            var errormensaje = "";
            var lista = new List<MenuEntidad>();
            bool response = false;
            try
            {
                lista = menubl.MenuListarporTipoJson();
                response = true;
                if (lista.Count > 0)
                {
                    errormensaje = "Cargando Data...";
                }
                else {
                    errormensaje = "No Hay Registros...";
                }
              
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult MenuEliminarJson(int men_id)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;

            try
            {
                respuestaConsulta = menubl.MenuEliminarJson(men_id);
                if (respuestaConsulta)
                {
                    errormensaje = "Se Eliminó Correctamente";
                }
                else
                {
                    errormensaje = "Error, no se Puede Eliminar";
                }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult SubMenuListarporMenuJson(int men_id)
        {
            var errormensaje = "";
            var lista = new List<SubMenuEntidad>();
            bool response = false;
            try
            {
                lista = submenubl.SubMenuListarPorMenu(men_id);
                response = true;
                if (lista.Count > 0)
                {
                    errormensaje = "Cargando Data...";
                }
                else
                {
                    errormensaje = "No Hay Registros...";
                }

            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = response, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult SubMenuInsertarJson(SubMenuEntidad submenu)
        {
            var errormensaje = "";
            bool respuestaConsulta = false;
            //int ultimoMenu = menubl.MenuObtenerUltimo();
            submenu.snu_id = submenubl.SubMenuObtenerUltimo() + 1;
            try
            {
                    submenu.snu_estado = "A";
                    respuestaConsulta = submenubl.SubMenuInsertarJson(submenu);
                    if (respuestaConsulta)
                    {
                        errormensaje = "Se Registró Correctamente";
                    }
                    else
                    {
                        errormensaje = "Error, no se Puede Registrar";
                    }
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + " ,Llame Administrador";
            }

            return Json(new { respuesta = respuestaConsulta, mensaje = errormensaje });
        }
    }
}