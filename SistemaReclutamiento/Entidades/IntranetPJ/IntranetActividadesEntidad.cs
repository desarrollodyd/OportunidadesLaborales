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
        public int fk_imagen { get; set; }
        public DateTime act_fecha { get; set; }
        public string act_estado { get; set; }
        //imagen
        public string img_ubicacion { get; set; }
        //cumpleaños de persona para listado de noticias
        public string per_nombre { get; set; }
        public string per_apellido_pat { get; set; }
        public string per_apellido_mat { get; set; }
        public string per_fechanacimiento { get; set; }
    }
}