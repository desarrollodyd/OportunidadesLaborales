using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class estOfimaticaEntidad
    {
        public int eof_id { get; set; }
        public string eof_nombre { get; set; }
        public string eof_estado { get; set; }
        public DateTime eof_fecha_reg { get; set; }
        public DateTime eof_fecha_act { get; set; }
    }
}