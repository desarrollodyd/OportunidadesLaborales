using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.WebCorporativa
{
    public class WebCorporativaController : Controller
    {
        // GET: WebCorporativa
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult apuestas()
        {
            return View();
        }

        public ActionResult noticias()
        {
            return View();
        }

        public ActionResult conocenos()
        {
            return View();
        }
    }
}