using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetSliderEntidad
    {
        public int slid_id { get; set; }
        public int fk_menu { get; set; }
        public int fk_imagen { get; set; }
        public string slid_descripcion { get; set; }
        public string slid_estado { get; set; }
    }
}