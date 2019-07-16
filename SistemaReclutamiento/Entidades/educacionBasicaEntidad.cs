using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class educacionBasicaEntidad
    {
        public int eva_id { get; set; }
        public string eba_tipo { get; set; }
        public string eba_nombre { get; set; }
        public string eba_condicion { get; set; }
        public DateTime eba_fecha_reg { get; set; }
        public DateTime eba_fecha_act { get; set; }
        public int fk_postulante { get; set; }
    }
}