using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class PosPreguntaOLAEntidad
    {
        public int pol_id { get; set; }
        public string pol_pregunta { get; set; }
        public string pol_tipo { get; set; }
        public string pol_porcentaje { get; set; }
        public bool pol_acertiva { get; set; }
        public string pol_calificaion { get; set; }
        public int fk_postulacion { get; set; }
    }
}