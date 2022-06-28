using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.BoletasGDT
{
    public class BolEmailRemitenteEntidad
    {
        public int email_id { get; set; }
        public string email_nombre { get; set; }
        public string email_direccion { get; set; }
        public string email_password { get; set; }
        public bool email_ssl { get; set; }
        public string email_smtp { get; set; }
        public int email_puerto { get; set; }
        public int email_estado { get; set; }
        public int email_limite { get; set; }
        public int email_cantidad_envios { get; set; }
        public DateTime email_ultimo_envio { get; set; }
    }
}