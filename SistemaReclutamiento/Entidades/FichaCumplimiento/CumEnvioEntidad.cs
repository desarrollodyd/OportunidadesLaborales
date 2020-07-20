using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class CumEnvioEntidad
    {
        public int env_id { get; set; }
        public string env_nombre { get; set; }
        public string env_tipo { get; set; }
        public DateTime env_fecha_reg { get; set; }
        public DateTime env_fecha_act { get; set; }
        public string env_estado { get; set; }
        public int fk_cuestionario { get; set; }
        public int fk_usuario { get; set; }
    }
}