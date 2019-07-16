using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class postgradoEntidad
    {
        public int pos_id { get; set; }
        public string pos_tipo { get; set; }
        public string pos_centro_estudio { get; set; }
        public string pos_carrera { get; set; }
        public string pos_nombre { get; set; }
        public DateTime pos_periodo_ini { get; set; }
        public DateTime pos_periodo_fin { get; set; }
        public string pos_condicion { get; set; }
        public DateTime pos_fecha_reg { get; set; }
        public DateTime pos_fecha_act { get; set; }
        public int fk_postulante { get; set; }
    }
}