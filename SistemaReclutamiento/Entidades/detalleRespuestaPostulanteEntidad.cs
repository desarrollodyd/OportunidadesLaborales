using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class DetalleRespuestaPostulanteEntidad
    {
        public int dre_id { get; set; }
        public string dre_pregunta { get; set; }
        public string dre_tipo { get; set; }
        public string dre_resp1 { get; set; }
        public string dre_resp2 { get; set; }
        public string dre_porcentaje { get; set; }
        public string dre_respuesta { get; set; }
        public int fk_postulante { get; set; }
        public int fk_oferta_laboral { get; set; }
    }
}