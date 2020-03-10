using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.Postulante
{
    public class PosSeleccionEntidad
    {
        public int spo_id { get; set; }
        public int spo_nivel1_calif { get; set; }
        public bool spo_nivel1_selec { get; set; }
        public int spo_nivel2_calif { get; set; }
        public bool spo_nivel2_selec { get; set; }
        public int spo_nivel3_calif { get; set; }
        public bool spo_nivel3_selec { get; set; }
        public int spo_nivel4_calif { get; set; }
        public bool spo_nivel4_selec { get; set; }
        public int spo_nivel5_calif { get; set; }
        public bool spo_nivel5_selec { get; set; }
        public int spo_nivel6_calif { get; set; }
        public bool spo_nivel6_selec { get; set; }
        public int spo_nivel7_calif { get; set; }
        public bool spo_nivel7_selec { get; set; }
        public int spo_total_calif { get; set; }
        public int spo_total_selec { get; set; }
        public DateTime spo_fecha_reg { get; set; }
        public DateTime spo_fecha_act { get; set; }
        public int fk_postulacion { get; set; }
        public int fk_usuario { get; set; }
    }
}