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
            //Assembly asm = Assembly.GetAssembly(typeof(SistemaReclutamiento.MvcApplication));

            //var controlleractionlist = asm.GetTypes()
            //        .Where(type => typeof(Controller).IsAssignableFrom(type))
            //        .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            //        .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
            //        .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name,
            //          Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
            //        .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            //ViewBag.controlleractionlist = controlleractionlist;

            Assembly asm = Assembly.GetExecutingAssembly();

            //ViewBag.controlleractionlist = asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type))
            //    .SelectMany(type => type.GetMethods(BindingFlags.Instance|BindingFlags.DeclaredOnly|BindingFlags.Public))
            //    .Where(m => (System.Attribute.GetCustomAttributes(typeof(SeguridadMenu), true).Length > 0))
            //    .Where(m => m.IsPublic && !m.IsDefined(typeof(NonActionAttribute)))
            //    .Select(x => new
            //    {
            //        Controller = x.DeclaringType.Name,
            //        Action = x.Name,
            //        ReturnType = x.ReturnType.Name,
            //        Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
            //    })
            //    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            ViewBag.controlleractionlist = asm.GetTypes()
                                    .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                                    .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                                    .Where(m => (System.Attribute.GetCustomAttributes(typeof(SeguridadMenu), true).Length > 0))
                                   // .Where(x=>String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))!= "HttpPost")
                                    .Select(x => new
                                    {
                                        Controller = x.DeclaringType.Name,
                                        Action = x.Name,
                                        ReturnType = x.ReturnType.Name,
                                        Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                                    })
                                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            return View();
        }
    }
}