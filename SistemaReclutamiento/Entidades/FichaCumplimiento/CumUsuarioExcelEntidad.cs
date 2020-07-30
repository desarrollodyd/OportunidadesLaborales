using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class CumUsuarioExcelEntidad
    {
        public int cue_id { get; set; }
        public string cue_numdoc { get; set; }
        public string cue_correo { get; set; }
        public DateTime cue_fecha_reg { get; set; }
        public DateTime cue_fecha_act { get; set; }
    }
}