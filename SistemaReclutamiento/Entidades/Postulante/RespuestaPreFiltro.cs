using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class RespuestaPreFiltro
    {
        public int rpf_id { get; set; }
        public string rpf_respuesta { get; set; }
        public string rpf_tipo { get; set; }
        public int rpf_orden { get; set; }
        public string rpf_estado { get; set; }
        public int fk_pregunta_pref { get; set; }
    }
}