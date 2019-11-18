using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.Proveedor
{
    public class CPCARTEntidad
    {
        public string CP_CVANEXO { get; set; }
        public string CP_CCODIGO { get; set; }
        public string CP_CTIPDOC { get; set; }
        public string CP_CNUMDOC { get; set; }
        public string CP_CFECDOC { get; set; }
        public string CP_CFECVEN { get; set; }
        public string CP_CFECREC { get; set; }
        public string CP_CSITUAC { get; set; }
        public string CP_CFECCOM { get; set; }
        public string CP_CSUBDIA { get; set; }
        public string CP_CCOMPRO { get; set; }
        public string CP_CDEBHAB { get; set; }
        public string CP_CCODMON { get; set; }
        public decimal CP_NTIPCAM { get; set; }
        public decimal CP_NIMPOMN { get; set; }
        public decimal CP_NIMPOUS { get; set; }
        public decimal CP_NSALDMN { get; set; }
        public decimal CP_NSALDUS { get; set; }
        public decimal CP_NIGVMN { get; set; }
        public decimal CP_NIGVUS { get; set; }
        public decimal CP_NIMP2MN { get; set; }
        public decimal CP_NIMP2US { get; set; }
        public decimal CP_NIMPAJU { get; set; }
        public string CP_CCUENTA { get; set; }
        public string CP_CAREA { get; set; }
        public string CP_CFECUBI { get; set; }
        public string CP_CTDOCRE { get; set; }
        public string CP_CNDOCRE { get; set; }
        public string CP_CFDOCRE { get; set; }
        public string CP_CTDOCCO { get; set; }
        public DateTime CP_DFECDOC { get; set; }
        public decimal subtotalSoles { get; set; }
        public decimal subtotalDolares { get; set; }
        public DateTime CP_DFECCOM { get; set; }
    }
}