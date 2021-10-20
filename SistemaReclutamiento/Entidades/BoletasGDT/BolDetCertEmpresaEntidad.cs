using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.BoletasGDT
{
    public class BolDetCertEmpresaEntidad
    {
        public int det_id { get; set; }
        public string det_ruta_cert { get; set; }
        public string det_nomb_cert { get; set; }
        public string det_pass_cert { get; set; }
        public int det_estado_cert { get; set; }
        public int det_en_uso { get; set; }
        public int det_empr_id { get; set; }
    }
}