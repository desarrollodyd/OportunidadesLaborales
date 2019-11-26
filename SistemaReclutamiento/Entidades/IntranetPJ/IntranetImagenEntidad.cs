using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetImagenEntidad
    {
        public int img_id { get; set; }
        public string img_descripcion { get; set; }
        public string img_nombre { get; set; }
        public string img_extension { get; set; }
        public string img_ubicacion { get; set; }
        public string img_estado { get; set; }
        public int fk_elemento { get; set; }
        public int fk_seccion_elemento { get; set; }
    }
}