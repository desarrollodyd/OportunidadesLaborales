using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.FichaCumplimiento
{
    public class CumUsuarioEntidad
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
        public string cus_firma_act { get; set; }
        public ICollection<CumUsuPreguntaEntidad> CumUsuPregunta { get; set; }
        //data extra para mostrar en ficha sintomatologica
        public string nombre { get; set; }
        public string apellido_pat { get; set; }
        public string apellido_mat { get; set; }
        public string empresa { get; set; }
        public string ruc { get; set; }
        public string sede { get; set; }
        public string direccion { get; set; }
        public string celular { get; set; }
        public CumUsuarioEntidad()
        {
            this.CumUsuPregunta = new HashSet<CumUsuPreguntaEntidad>();
        }
    }
}