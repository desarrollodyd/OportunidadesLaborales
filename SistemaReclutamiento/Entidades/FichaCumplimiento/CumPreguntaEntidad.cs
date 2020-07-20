using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class CumPreguntaEntidad
    {
        public int pre_id { get; set; }
        public string pre_pregunta { get; set; }
        public string pre_tipo { get; set; }
        public DateTime pre_fecha_act { get; set; }
        public DateTime pre_fecha_reg { get; set; }
        public string pre_estado { get; set; }
        public int fk_usuario { get; set; }
        public int fk_cuestionario { get; set; }
    }
}