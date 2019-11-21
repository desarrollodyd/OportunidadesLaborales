using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetActividadesEntidad
    {
        public int act_id { get; set; }
        public string act_descripcion { get; set; }
        public int fk_icono { get; set; }
        public DateTime act_fecha { get; set; }
        public string act_estado { get; set; }
        public int fk_layout { get; set; }
    }
}