using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class OfertaLaboralEntidad
    {
        public int ola_id { get; set; }
        public string ola_nombre { get; set; }
        public string ola_requisitos { get; set; }
        public string ola_funciones { get; set; }
        public string ola_competencias { get; set; }
        public string ola_condiciones_lab { get; set; }
        public int ola_vacantes { get; set; }
        public bool ola_enviar { get; set; }
        public bool ola_enviado { get; set; }
        public bool ola_publicado { get; set; }
        public DateTime ola_fecha_pub { get; set; }
        public string ola_estado_oferta { get; set; }
        public int ola_duracion { get; set; }
        public DateTime ola_fecha_fin { get; set; }
        public DateTime ola_fecha_reg { get; set; }
        public DateTime ola_fecha_act { get; set; }
        public string ola_estado { get; set; }
        public string ola_cod_empresa { get; set; }
        public string ola_cod_unidad { get; set; }
        public string ola_cod_sede { get; set; }
        public string ola_cod_gerencia { get; set; }
        public string ola_cod_area { get; set; }
        public string ola_cod_puesto { get; set; }
        public int fk_ubigeo { get; set; }
        public int fk_usuario { get; set; }
        public bool es_favorito { get; set; }
        public bool ya_postulo { get; set; }
    }
}