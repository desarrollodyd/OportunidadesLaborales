using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class PosRespuestaOLAEntidad
    {
        public int rol_id { get; set; }
        public string rol_respuesta { get; set; }
        public string rol_tipo { get; set; }
        public int rol_orden { get; set; }
        public bool rol_elegida { get; set; }
        public string rol_calificacion { get; set; }
        public string rol_estado { get; set; }
        public int fk_pos_pregunta_ol { get; set; }
    }
}