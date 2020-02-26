using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.WebCorporativa
{
    public class WebSeccionEntidad
    {
        public int sec_id { get; set; }
        public string sec_estado { get; set; }
        public string sec_orden { get; set; }
        public int fk_menu { get; set; }
    }
}