using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class MenuEntidad
    {
        public string men_descripcion { get; set; }
        public int men_orden { get; set; }
        public string men_icono { get; set; }
        public string men_estado { get; set; }
        public int men_id { get; set; }
        public string men_descripcion_eng { get; set; }
        public string men_tipo { get; set; }
        public int fk_modulo { get; set; }
    }
}