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
        // GET: Super
        public ActionResult Control()
        {
            return View("~/Views/Desarrollo/PanelPrincipal.cshtml");
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
                respuestaConsulta = true;
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
    }
}