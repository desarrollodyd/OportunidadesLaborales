using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.BoletasGDT
{
    public class BolConfiguracionEntidad
    {
        public int config_id { get; set; }
        public string config_descripcion { get; set; }
        public int config_estado { get; set; }
        public string config_valor { get; set; }
        public string config_tipo { get; set; }
    }
}