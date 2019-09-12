using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class RolEntidad
    {
        public int rol_id { get; set; }
        public string rol_nombre { get; set; }
        public string rol_estado { get; set; }
        public DateTime rol_fecha_reg { get; set; }
        public DateTime rol_fecha_act { get; set; }
        public int fk_usuario { get; set; }
    }
}