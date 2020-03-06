using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaReclutamiento.Entidades.WebCorporativa
{
    public class WebDetalleElementoEntidad 
    {
        public int detel_id { get; set; }
        public int fk_elemento { get; set; }
        public string detel_titulo { get; set; }
        public string detel_subtitulo { get; set; }
        [AllowHtml]
        public string detel_parrafo { get; set; }
        public string detel_imagen { get; set; }
        public string detel_imagen_detalle { get; set; }
        public string detel_estado { get; set; }
        public int detel_orden { get; set; }
        public int fk_tipo { get; set; }

        public string detel_nombre_imagen { get; set; }
        public string detel_nombre_imagen_detalle { get; set; }
    }
}