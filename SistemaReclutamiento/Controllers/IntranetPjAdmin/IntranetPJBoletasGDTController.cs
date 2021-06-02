using SistemaReclutamiento.Entidades.BoletasGDT;
using SistemaReclutamiento.Models;
using SistemaReclutamiento.Models.BoletasGDT;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Controllers.IntranetPjAdmin
{
    public class IntranetPJBoletasGDTController : Controller
    {
        string PathDirectorioBaseBoletasGDT = ConfigurationManager.AppSettings["PathDirectorioBaseBoletasGDT"].ToString();
        SQLModel sqlbl = new SQLModel();
        BolConfiguracionModel bolConfigBL = new BolConfiguracionModel();
        [HttpPost]
        public ActionResult CrearDirectorio()
        {
            string[] meses = {"Enero","Febrero","Marzo","Abril","Mayo","Junio","Julio","Agosto","Setiembre","Octubre","Noviembre","Diciembre" };
            bool respuesta = false;
            string mensaje = "";
            List<TMEMPR> listaempresa = new List<TMEMPR>();
            claseError error = new claseError();
            try
            {
                var sqltupla = sqlbl.EmpresaListarJson();
                listaempresa = sqltupla.listaempresa;
                error = sqltupla.error;
                if (error.Value.Equals(string.Empty))
                {
                    int anio = DateTime.Now.Year;
                }
            }
            catch(Exception ex)
            {

            }
            return Json(new {mensaje,respuesta });
        }
        #region Configuracion Boletas
        [HttpPost]
        public ActionResult BoolConfiguracionObtenerxTipoJson(string tipo)
        {
            string mensaje = "";
            bool respuesta = false;
            BolConfiguracionEntidad configuracion = new BolConfiguracionEntidad();

            try
            {
                var configuracionTupla = bolConfigBL.BoolConfiguracionObtenerxTipoJson(tipo);
                if (configuracionTupla.error.Value.Equals(string.Empty))
                {
                    configuracion = configuracionTupla.configuracion;
                    respuesta = true;
                    mensaje = "Obteniendo Registro";
                }
                else
                {
                    mensaje = configuracionTupla.error.Value;
                }
            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { mensaje, respuesta, data = configuracion });
        }
        #endregion

    }
}