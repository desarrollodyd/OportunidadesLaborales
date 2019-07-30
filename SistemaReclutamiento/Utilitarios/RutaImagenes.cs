using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace SistemaReclutamiento.Utilitarios
{
    public class RutaImagenes
    {
        public void imagenPostulante_CV(string name)
        {
            string postulante_image = @""+ConfigurationManager.AppSettings["PathImagenesPerfil"] +"/"+name;
            if (postulante_image != null)
            {
                byte[] imagebytes = System.IO.File.ReadAllBytes(postulante_image);
                string base64String = Convert.ToBase64String(imagebytes);
                HttpContext.Current.Session["rutaPerfil"] = base64String;
            }
            
        }

        
    }
}