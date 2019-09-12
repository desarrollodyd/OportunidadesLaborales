using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class ModuloEntidad
    {
        public int mod_id { get; set; }
        public string mod_descripcion { get; set; }
        public string mod_descripcion_eng { get; set; }
        public string mod_tipo { get; set; }
        public int mod_orden { get; set; }
        public string mod_icono { get; set; }
        public string mod_estado { get; set; }
    }
}