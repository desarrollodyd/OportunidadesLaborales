using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class DetPreguntaOLAEntidad
    {
        public int dop_id { get; set; }
        public string dop_pregunta { get; set; }
        public string dop_tipo { get; set; }
        public string dop_resp1 { get; set; }
        public string dop_resp2 { get; set; }
        public string dop_porcentaje { get; set; }
        public int fk_oferta_laboral { get; set; }
        public ICollection<DetRespuestaOLAEntidad> DetalleRespuesta { get; set; }
        public DetPreguntaOLAEntidad() {
            this.DetalleRespuesta = new HashSet<DetRespuestaOLAEntidad>();
        }
    }
}