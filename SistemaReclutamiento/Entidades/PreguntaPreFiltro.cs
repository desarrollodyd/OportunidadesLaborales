using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class PreguntaPreFiltro
    {
        public int ppf_id { get; set; }
        public string ppf_pregunta { get; set; }
        public string ppf_tipo { get; set; }
        public string ppf_resp1 { get; set; }
        public string ppf_resp2 { get; set; }
        public string ppf_porcentaje { get; set; }
        public DateTime ppf_fecha_reg { get; set; }
        public DateTime ppf_fecha_act { get; set; }
        public string ppf_estado { get; set; }
        public int fk_usuario { get; set; }
    }
}