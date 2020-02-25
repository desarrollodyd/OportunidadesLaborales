using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SistemaReclutamiento
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Postulante",
                url: "Postulante",
                defaults: new { controller = "Login", action = "PostulanteIndex" }
            );
            routes.MapRoute(
                name: "Proveedor",
                url: "Proveedor",
                defaults: new { controller = "Login", action = "ProveedorIndex"}
            );
            routes.MapRoute(
                name: "Intranet",
                url: "Intranet",
                defaults: new { controller = "IntranetPJ", action = "Index" }
            );
            routes.MapRoute(
                name: "IntranetAdmin",
                url: "SGC",
                defaults: new { controller = "IntranetPJAdmin", action = "Index" }
            );

            //Todas las demas rutas deben ir arriba de esta, para que no haya problemas
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "PostulanteIndex", id = UrlParameter.Optional }
            );
        }
    }
}
