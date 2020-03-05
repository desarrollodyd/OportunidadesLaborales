using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.WebCorporativa
{
    public class WebElementoEntidad
    {
        public int elem_id { get; set; }
        public string elem_contenido { get; set; }
        public int elem_orden { get; set; }
        public int fk_menu { get; set; }
        public int fk_tipo_elemento { get; set; }
        public string tipo_nombre { get; set; }
        public string elem_estado { get; set; }
    }
}