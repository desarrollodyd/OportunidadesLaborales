using SistemaReclutamiento.Context;
using SistemaReclutamiento.Utilitarios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers
{
    [autorizacion(false)]
    public class AdministrativoTestController : Controller
    {
        private AdministrativoDbContext _administrativoDbContext=new AdministrativoDbContext();
        [HttpPost]
        public JsonResult Test()
        {
            DateTime fechaInicio = new DateTime(2023, 3, 1);
            DateTime fechaFin = new DateTime(2023, 3, 1);

            //var result = _administrativoDbContext.DetalleContadoresGames.OrderBy(x => x.CodDetalleContadoresGame).Where(x=>x.FechaOperacion>=fechaInicio&&x.FechaOperacion<=fechaFin);
            //var otherResult= (from dg in (from dg in result
            //                              select new
            //                              {
            //                                  coinin = (decimal?)(dg.SaldoCoinIn * (decimal)0.1),
            //                                  extra = "extra",
            //                                  dg.FechaOperacion,
            //                              }
            //                            )
            //                  group dg by new { dg.FechaOperacion }
            //        into g
            //                  select new
            //                  {
            //                      coinin = g.Sum(p => p.coinin),
            //                      g.Key.FechaOperacion,
            //                  }
            //        ).ToList();
            var otherResult = _administrativoDbContext.DetalleContadoresGame.Where(x=>x.CodDetalleContadoresGame== 6481081).Include("EventosPorSlot").FirstOrDefault();
            return Json( new { data=otherResult });
        }
    }
}