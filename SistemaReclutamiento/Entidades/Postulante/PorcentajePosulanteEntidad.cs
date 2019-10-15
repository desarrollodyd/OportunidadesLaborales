using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class PorcentajePosulanteEntidad
    {
        public string usu_nombre { get; set; }
        public string usu_contrasenia { get; set; }
        public string usu_estado { get; set; }
        public string per_tipodoc { get; set; }
        public string per_numdoc { get; set; }
        public string per_nombre { get; set; }
        public string per_apellido_pat { get; set; }
        public string per_apellido_mat { get; set; }
        public DateTime per_fechanacimiento { get; set; }
        public string per_telefono { get; set; }
        public string per_celular { get; set; }
        public string per_sexo { get; set; }
        //Datos de Ubigeo
        public string ubi_departamento_id { get; set; }
        public string ubi_distrito_id { get; set; }
        public string ubi_provincia_id { get; set; }
        public string ubi_pais_id { get; set; }
        //Datos de Postulante

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
        public string pos_url_perfil { get; set; }
        public string fk_nacionalidad { get; set; }
        public bool pos_familia_amigos { get; set; }
        public string pos_fam_ami_desc { get; set; }
        public bool pos_trabajo_pj { get; set; }
        public string pos_trab_pj_desc { get; set; }
        public int educacionBasica { get; set; }
        public int educacionSuperior { get; set; }
        public int fimatica { get; set; }
        public int idiomas { get; set; }
        public int xperiencia { get; set; }
        public int postgrado { get; set; }
    }
}