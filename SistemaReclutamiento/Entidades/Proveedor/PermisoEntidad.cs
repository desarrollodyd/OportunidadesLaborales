using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class PermisoEntidad
    {
        public string pem_estado { get; set; }
        public int fk_usuario { get; set; }
        public int fk_boton { get; set; }
        public int fk_submenu { get; set; }
        public DateTime pem_fecha_reg { get; set; }
        public int pem_usu_reg { get; set; }
    }
}