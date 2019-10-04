using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class PostulanteEntidad
    {
        public int pos_id { get; set; }
        public string pos_condicion_viv { get; set; }
        public string pos_direccion { get; set; }
        public string pos_tipo_calle { get; set; }
        public string pos_numero_casa { get; set; }
        public string pos_tipo_casa { get; set; }
        public string pos_celular { get; set; }
        public string pos_estado_civil { get; set; }
        public bool pos_brevete { get; set; }
        public string pos_num_brevete { get; set; }
        public bool pos_referido { get; set; }
        public string pos_nombre_referido { get; set; }
        public string pos_cv { get; set; }
        public string pos_foto { get; set; }
        public string pos_situacion { get; set; }
        public DateTime pos_fecha_reg { get; set; }
        public DateTime pos_fecha_act { get; set; }
        public string pos_estado { get; set; }
        public int fk_usuario { get; set; }
        public int fk_nacionalidad { get; set; }
        public string pos_url_perfil { get; set; }
        public bool pos_familia_amigos { get; set; }
        public string pos_fam_ami_desc { get; set; }
        public bool pos_trabajo_pj { get; set; }
        public string pos_trab_pj_desc { get; set; }
    }
}