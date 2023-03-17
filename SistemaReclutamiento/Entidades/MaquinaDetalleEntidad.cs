using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades {
    public class MaquinaDetalleEntidad {
        public int CodMaquina { get; set; }
        public int CodLinea { get; set; }
        public int CodJuego { get; set; }
        public int CodSala { get; set; }
        public int CodModeloMaquina { get; set; }
        public int CodMarcaMaquina { get; set; }
        public int CodContrato { get; set; }
        public int CodFicha { get; set; }
        public string CodMaquinaLey { get; set; }
        public string NombreLinea { get; set; }
        public string NroSerie { get; set; }
        public string NombreJuego { get; set; }
        public string NombreSala { get; set; }
        public string NombreModeloMaquina { get; set; }
        public string DescripcionContrato { get; set; }
        public string NombreFicha { get; set; }
        public string NombreMarcaMaquina { get; set; }
        public double Token { get; set; }
    }
}