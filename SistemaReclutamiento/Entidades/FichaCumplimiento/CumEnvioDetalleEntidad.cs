using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.FichaCumplimiento
{
    public class CumEnvioDetalleEntidad
    {
        public int end_id { get; set; }
        public string end_dni { get; set; }
        public string end_correo_corp { get; set; }
        public string end_correo_pers { get; set; }
        public DateTime end_fecha_reg { get; set; }
        public DateTime end_fecha_act { get; set; }
        public string end_estado { get; set; }
    }
}