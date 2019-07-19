using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class educacionSuperiorEntidad
    {
        public int esu_id { get; set; }
        public string esu_tipo { get; set; }
        public string esu_centro_estudio { get; set; }
        public string esu_carrera { get; set; }
        public DateTime esu_periodo_ini { get; set; }
        public DateTime esu_periodo_fin { get; set; }
        public string esu_condicion { get; set; }
        public DateTime esu_fecha_reg { get; set; }
        public DateTime esu_fecha_act { get; set; }
        public int fk_postulante{get;set;}
    }
}