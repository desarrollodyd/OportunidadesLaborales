using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor

        public ActionResult Index()
        {
            string replace = "";
            ViewBag.controlleractionlist = ListarMetodosPorControlador("EducacionSuperior");
            //ViewBag.controllersnames = GetControllerNames();
            List<dynamic> lista = new List<dynamic>();
            foreach (var m in GetControllerNames()) {
                lista.Add(m.Replace("Controller", replace));
            }
            ViewBag.controllersnames = lista;
            return View();
        }
        [HttpPost]
        public ActionResult ModuloListarJson() {
            var errormensaje = "";
            ModuloModel modulobl = new ModuloModel();
            var lista = new List<ModuloEntidad>();
            try
            {
                lista = modulobl.ModuloListarJson();
                errormensaje = "Cargando Data...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
        }
        [HttpPost]
        public ActionResult RolListarJson()
        {
            var errormensaje = "";
            RolModel rolbl = new RolModel();
            var lista = new List<RolEntidad>();
            try
            {
                lista = rolbl.RolListarJson();
                errormensaje = "Cargando Data...";
            }
            catch (Exception exp)
            {
                errormensaje = exp.Message + ",Llame Administrador";
            }
            return Json(new { data = lista.ToList(), respuesta = true, mensaje = errormensaje });
        }
        public List<dynamic> ListarMetodosPorControlador(string controlador)
        {
            List<dynamic> listametodos = new List<dynamic>();
            string nombrecontrolador = controlador+"Controller";
            Assembly asm = Assembly.GetExecutingAssembly();
            listametodos = asm.GetTypes()
                                  .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                                  .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                                  .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                                  .Where(m => (System.Attribute.GetCustomAttributes(typeof(SeguridadMenu), true).Length > 0))
                                  //.Where(x=>String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))!= "SeguridadMenu")
                                  .Where(m => (String.Join(",", (m.GetCustomAttributesData()
                                                              .Where(f => f.Constructor.DeclaringType.Name == "SeguridadMenu")
                                                              .Select(c => (c.ConstructorArguments[0].Value))))
                                                        != "False"))
                                 .Where(m => m.DeclaringType.Name.Equals(nombrecontrolador))
                                  .Select(x => new
                                  {
                                      Controller = x.DeclaringType.Name,
                                      Action = x.Name,
                                      ReturnType = x.ReturnType.Name,
                                      Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                                  })
                                  .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList<dynamic>();
            return listametodos;

        }
        private static List<Type> GetSubClasses<T>()
        {
            List<Type> lista = new List<Type>();
            lista= Assembly.GetCallingAssembly().GetTypes().Where(
                type => type.IsSubclassOf(typeof(T))).ToList();
            return lista;
        }

        public List<string> GetControllerNames()
        {
            List<string> controllerNames = new List<string>();
            GetSubClasses<Controller>().ForEach(
                type => controllerNames.Add(type.Name));
            return controllerNames;
        }
    }
}