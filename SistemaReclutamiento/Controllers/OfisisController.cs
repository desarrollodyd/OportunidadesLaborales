using SistemaReclutamiento.Entidades;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    [autorizacion(false)]
    public class OfisisController : Controller
    {
        SQLModel sqlbl = new SQLModel();
        [HttpGet]
        public ActionResult PersonaPorDniFechaCese(string dni="")
        {
            PersonaSqlEntidad personaSQL = new PersonaSqlEntidad();
            try
            {
                personaSQL = sqlbl.PersonaPorDniFechaCese(dni);

            }
            catch (Exception ex)
            {
            }

            return Json(new {data=personaSQL }, JsonRequestBehavior.AllowGet);
        }
    }
}