using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class DetalleMovAuxTitoEntidad
    {
        public double TitoCortesia { get; set; }
        public double TitoCortesiaNoDest { get; set; }
        public double TitoPromocion { get; set; }
        public double TitoPromocionNoDest { get; set; }
        public int Estado { get; set; }
        public DateTime FechaTicketIni { get; set; }
        public DateTime FechaTicketFin { get; set; }
    }
}