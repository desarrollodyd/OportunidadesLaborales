using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades
{
    public class usuarioEntidad
    {
      public int usuarioId {get;set;}
      public int personaId { get; set; }
      public string usuarioEmail { get; set; }
      public string usuarioContrasenia { get; set; }
      public string usuarioToken { get; set; }
      public string usuarioAvatarExtension { get; set; }
      public int usuarioEstado { get; set; }
      public DateTime usuarioFechaCreacion { get; set; }
      public int usuarioValidado { get; set; }
    }
}