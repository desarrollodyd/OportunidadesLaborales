using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class TDINFO_TRAB
    {
        public string CO_TRAB { get; set; }
        public string CO_EMPR { get; set; }
        public string CO_PLAN { get; set; }
        public int NU_ANNO { get; set; }
        public int NU_PERI { get; set; }
        public int NU_CORR_PERI { get; set; }
        public string CO_CPTO_FORM { get; set; }
        public DateTime FE_INIC_VIGE { get; set; }
        public DateTime FE_FINA_VIGE { get; set; }
        public double NU_DATO_INFO { get; set; }
        public string CO_USUA_CREA { get; set; }
        public DateTime FE_USUA_CREA { get; set; }
        public string CO_USUA_MODI { get; set; }
        public DateTime FE_USUA_MODI { get; set; }
    }
}