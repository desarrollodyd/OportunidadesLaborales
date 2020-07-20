using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class CumUsuPreguntaEntidad
    {
        public int upr_id { get; set; }
        public string upr_dni { get; set; }
        public string upr_pregunta { get; set; }
        public string upr_tipo { get; set; }
        public DateTime upr_fecha_reg { get; set; }
        public DateTime upr_fecha_act { get; set; }
        public string upr_estado { get; set; }
        public int fk_pregunta { get; set; }
        public int fk_usuario { get; set; }
        public ICollection<CumUsuRespuestaEntidad> CumUsuRespuesta { get; set; }
        public CumUsuPreguntaEntidad()
        {
            this.CumUsuRespuesta = new HashSet<CumUsuRespuestaEntidad>();
        }
    }
}