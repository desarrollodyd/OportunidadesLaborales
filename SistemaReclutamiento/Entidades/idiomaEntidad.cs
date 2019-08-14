using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class idiomaEntidad
    {
        public int idi_id { get; set; }
        public string idi_tipo { get; set; }
        public string idi_centro_estudio { get; set; }
        public int fk_idioma { get; set; }
        public DateTime idi_periodo_ini { get; set; }
        public DateTime idi_periodo_fin { get; set; }
        public string idi_nivel { get; set; }
        public DateTime idi_fecha_reg { get; set; }
        public DateTime idi_fecha_act { get; set; }
        public int fk_postulante { get; set; }
        public string eid_nombre { get; set; }
    }
}