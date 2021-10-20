using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.BoletasGDT
{
    public class BolEmpresaEntidad
    {
        public int emp_id { get; set; }
        public string emp_co_ofisis { get; set; }
        public string emp_nomb { get; set; }
        public string emp_nomb_corto { get; set; }
        public string emp_depa { get; set; }
        public string emp_prov { get; set; }
        public string emp_rucs { get; set; }
        public string emp_pais { get; set; }
        public int emp_firma_visible { get; set; }
        public string emp_firma_img { get; set; }
        public string emp_nom_rep_legal { get; set; }
        public List<BolDetCertEmpresaEntidad> DetalleCerts { get; set; }
        public BolEmpresaEntidad()
        {
            this.DetalleCerts = new List<BolDetCertEmpresaEntidad>();
        }
    }
}