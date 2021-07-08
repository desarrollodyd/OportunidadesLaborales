using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.BoletasGDT
{
    public class BolBitacoraEntidad
    {
        public int btc_id { get; set; }
        public int btc_usuario_id{ get; set; }
        public string btc_accion { get; set; }
        public string btc_vista { get; set; }
        public DateTime btc_fecha_reg { get; set; }
        public int btc_estado { get; set; }
        public string btc_co_trab { get; set; }
        public string btc_ruta_pdf { get; set; }
        public UsuarioEntidad Usuario { get; set; }
        public BolBitacoraEntidad()
        {
            this.Usuario = new UsuarioEntidad();
        }
    }
}