using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class submenuEntidad
    {
        private string snu_descripcion { get; set; }
        private string snu_url { get; set; }
        private int snu_orden { get; set; }
        private string snu_icono { get; set; }
        private string snu_estado { get; set; }
        private int fk_menu { get; set; }
        private int snu_id { get; set; }
        private string snu_descripcion_eng { get; set; }
        private string snu_template { get; set; }
    }
}