using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    public class SuperController : Controller
    {
        // GET: Super
        public ActionResult Control()
        {
            return View("~/Views/Desarrollo/PanelPrincipal.cshtml");
        }
    }
}