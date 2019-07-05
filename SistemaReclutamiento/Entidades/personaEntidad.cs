using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class personaEntidad
    {
        public int personaId { get; set; }
        public string personaDni { get; set; }
        public string personaNombre { get; set; }
        public string personaApellidoPaterno { get; set; }
        public string personaApellidoMaterno { get; set; }
        public string personaEmail { get; set; }
        public int personaEstado { get; set; }
        public DateTime personaFechaNacimiento { get; set; }
        public string personaDireccion { get; set; }
        public string personaContacto1 { get; set; }
        public string personaContacto2 { get; set; }
        public string personaTelefono { get; set; }
        public string personaSexo { get; set; }
        public string personaEstadoCivil { get; set; }
    }
}