using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class EstIdiomaEntidad
    {
        public int eid_id { get; set; }
        public string eid_nombre { get; set; }
        public DateTime eid_fecha_reg { get; set; }
        public DateTime eid_fecha_act { get; set; }
        public string eid_estado { get; set; }
    }
}