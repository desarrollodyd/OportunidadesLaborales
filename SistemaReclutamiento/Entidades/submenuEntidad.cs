using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class SubMenuEntidad
    {
        public string snu_descripcion { get; set; }
        public string snu_url { get; set; }
        public int snu_orden { get; set; }
        public string snu_icono { get; set; }
        public string snu_estado { get; set; }
        public int fk_menu { get; set; }
        public int snu_id { get; set; }
        public string snu_descripcion_eng { get; set; }
        public string snu_template { get; set; }
    }
}