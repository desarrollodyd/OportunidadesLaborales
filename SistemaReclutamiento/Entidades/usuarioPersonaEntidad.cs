using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class usuarioPersonaEntidad
    {
        public int usu_id { get; set; }
        public int per_id { get; set; }
        public string usu_nombre { get; set; }
        public string usu_contrasenia { get; set; }
        public string usu_estado { get; set; }
        public string per_tipodoc { get; set; }
        public string per_numdoc { get; set; }
        public string per_nombre { get; set; }
        public string per_apellido_pat { get; set; }
        public string per_apellido_mat { get; set; }
        public string per_correoelectronico { get; set; }
        public string ubi_departamento_id { get; set; }
        public string ubi_distrito_id { get; set; }
        public string ubi_provincia_id { get; set; }
        public string ubi_pais_id { get; set; }
    }
}