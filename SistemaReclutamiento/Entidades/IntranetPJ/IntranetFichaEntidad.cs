using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.IntranetPJ
{
    public class IntranetFichaEntidad
    {
    }

    public class cum_usuario
    {
        public int cus_id { get; set; }
        public string cus_dni { get; set; }
        public string cus_tipo { get; set; }
        public string cus_correo { get; set; }
        public string cus_clave { get; set; }
        public string cus_firma { get; set; }
        public DateTime cus_fecha_reg { get; set; }
        public DateTime cus_fecha_act { get; set; }
        public string cus_estado { get; set; }
        public int fk_usuario { get; set; }
    }

    public class cum_envio
    {
        public int env_id { get; set; }
        public string env_nombre { get; set; }
        public string env_tipo { get; set; }
        public DateTime env_fecha_reg { get; set; }
        public DateTime env_fecha_act { get; set; }
        public string env_estado { get; set; }
        public int fk_cuestionario { get; set; }
        public int fk_usuario { get; set; }

        public string per_nombre { get; set; }
        public string per_apellido_pat { get; set; }
        public string end_correo_corp { get; set; }
        public string end_correo_pers { get; set; }
        public string per_apellido_mat { get; set; }
        public string cus_tipo { get; set; }

    }
}