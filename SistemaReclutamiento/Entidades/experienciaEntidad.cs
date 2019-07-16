using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class experienciaEntidad
    {
        public int exp_id { get; set; }
        public string exp_empresa { get; set; }
        public string exp_cargo { get; set;}
        public DateTime exp_fecha_ini { get; set; }
        public DateTime exp_fecha_fin { get; set; }
        public string exp_motivo_cese { get; set; }
        public DateTime exp_fecha_reg { get; set; }
        public DateTime exp_fecha_act { get; set; }
        public string exp_estado { get; set; }
        public int fk_postulante { get; set; }
    }
}