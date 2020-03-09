using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class DetRespuestaOLAEntidad
    {
        public int dro_id { get; set; }
        public string dro_respuesta { get; set; }
        public string dro_tipo { get; set; }
        public int dro_orden { get; set; }
        public string dro_estado { get; set; }
        public int fk_det_pregunta_of { get; set; }
        public int dro_calificacion { get; set; }
    }
}