using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.FichaCumplimiento
{
    public class CumCuestionarioEntidad
    {
        public string cue_descripcion { get; set; }
        public string cue_esado { get; set; }
        public DateTime cue_fecha_act { get; set; }
        public DateTime cue_fecha_reg { get; set; }
        public int cue_id { get; set; }
        public string cue_nombre { get; set; }
        public int cue_fk_usuario { get; set; }
    }
}