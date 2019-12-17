using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaReclutamiento.Entidades.Postulante
{
    public class PostulanteFavoritosEntidad
    {
        public int posfav_id { get; set; }
        public int fk_postulante{get;set;}
        public int fk_oferta_laboral { get; set; }
        public string posfav_estado { get; set; }
        public bool posfav_notificar { get; set; }
    }
}