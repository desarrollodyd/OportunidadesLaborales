using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class ofimaticaHerramientaEntidad
    {
        public int her_id { get; set; }
        public string her_descripcion { get; set; }
        public string her_estado { get; set; }
        public DateTime her_fecha_reg { get; set; }
        public DateTime her_fecha_act { get; set; }
    }
}