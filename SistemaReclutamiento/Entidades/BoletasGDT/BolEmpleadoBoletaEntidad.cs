using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.BoletasGDT
{
    public class BolEmpleadoBoletaEntidad
    {
        public string emp_co_trab { get; set; }
        public string emp_co_empr { get; set; }
        public string emp_anio { get; set; }
        public string emp_periodo { get; set; }
        public string emp_ruta_pdf { get; set; }
        public int emp_enviado { get; set; }
        public int emp_descargado { get; set; }
        public DateTime emp_fecha_act { get; set; }
        public DateTime emp_fecha_reg { get; set; }
        public string emp_no_trab { get; set; }
        public string emp_apel_pat { get; set; }
        public string emp_apel_mat { get; set; }
        public string emp_direc_mail { get; set; }
        public string emp_nro_cel { get; set; }
        public string emp_tipo_doc { get; set; }
        public string nombreEmpresa { get; set; }
        public bool boletaCreada { get; set; }
        public BolEmpleadoBoletaEntidad()
        {
            this.emp_enviado = 0;
            this.emp_descargado = 0;
            this.emp_fecha_act = DateTime.Now;
            this.emp_fecha_reg = DateTime.Now;
        }
    }
}