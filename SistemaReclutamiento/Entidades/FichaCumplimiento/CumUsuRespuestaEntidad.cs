using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class CumUsuRespuestaEntidad
    {
        public int ure_id { get; set; }
        public string ure_dni { get; set; }
        public string ure_respuesta { get; set; }
        public string ure_tipo { get; set; }
        public int ure_orden { get; set; }
        public int ure_calificacion { get; set; }
        public string ure_estado { get; set; }
        public int fk_usu_pregunta { get; set; }
        public int fk_usuario { get; set; }
    }
}