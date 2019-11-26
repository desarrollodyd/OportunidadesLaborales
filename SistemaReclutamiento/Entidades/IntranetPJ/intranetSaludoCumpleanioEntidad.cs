using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetSaludoCumpleanioEntidad
    {
        public int sld_id { get; set; }
        public string sld_cuerpo { get; set; }
        public string sld_estado { get; set; }
        public DateTime sld_fecha_envio { get; set; }
        public int fk_persona { get; set; }
        
    }
}