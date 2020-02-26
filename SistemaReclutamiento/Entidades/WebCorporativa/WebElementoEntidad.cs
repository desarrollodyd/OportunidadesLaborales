using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.WebCorporativa
{
    public class WebElementoEntidad
    {
        public int elm_id { get; set; }
        public string elm_titulo { get; set; }
        public string elm_subtitulo { get; set; }
        public string elm_parrafo { get; set; }
        public string elm_imagen { get; set; }
        public int elm_orden { get; set; }
        public int fk_seccion { get; set; }
    }
}