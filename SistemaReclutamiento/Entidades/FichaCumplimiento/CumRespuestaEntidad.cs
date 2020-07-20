using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class CumRespuestaEntidad
    {
        public int res_id { get; set; }
        public string res_respuesta { get; set; }
        public string res_tipo { get; set; }
        public int res_orden { get; set; }
        public int res_clasificacion { get; set; }
        public string res_estado { get; set; }
        public int fk_pregunta { get; set; }
        public int fk_usuario { get; set; }
    }
}