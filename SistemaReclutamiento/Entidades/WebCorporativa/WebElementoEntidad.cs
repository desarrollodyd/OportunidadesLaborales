using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.WebCorporativa
{
    public class WebElementoEntidad
    {
        public int elm_id { get; set; }
        public string elm_contenido { get; set; }
        public int elm_orden { get; set; }
        public int fk_menu { get; set; }
        public int fk_tipo { get; set; }
    }
}