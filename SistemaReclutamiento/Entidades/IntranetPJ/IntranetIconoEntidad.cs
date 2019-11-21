using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetIconoEntidad
    {
        public int icon_id { get; set; }
        public string icon_descripcion { get; set; }
        public string icon_ubicacion { get; set; }
        public string icon_estado { get; set; }
        public int fk_layout { get; set; }
    }
}