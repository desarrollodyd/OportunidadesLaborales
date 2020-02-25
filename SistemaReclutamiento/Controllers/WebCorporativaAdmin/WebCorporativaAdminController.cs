using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.WebCorporativaAdmin
{
    public class WebCorporativaAdminController : Controller
    {
        // GET: WebCorporativaAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PanelDepartamento() {
            return View("~/Views/WebCorporativaAdmin/WebDepartamento.cshtml");
        }
    }
}