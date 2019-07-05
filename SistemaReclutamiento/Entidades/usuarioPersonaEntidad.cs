using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class usuarioPersonaEntidad
    {
        public int usuarioId { get; set; }
        public int personaId { get; set; }
        public string usuarioEmail { get; set; }
        public string usuarioContrasenia { get; set; }
        public string usuarioValidado { get; set; }
        public string personaDni { get; set; }
        public string personaNombre { get; set; }
        public string personaApellidoPaterno { get; set; }
        public string personaApellidoMaterno { get; set; }
        public string personaEmail { get; set; }
    }
}